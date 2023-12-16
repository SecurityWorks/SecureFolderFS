﻿using System;
using System.Threading;
using System.Threading.Tasks;
using SecureFolderFS.Shared.Helpers;
using SecureFolderFS.Shared.Utilities;

namespace SecureFolderFS.Shared.Extensions
{
    public static class ValidationExtensions
    {
        public static async Task<IResult> TryValidateAsync<T>(this IAsyncValidator<T> validator, T value, CancellationToken cancellationToken)
        {
            try
            {
                await validator.ValidateAsync(value, cancellationToken);
                return CommonResult.Success;
            }
            catch (Exception ex)
            {
                return CommonResult.Failure(ex);
            }
        }
    }
}
