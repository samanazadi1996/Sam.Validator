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
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        private string? currentField;
        private object? value;
        private string? lastError; // آخرین خطای اضافه شده

        T Instance => (T)(object)this!;
        public abstract void HandleValidation(ValidationContext validationContext);

        protected T RuleFor(Expression<Func<T, object?>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression), "Validation expression is required.");

            currentField = GetFieldName(expression);
            value = expression.Compile().Invoke(Instance);

            return Instance;
        }
        protected T NotNull()
        {
            if (value is null)
                AddError(Localizer.Get(ValidationMessages.ValueCannotBeNull));

            return Instance;
        }

        protected T NotEmpty()
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                AddError(Localizer.Get(ValidationMessages.ValueCannotBeEmpty));

            return Instance;
        }

        protected T Min(int min)
        {
            if (int.TryParse(value?.ToString(), out var _value) && _value < min)
                AddError(Localizer.Get(ValidationMessages.MinimumValue, min));

            return Instance;
        }

        protected T Max(int max)
        {
            if (int.TryParse(value?.ToString(), out var _value) && _value > max)
                AddError(Localizer.Get(ValidationMessages.MaximumValue, max));

            return Instance;
        }

        protected T Length(int minLength, int maxLength)
        {
            var value = this.value?.ToString();
            if (value != null && (value.Length < minLength || value.Length > maxLength))
                AddError(Localizer.Get(ValidationMessages.LengthRange, minLength, maxLength));

            return Instance;
        }

        protected T Matches(string pattern)
        {
            var value = this.value?.ToString();
            if (value != null && !Regex.IsMatch(value, pattern))
                AddError(Localizer.Get(ValidationMessages.PatternMismatch));

            return Instance;
        }

        protected T Must(Func<bool> condition)
        {
            if (!condition())
                AddError(Localizer.Get(ValidationMessages.CustomConditionFailed));

            return Instance;
        }

        protected T Email()
        {
            var value = this.value?.ToString();
            if (!string.IsNullOrWhiteSpace(value) && !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                AddError(Localizer.Get(ValidationMessages.InvalidEmail));

            return Instance;
        }

        protected T In(params string[] allowed)
        {
            var value = this.value?.ToString();
            if (!string.IsNullOrWhiteSpace(value) && !allowed.Contains(value))
                AddError(Localizer.Get(ValidationMessages.MustBeOneOf, string.Join(", ", allowed)));

            return Instance;
        }

        protected T GreaterThan<TValue>(TValue min) where TValue : IComparable
        {
            if (value is TValue comparable && comparable.CompareTo(min) <= 0)
                AddError(Localizer.Get(ValidationMessages.GreaterThan, min));

            return Instance;
        }

        protected T Must(Func<T, bool> predicate)
        {
            if (!predicate(Instance))
                AddError(Localizer.Get(ValidationMessages.CustomConditionFailed));

            return Instance;
        }

        protected T LessThan<TValue>(TValue max) where TValue : IComparable
        {
            if (value is TValue comparable && comparable.CompareTo(max) >= 0)
                AddError(Localizer.Get(ValidationMessages.LessThan, max));

            return Instance;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            HandleValidation(validationContext);

            return errors.SelectMany(p => p.Value.Distinct().Select(x => new ValidationResult(x, new string[] { p.Key })));
        }

        void AddError(string message)
        {
            if (currentField != null)
            {
                if (!errors.ContainsKey(currentField))
                    errors[currentField] = new List<string>();

                errors[currentField].Add(message);
                lastError = message;
            }
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

        protected T WithMessage(string errorMessage)
        {
            if (currentField != null && lastError != null && errors.ContainsKey(currentField))
            {
                var list = errors[currentField];
                var index = list.LastIndexOf(lastError);
                if (index >= 0)
                {
                    list[index] = errorMessage;
                    lastError = errorMessage;
                }
            }
            return Instance;
        }
    }

}
