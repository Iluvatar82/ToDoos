using System.Runtime.CompilerServices;

namespace Core.Validation
{
    public static class Validator
    {
        public static void NotNull<T>(this T value, [CallerArgumentExpression(nameof(value))] string? expression = null)
        {
            if (value == null)
                throw new ArgumentNullException($"Value \"{expression}\" is null");
        }

        public static void NotNullOrEmpty<T>(this T value, [CallerArgumentExpression(nameof(value))] string? expression = null)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                throw new ArgumentNullException($"Value \"{expression}\" is null");
        }

        public static void Satisfies<T>(this T value, Func<T, bool> satisfies, [CallerArgumentExpression(nameof(value))] string? valueExpression = null, [CallerArgumentExpression(nameof(satisfies))] string? satisfiesExpression = null)
        {
            if (!satisfies(value))
                throw new ArgumentException($"Value \"{valueExpression}\" does not satisfy \"{satisfiesExpression}\"");
        }
    }
}
