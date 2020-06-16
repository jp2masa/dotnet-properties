using System;
using System.ComponentModel.DataAnnotations;

using NuGet.Packaging.Licenses;

namespace DotNet.Properties.DataAnnotations
{
    internal sealed class NuGetPackageLicenseExpressionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is string str))
            {
                throw new InvalidOperationException();
            }

            try
            {
                var expression = NuGetLicenseExpression.Parse(str);

                if (expression.HasOnlyStandardIdentifiers())
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult("Unknown license identifier(s)!");
            }
            catch (NuGetLicenseExpressionParsingException e)
            {
                return new ValidationResult(e.Message);
            }
        }
    }
}
