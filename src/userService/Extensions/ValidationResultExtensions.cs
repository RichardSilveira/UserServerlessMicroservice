﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public ModelFailure()
        {
        }

        public ModelFailure(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public static ModelFailure BuildModelFailure<T>(Expression<Func<T, object>> property, string errorMessage) =>
            new ModelFailure()
            {
                PropertyName = ((MemberExpression) property.Body).Member.Name,
                ErrorMessage = errorMessage
            };
    }
}