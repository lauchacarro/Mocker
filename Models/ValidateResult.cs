using Mocker.Enums;

namespace Mocker.Models
{
    public class ValidateResult
    {
        public bool Success { get; set; }
        public ErrorMessageCodeEnum[] ErrorMessages { get; set; }
    }
}
