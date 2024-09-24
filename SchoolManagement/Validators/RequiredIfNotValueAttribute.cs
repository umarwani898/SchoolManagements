using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Validators
{
    public class RequiredIfNotValueAttribute : ValidationAttribute
    {
        private readonly string _defaultValue;

        public RequiredIfNotValueAttribute(string defaultValue)
        {
            _defaultValue = defaultValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value.ToString() == _defaultValue)
            {
                return new ValidationResult(ErrorMessage ?? "This field is required.");
            }

            return ValidationResult.Success;
        }
    }
}
