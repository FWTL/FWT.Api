using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FWT.Infrastructure.Telegram
{
    public static class TelegramRequest
    {
        public static async Task<TResult> HandleAsync<TResult>(Func<Task<TResult>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                ThrowValidationException(ex);
            }

            throw new Exception("Unsupported path");
        }

        public static async Task HandleAsync(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                ThrowValidationException(ex);
            }
        }

        public static async Task<TResult> HandleAsync<TResult>(Func<Task<TResult>> func, CustomContext context)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                context.AddFailure(ex.Message);
                return default(TResult);
            }

            throw new Exception("Unsupported path");
        }

        private static void ThrowValidationException(Exception ex)
        {
            throw new ValidationException(new List<ValidationFailure>()
            {
                new ValidationFailure(ex.GetType().FullName, ex.Message)
            });
        }
    }
}