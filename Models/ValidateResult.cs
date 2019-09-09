using System.Collections.Generic;
using Mocker.Enums;

namespace Mocker.Models
{
    public class ValidateResult
    {
        public bool Success { get; set; }
        public IEnumerable<ErrorMessageCodeEnum> ErrorMessages { get; set; }
    }
}
