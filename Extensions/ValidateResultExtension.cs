using Mocker.Enums;
using Mocker.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mocker.Extensions
{
    public static class ValidateResultExtension
    {
        public static async Task<ValidateResult> Success(this ValidateResult result, Func<Task> callback)
        {
            if (result.Success)
                await callback();

            return result;
        }

        public static ValidateResult Error(this ValidateResult result, Action<IEnumerable<ErrorMessageCodeEnum>> callback)
        {
            if (!result.Success)
                callback(result.ErrorMessages);

            return result;
        }
    }
}
