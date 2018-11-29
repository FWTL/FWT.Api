using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FWT.Infrastructure.Telegram.Parsers
{
    public class DialogParser
    {
        private static readonly Dictionary<string, Func<IDialog, TVector<IUser>, TelegramDialog>> Switch = new Dictionary<string, Func<IDialog, TVector<IUser>, TelegramDialog>>()
        {
              { typeof(TDialog).FullName, (dialog,users) => { return Parse(dialog as TDialog, users); } },
        };

        private static TelegramDialog Parse(TDialog dialog, TVector<TUser> users)
        {
            var peer = dialog.Peer.As<TPeerUser>();
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

        public static TelegramDialog Parse(IDialog dialog, TVector<IUser> users)
        {
            return Switch[dialog.GetType().FullName](dialog, users);
        }
    }
}