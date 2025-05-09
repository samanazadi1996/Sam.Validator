using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sam.Validator
{
    public abstract class Validator<T> : IValidatableObject
    {
        private readonly ICollection<ValidationResult> result = new List<ValidationResult>();
        private Func<T, object?>? context;

        public abstract void Validate();

        protected T RuleFor(Func<T, object?> context)
        {
            this.context = context;

            if (context == null)
                result.Add(new ValidationResult("Validation context is not set."));

            return (T)(object)this!;
        }

        protected T NotNull()
        {
            var value = context?.Invoke((T)(object)this!);
            if (value is null)
                result.Add(new ValidationResult("Value cannot be null."));
            return (T)(object)this!;
        }

        protected T NotEmpty()
        {
            var value = context?.Invoke((T)(object)this!);
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                result.Add(new ValidationResult("Value cannot be empty."));
            return (T)(object)this!;
        }

        protected T Min(int min)
        {
            if (int.TryParse(context?.Invoke((T)(object)this!)?.ToString(), out var value) && value < min)
                result.Add(new ValidationResult($"Minimum allowed value is {min}."));
            return (T)(object)this!;
        }

        protected T Max(int max)
        {
            if (int.TryParse(context?.Invoke((T)(object)this!)?.ToString(), out var value) && value > max)
                result.Add(new ValidationResult($"Maximum allowed value is {max}."));
            return (T)(object)this!;
        }

        protected T Length(int minLength, int maxLength)
        {
            var value = context?.Invoke((T)(object)this!)?.ToString();
            if (value != null && (value.Length < minLength || value.Length > maxLength))
                result.Add(new ValidationResult($"Value must be between {minLength} and {maxLength} characters."));
            return (T)(object)this!;
        }

        protected T Matches(string pattern)
        {
            var value = context?.Invoke((T)(object)this!)?.ToString();
            if (value != null && !Regex.IsMatch(value, pattern))
                result.Add(new ValidationResult($"Value does not match the required pattern."));
            return (T)(object)this!;
        }

        protected T Must(Func<bool> condition)
        {
            if (!condition())
                result.Add(new ValidationResult("Custom validation failed."));
            return (T)(object)this!;
        }

        protected T Email()
        {
            var value = context?.Invoke((T)(object)this!)?.ToString();
            if (!string.IsNullOrWhiteSpace(value) && !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                result.Add(new ValidationResult("Invalid email format."));
            return (T)(object)this!;
        }

        protected T In(params string[] allowed)
        {
            var value = context?.Invoke((T)(object)this!)?.ToString();
            if (!string.IsNullOrWhiteSpace(value) && !allowed.Contains(value))
                result.Add(new ValidationResult($"Value must be one of the following: {string.Join(", ", allowed)}"));
            return (T)(object)this!;
        }

        protected T GreaterThan<TValue>(TValue min) where TValue : IComparable
        {
            var value = context?.Invoke((T)(object)this!);
            if (value is TValue comparable && comparable.CompareTo(min) <= 0)
                result.Add(new ValidationResult($"Value must be greater than {min}."));
            return (T)(object)this!;
        }

        protected T Must(Func<T, bool> predicate, string? errorMessage = null)
        {
            if (!predicate((T)(object)this!))
            {
                result.Add(new ValidationResult(errorMessage ?? "Custom condition failed."));
            }

            return (T)(object)this!;
        }

        protected T LessThan<TValue>(TValue max) where TValue : IComparable
        {
            var value = context?.Invoke((T)(object)this!);
            if (value is TValue comparable && comparable.CompareTo(max) >= 0)
                result.Add(new ValidationResult($"Value must be less than {max}."));
            return (T)(object)this!;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            result.Clear();
            Validate();
            return result;
        }
    }

}
