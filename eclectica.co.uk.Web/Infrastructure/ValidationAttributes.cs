using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace eclectica.co.uk.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public sealed class MustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "{0} must match {1}.";

        private string _fieldName;

        public MustMatchAttribute(string fieldName) : base(_defaultErrorMessage)
        {
            _fieldName = fieldName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object matchValue = context.ObjectType.GetProperty(_fieldName).GetValue(context.ObjectInstance, null);

            if (!Object.Equals(value, matchValue))
                return new ValidationResult(String.Format(CultureInfo.CurrentUICulture, _defaultErrorMessage, context.DisplayName, _fieldName));

            return ValidationResult.Success;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class MustEqualAttribute : ValidationAttribute
    {
        private string[] _validValues;
        private bool _caseInsensitive;

        public MustEqualAttribute(string errorMessage, string[] validValues, bool caseInsensitive) : base(errorMessage)
        {
            _caseInsensitive = caseInsensitive;

            _validValues = (_caseInsensitive) ? validValues.Select(x => x.ToLower()).ToArray() : validValues;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string val = (value as string).Trim();

            if (_caseInsensitive)
                val = val.ToLower();

            return (_validValues.Contains(val));
        }
    }
}