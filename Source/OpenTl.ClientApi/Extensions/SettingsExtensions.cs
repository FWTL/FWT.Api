﻿namespace OpenTl.ClientApi.Extensions
{
    using OpenTl.ClientApi.Exceptions;
    using OpenTl.ClientApi.MtProto;

    public static class SettingsExtensions
    {
        public static void EnsureUserAuthorized(this IClientSettings clientSession)
        {
            if (!clientSession.IsUserAuthorized())
            {
                throw new UserNotAuthorizeException();
            }
        }

        public static bool IsUserAuthorized(this IClientSettings clientSession)
        {
            return clientSession.ClientSession.UserId.HasValue;
        }
    }
}