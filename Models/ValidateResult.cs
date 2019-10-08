using Mocker.Enums;
using System.Collections.Generic;

namespace Mocker.Models
{
    public class ValidateResult
    {
        public bool Success { get; set; }
        public IEnumerable<ErrorMessageCodeEnum> ErrorMessages { get; set; }
    }
}
