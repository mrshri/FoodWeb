using System.ComponentModel.DataAnnotations;

namespace Food.Web.Utilities
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int  _maxFileSize;
        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null) { 
             if(file.Length > (_maxFileSize * 1024 * 1024))
                {
                    return new ValidationResult($"File size should be less than {_maxFileSize} MB!");
                }
            }
            return ValidationResult.Success;
        }
    }
}
