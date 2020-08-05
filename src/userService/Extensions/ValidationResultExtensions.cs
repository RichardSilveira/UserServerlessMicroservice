using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace UserService.Extensions
{
    public static class ValidationResultExtensions
    {
        public static IEnumerable<ModelFailure> ToModelFailures(this IList<ValidationFailure> errors)
        {
            if (!errors.Any())
                yield break;

            foreach (var error in errors)
            {
                yield return new ModelFailure(error.PropertyName, error.ErrorMessage);
            }
        }
    }

    public class ModelFailure
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public ModelFailure(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }
}