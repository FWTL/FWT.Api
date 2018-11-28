using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FWT.Infrastructure.Telegram
{
    public static class TelegramRequest
    {
        public static async Task<TResult> Handle<TResult>(Func<Task<TResult>> func)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                ThrowValidationException(ex);
            }

            throw new Exception("Unsupported Path");
        }

        public static async Task Handle(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                ThrowValidationException(ex);
            }

            throw new Exception("Unsupported Path");
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