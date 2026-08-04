using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Core.Attribute
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    internal class FileValidationAttribute : ValidationAttribute
    {

        #region Private members


        private readonly int _size;
        private readonly string _sizeErrorMessage;
        private readonly bool _isSize = false;

        private readonly string[]? _extensions;
        private readonly string _extensionsErrorMessage;
        private readonly bool _isExtensions = false;


        #endregion Private members


        #region Constructors


        /// <summary>
        /// Init File Validation Attribute
        /// </summary>
        internal FileValidationAttribute() { }


        /// <summary>
        /// Init File Validation Attribute
        /// </summary>
        /// <param name="size">In bytes 10,48,576 (1 Mega Byte) = 1024 (Bytes) * 1024 (Kilo byte) * 1 (Mega byte)</param>
        /// <param name="sizeErrorMessage">Size invalid error message</param>
        internal FileValidationAttribute(int size, string? sizeErrorMessage = null)
        {
            _isSize = true;
            _size = size;
            _sizeErrorMessage = sizeErrorMessage;
        }


        /// <summary>
        /// Init File Validation Attribute
        /// </summary>
        /// <param name="extensions">Validate file extension</param>
        /// <param name="extensionsErrorMessage">Extenstion invalid error message</param>
        internal FileValidationAttribute(string[] extensions, string? extensionsErrorMessage = null)
        {
            _isExtensions = true;
            _extensions = extensions;
            _extensionsErrorMessage = extensionsErrorMessage;
        }


        /// <summary>
        /// Init File Validation Attribute
        /// </summary>
        /// <param name="size">In bytes 10,48,576 (1 Mega Byte) = 1024 (Bytes) * 1024 (Kilo byte) * 1 (Mega byte)</param>
        /// <param name="sizeErrorMessage">Size invalid error message</param>
        /// <param name="extensions">Validate file extension</param>
        /// <param name="extensionsErrorMessage">Extenstion invalid error message</param>
        internal FileValidationAttribute(int size, string[] extensions, string? sizeErrorMessage = null, string? extensionsErrorMessage = null)
            : this(extensions, extensionsErrorMessage)
        {
            _isSize = true;
            _size = size;
            _sizeErrorMessage = sizeErrorMessage;
        }


        #endregion Constructors


        #region Override methods


        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {

                IFormFile file = value as IFormFile;

                if (file != null)
                {
                    if (_isExtensions && !_extensions.Contains(new FileInfo(file.FileName).Extension.ToLower()))
                        return new ValidationResult(_extensionsErrorMessage
                            ?? string.Format("{0} invalid file.", validationContext.DisplayName));

                    else if (_isSize && file.Length > _size)
                        return new ValidationResult(_sizeErrorMessage
                            ?? string.Format("{0} max length reached.", validationContext.DisplayName));
                }

                List<IFormFile> files = value as List<IFormFile>;

                if (files != null)
                    for (int i = 0; i < files.Count; i++)
                        if (_isExtensions && !_extensions.Contains(new FileInfo(files[i].FileName).Extension.ToLower()))
                            return new ValidationResult(string.IsNullOrEmpty(_extensionsErrorMessage)
                                ? string.Format("{0}'s file {1} invalid file.", validationContext.DisplayName, i)
                                : string.Format(_extensionsErrorMessage, i));

                        else if (_isSize && files[i].Length > _size)
                            return new ValidationResult(string.IsNullOrEmpty(_sizeErrorMessage)
                                ? string.Format("{0}'s file {1} max length reached.", validationContext.DisplayName, i)
                                : string.Format(_sizeErrorMessage, i));
            }

            return ValidationResult.Success;
        }


        #endregion Override methods
    }
}