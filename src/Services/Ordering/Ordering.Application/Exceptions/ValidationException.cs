using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ArgumentException 
    {

        public ValidationException() : base("One or more validation failures have occured.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures):this()
        {
            Errors = failures.
                GroupBy(x=>x.PropertyName,e=>e.ErrorMessage)
                .ToDictionary(failureGroup=> failureGroup.Key,failureGroup => failureGroup.ToArray());
        }

        public IDictionary<string, string[]> Errors { get; private set; }
    }
}
