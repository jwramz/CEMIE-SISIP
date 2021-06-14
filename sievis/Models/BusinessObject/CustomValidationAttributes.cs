using System.ComponentModel.DataAnnotations;

namespace sievis.Models
{
    #region [RequiredTrueAorB]
    public class RequiredTrueAorBAttribute : ValidationAttribute
    {
        RequiredAttribute _innerAttribute = new RequiredAttribute();
        public string _dependentProperty { get; set; }
        public string _message { get; set; }

        public RequiredTrueAorBAttribute(string dependentProperty, string message)
        {
            this._dependentProperty = dependentProperty;
            this._message = message;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = validationContext.ObjectType.GetProperty(_dependentProperty);
            if (field != null)
            {
                var thisValue = value;
                var dependentValue = field.GetValue(validationContext.ObjectInstance, null);

                if (thisValue != null && ((bool)thisValue) == true)
                {
                    return ValidationResult.Success;
                }
                if (dependentValue != null && ((bool)dependentValue) == true)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(this._message);
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(_dependentProperty));
            }
        }
    }
    #endregion
}