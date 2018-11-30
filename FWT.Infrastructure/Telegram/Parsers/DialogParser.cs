using FWT.Core.Extensions;
using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FWT.Infrastructure.Telegram.Parsers
{
    public class DialogParser
    {
        private static readonly Dictionary<string, Func<IDialog, List<TUser>, TelegramDialog>> Switch = new Dictionary<string, Func<IDialog, List<TUser>, TelegramDialog>>()
        {
              { typeof(TDialog).FullName, (dialog,users) => { return Parse(dialog as TDialog, users); } },
        };

        private static TelegramDialog Parse(TDialog dialog, List<TUser> users)
        {
            var peer = dialog.Peer.As<TPeerUser>();
            if (peer.IsNotNull())
            {
                var user = users.First(u => u.Id == peer.UserId);

                var appDialog = new TelegramDialog()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.Username,
                };

                return appDialog;
            }

            return null;
        }

        public static TelegramDialog Parse(IDialog dialog, List<TUser> users)
        {
            return Switch[dialog.GetType().FullName](dialog, users);
        }
    }
}