using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Sam.Validator
{
    public abstract class Validator<T> : IValidatableObject
    {
        private Dictionary<string, List<string>> Errors;
        private object Value;
        public abstract void Validate();

        protected T RuleFor(Expression<Func<T, object?>> expression)
        {
            Errors ??= new Dictionary<string, List<string>>();

            string? fieldName = null;

            if (expression.Body is MemberExpression member)
                fieldName = member.Member.Name;

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                fieldName = memberOperand.Member.Name;

            if (expression == null)
                throw new ArgumentNullException(nameof(expression), "Validation expression is not set.");

            if (fieldName != null)
            {
                Errors.TryAdd(fieldName, new List<string>());
                Errors = Errors.OrderBy(p => p.Key == fieldName).ToDictionary(p => p.Key, x => x.Value);
            }

            Value = expression.Compile()((T)(object)this!);

            return (T)(object)this!;
        }

        protected T NotNull()
        {
            if (Value is null)
                AddError("Value cannot be null.");

            return (T)(object)this!;
        }

        protected T NotEmpty()
        {
            if (string.IsNullOrWhiteSpace(Value?.ToString()))
                AddError("Value cannot be empty.");

            return (T)(object)this!;
        }

        protected T Min(int min)
        {
            if (int.TryParse(Value?.ToString(), out var value) && value < min)
                AddError($"Minimum allowed value is {min}.");
            return (T)(object)this!;
        }

        protected T Max(int max)
        {
            if (int.TryParse(Value?.ToString(), out var value) && value > max)
                AddError($"Maximum allowed value is {max}.");
            return (T)(object)this!;
        }

        protected T Length(int minLength, int maxLength)
        {
            var value = Value?.ToString();
            if (value != null && (value.Length < minLength || value.Length > maxLength))
                AddError($"Value must be between {minLength} and {maxLength} characters.");
            return (T)(object)this!;
        }

        protected T Matches(string pattern)
        {
            var value = Value?.ToString();
            if (value != null && !Regex.IsMatch(value, pattern))
                AddError($"Value does not match the required pattern.");
            return (T)(object)this!;
        }

        protected T Must(Func<bool> condition)
        {
            if (!condition())
                AddError("Custom validation failed.");
            return (T)(object)this!;
        }

        protected T Email()
        {
            var value = Value?.ToString();
            if (!string.IsNullOrWhiteSpace(value) && !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                AddError("Invalid email format.");
            return (T)(object)this!;
        }

        protected T In(params string[] allowed)
        {
            var value = Value?.ToString();
            if (!string.IsNullOrWhiteSpace(value) && !allowed.Contains(value))
                AddError($"Value must be one of the following: {string.Join(", ", allowed)}");
            return (T)(object)this!;
        }

        protected T GreaterThan<TValue>(TValue min) where TValue : IComparable
        {
            if (Value is TValue comparable && comparable.CompareTo(min) <= 0)
                AddError($"Value must be greater than {min}.");
            return (T)(object)this!;
        }

        protected T Must(Func<T, bool> predicate, string? errorMessage = null)
        {
            if (!predicate((T)(object)this!))
            {
                AddError(errorMessage ?? "Custom condition failed.");
            }

            return (T)(object)this!;
        }

        protected T LessThan<TValue>(TValue max) where TValue : IComparable
        {
            if (Value is TValue comparable && comparable.CompareTo(max) >= 0)
                AddError($"Value must be less than {max}.");
            return (T)(object)this!;
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Validate();

            return Errors.SelectMany(p => p.Value.Distinct().Select(x => new ValidationResult(x, new string[] { p.Key })));
        }

        void AddError(string message)
        {
            Errors.LastOrDefault().Value.Add(message);
        }
    }

}
