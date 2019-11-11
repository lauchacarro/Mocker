using System;

namespace Mocker.Web.Extensions.Validations
{
    public static class ValidateFileSizeExtension
    {
        private const int HundredMegaBytes = 100 * 1000 * 1000;

        public static long IsGreaterThan100MB(this long lenght, Action callback)
        {
            if (lenght > HundredMegaBytes)
                callback();

            return lenght;
        }
    }
}
