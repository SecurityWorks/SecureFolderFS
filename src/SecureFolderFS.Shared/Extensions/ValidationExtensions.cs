﻿using System;
using System.Threading;
using System.Threading.Tasks;
using SecureFolderFS.Shared.Helpers;
using SecureFolderFS.Shared.ComponentModel;

namespace SecureFolderFS.Shared.Extensions
{
    public static class ValidationExtensions
    {
        public static async Task<IResult> TryValidateAsync<T>(this IAsyncValidator<T> validator, T value, CancellationToken cancellationToken)
        {
            try
            {
                await validator.ValidateAsync(value, cancellationToken);
                return Result.Success;
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
