using Mocker.Web.Enums;
using System.Collections.Generic;

namespace Mocker.Web.Models
{
    public class ValidateResult
    {
        public bool Success { get; set; }
        public IEnumerable<ErrorMessageCodeEnum> ErrorMessages { get; set; }
    }
}
