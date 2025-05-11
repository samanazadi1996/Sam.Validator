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
        private Dictionary<string, List<string>> errors;
        private string? currentField;
        private object? value;
        public abstract void Validate();

        protected T RuleFor(Expression<Func<T, object?>> expression)
        {
            errors ??= new Dictionary<string, List<string>>();

            if (expression == null)
                throw new ArgumentNullException(nameof(expression), "Validation expression is required.");

            currentField = GetFieldName(expression);
            value = expression.Compile().Invoke((T)(object)this!);

            if (!errors.ContainsKey(currentField))
                errors[currentField] = new List<string>();

            return (T)(object)this!;
        }
        protected T NotNull()
        {
            if (value is null)
                AddError("Value cannot be null.");

            return (T)(object)this!;
        }

        protected T NotEmpty()
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                AddError("Value cannot be empty.");

            return (T)(object)this!;
        }

        protected T Min(int min)
        {
            if (int.TryParse(value?.ToString(), out var _value) && _value < min)
                AddError($"Minimum allowed value is {min}.");
            return (T)(object)this!;
        }

        protected T Max(int max)
        {
            if (int.TryParse(value?.ToString(), out var _value) && _value > max)
                AddError($"Maximum allowed value is {max}.");
            return (T)(object)this!;
        }

        protected T Length(int minLength, int maxLength)
        {
            var value = this.value?.ToString();
            if (value != null && (value.Length < minLength || value.Length > maxLength))
                AddError($"Value must be between {minLength} and {maxLength} characters.");
            return (T)(object)this!;
        }

        protected T Matches(string pattern)
        {
            var value = this.value?.ToString();
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
            var value = this.value?.ToString();
            if (!string.IsNullOrWhiteSpace(value) && !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                AddError("Invalid email format.");
            return (T)(object)this!;
        }

        protected T In(params string[] allowed)
        {
            var value = this.value?.ToString();
            if (!string.IsNullOrWhiteSpace(value) && !allowed.Contains(value))
                AddError($"Value must be one of the following: {string.Join(", ", allowed)}");
            return (T)(object)this!;
        }

        protected T GreaterThan<TValue>(TValue min) where TValue : IComparable
        {
            if (value is TValue comparable && comparable.CompareTo(min) <= 0)
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
            if (value is TValue comparable && comparable.CompareTo(max) >= 0)
                AddError($"Value must be less than {max}.");
            return (T)(object)this!;
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            Validate();

            return errors.SelectMany(p => p.Value.Distinct().Select(x => new ValidationResult(x, new string[] { p.Key })));
        }

        void AddError(string message)
        {
            if (currentField != null && errors.ContainsKey(currentField))
                errors[currentField].Add(message);
        }
        private static string GetFieldName(Expression<Func<T, object?>> expression)
        {
            return expression.Body switch
            {
                MemberExpression member => member.Member.Name,
                UnaryExpression unary when unary.Operand is MemberExpression memberOperand => memberOperand.Member.Name,
                _ => throw new InvalidOperationException("Invalid expression provided.")
            };
        }

    }

}
