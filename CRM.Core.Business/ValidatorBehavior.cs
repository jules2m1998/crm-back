using CRM.Core.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business
{
    public class ValidatorBehavior<T> where T : notnull
    {
        /// <summary>
        /// Check if Model is valid
        /// </summary>
        /// <param name="toValidate"></param>
        /// <exception cref="BaseException"></exception>
        public static void Validate(T toValidate)
        {
            ValidationContext context = new(toValidate);
            List<ValidationResult> results = new();

            bool isValid = Validator.TryValidateObject(toValidate, context, results, true);
            if (!isValid)
            {
                var errors = new Dictionary<string, List<string>>();
                results.ForEach(result =>
                {
                    errors.Add(string.Join(' ', result.MemberNames), new List<string> { result.ErrorMessage });
                });
                throw new BaseException(errors);
            }
        }
    }
}
