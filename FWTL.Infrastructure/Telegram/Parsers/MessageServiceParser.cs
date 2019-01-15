using FWTL.Events.Telegram.Messages;
using OpenTl.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using static FWTL.Events.Telegram.Enums;

namespace FWTL.Infrastructure.Telegram.Parsers
{
    public static class MessageServiceParser
    {
        private static readonly Dictionary<string, Func<IMessageAction, Message>> Switch = new Dictionary<string, Func<IMessageAction, Message>>()
        {
            { typeof(TMessageActionChannelCreate).FullName, x => { return Parse(x as TMessageActionChannelCreate); } },
            { typeof(TMessageActionPinMessage).FullName, x => { return Parse(x as TMessageActionPinMessage); } },
            { typeof(TMessageActionChatCreate).FullName, x => { return Parse(x as TMessageActionChatCreate); } },
            { typeof(TMessageActionChatMigrateTo).FullName, x => { return Parse(x as TMessageActionChatMigrateTo); } },
            { typeof(TMessageActionSecureValuesSentMe).FullName, x => { return Parse(x as TMessageActionSecureValuesSentMe); } },
            { typeof(TMessageActionHistoryClear).FullName, x => { return Parse(x as TMessageActionHistoryClear); } },
            { typeof(TMessageActionEmpty).FullName, x => { return Parse(x as TMessageActionEmpty); } },
            { typeof(TMessageActionChatEditPhoto).FullName, x => { return Parse(x as TMessageActionChatEditPhoto); } },
            { typeof(TMessageActionChannelMigrateFrom).FullName, x => { return Parse(x as TMessageActionChannelMigrateFrom); } },
            { typeof(TMessageActionChatAddUser).FullName, x => { return Parse(x as TMessageActionChatAddUser); } },
            { typeof(TMessageActionPaymentSentMe).FullName, x => { return Parse(x as TMessageActionPaymentSentMe); } },
            { typeof(TMessageActionSecureValuesSent).FullName, x => { return Parse(x as TMessageActionSecureValuesSent); } },
            { typeof(TMessageActionChatEditTitle).FullName, x => { return Parse(x as TMessageActionChatEditTitle); } },
            { typeof(TMessageActionChatDeletePhoto).FullName, x => { return Parse(x as TMessageActionChatDeletePhoto); } },
            { typeof(TMessageActionGameScore).FullName, x => { return Parse(x as TMessageActionGameScore); } },
            { typeof(TMessageActionPaymentSent).FullName, x => { return Parse(x as TMessageActionPaymentSent); } },
            { typeof(TMessageActionPhoneCall).FullName, x => { return Parse(x as TMessageActionPhoneCall); } },
            { typeof(TMessageActionChatJoinedByLink).FullName, x => { return Parse(x as TMessageActionChatJoinedByLink); } },
            { typeof(TMessageActionScreenshotTaken).FullName, x => { return Parse(x as TMessageActionScreenshotTaken); } },
            { typeof(TMessageActionBotAllowed).FullName, x => { return Parse(x as TMessageActionBotAllowed); } },
            { typeof(TMessageActionCustomAction).FullName, x => { return Parse(x as TMessageActionCustomAction); } },
            { typeof(TMessageActionChatDeleteUser).FullName, x => { return Parse(x as TMessageActionChatDeleteUser); } },
        };

        public static Message Parse(IMessageAction message)
        {
            string key = message.GetType().FullName;
            return Switch[key](message);
        }

        private static Message Parse(TMessageActionBotAllowed messageActionBotAllowed)
        {
            return new Message
            {
                Text = messageActionBotAllowed.Domain,
                Action = TelegramMessageAction.BotAllowed
            };
        }

        private static Message Parse(TMessageActionChannelCreate messageActionChannelCreate)
        {
            return new Message
            {
                Text = messageActionChannelCreate.Title,
                Action = TelegramMessageAction.ChannelCreate
            };
        }

        private static Message Parse(TMessageActionChannelMigrateFrom messageActionChannelMigrateFrom)
        {
            return new Message
            {
                Text = $"{messageActionChannelMigrateFrom.ChatId};{messageActionChannelMigrateFrom.Title}",
                Action = TelegramMessageAction.MigrateFrom
            };
        }

        private static Message Parse(TMessageActionChatAddUser messageActionChatAddUser)
        {
            return new Message
            {
                Text = string.Join(";", messageActionChatAddUser.Users),
                Action = TelegramMessageAction.ChatAddUser
            };
        }

        private static Message Parse(TMessageActionChatCreate messageActionChatCreate)
        {
            return new Message
            {
                Text = messageActionChatCreate.Title,
                Action = TelegramMessageAction.ChatCreate
            };
        }

        private static Message Parse(TMessageActionChatDeletePhoto messageActionChatDeletePhoto)
        {
            return new Message
            {
                Text = "Chat photo deleted",
                Action = TelegramMessageAction.ChatDeletePhoto
            };
        }

        private static Message Parse(TMessageActionChatDeleteUser messageActionChatDeleteUser)
        {
            return new Message
            {
                Text = messageActionChatDeleteUser.UserId.ToString(),
                Action = TelegramMessageAction.ChatDeleteUser
            };
        }

        private static Message Parse(TMessageActionChatEditPhoto messageActionChatEditPhoto)
        {
            return new Message
            {
                Text = $"Photo edited",
                Action = TelegramMessageAction.ChatEditPhoto
            };
        }

        private static Message Parse(TMessageActionChatEditTitle messageActionChatEditTitle)
        {
            return new Message
            {
                Text = "Chat title edited",
                Action = TelegramMessageAction.ChatEditTitle
            };
        }

        private static Message Parse(TMessageActionChatJoinedByLink messageActionChatJoinedByLink)
        {
            return new Message
            {
                Text = messageActionChatJoinedByLink.InviterId.ToString(),
                Action = TelegramMessageAction.ChatJoinedByLink
            };
        }

        private static Message Parse(TMessageActionChatMigrateTo messageActionChatMigrateTo)
        {
            return new Message
            {
                Text = "History Clear",
                Action = TelegramMessageAction.HistoryClear
            };
        }

        private static Message Parse(TMessageActionCustomAction messageActionCustomAction)
        {
            return new Message
            {
                Text = messageActionCustomAction.Message,
                Action = TelegramMessageAction.ActionCustomAction
            };
        }

        private static Message Parse(TMessageActionEmpty messageActionEmpty)
        {
            return new Message
            {
                Text = $"Empty",
                Action = TelegramMessageAction.Empty
            };
        }

        private static Message Parse(TMessageActionGameScore messageActionGameScore)
        {
            return new Message
            {
                Text = $"{messageActionGameScore.GameId};{messageActionGameScore.Score}",
                Action = TelegramMessageAction.GameScore
            };
        }

        private static Message Parse(TMessageActionHistoryClear messageActionHistoryClear)
        {
            return new Message
            {
                Text = "History Clear",
                Action = TelegramMessageAction.HistoryClear
            };
        }

        private static Message Parse(TMessageActionPaymentSent messageActionPaymentSent)
        {
            return new Message
            {
                Text = $"{messageActionPaymentSent.Currency};{messageActionPaymentSent.TotalAmount}",
                Action = TelegramMessageAction.PaymentSent
            };
        }

        private static Message Parse(TMessageActionPaymentSentMe messageActionPaymentSentMe)
        {
            return new Message
            {
                Text = $"{messageActionPaymentSentMe.Currency};{messageActionPaymentSentMe.TotalAmount}",
                Action = TelegramMessageAction.PaymentSentMe
            };
        }

        private static Message Parse(TMessageActionPhoneCall messageActionPhoneCall)
        {
            return new Message
            {
                Text = messageActionPhoneCall.Duration.ToString(),
                Action = TelegramMessageAction.PhoneCall
            };
        }

        private static Message Parse(TMessageActionPinMessage messageActionPinMessage)
        {
            return new Message
            {
                Text = "Message pinned",
                Action = TelegramMessageAction.PinMessage
            };
        }

        private static Message Parse(TMessageActionScreenshotTaken messageActionScreenshotTaken)
        {
            return new Message
            {
                Text = "Screenshot Taken",
                Action = TelegramMessageAction.ScreenshotTaken
            };
        }

        private static Message Parse(TMessageActionSecureValuesSent messageActionSecureValuesSent)
        {
            return new Message
            {
                Text = string.Join(",", messageActionSecureValuesSent.Types.Select(type => type.GetType().Name)),
                Action = TelegramMessageAction.SecureValuesSent
            };
        }

        private static Message Parse(TMessageActionSecureValuesSentMe messageActionSecureValuesSentMe)
        {
            return new Message
            {
                Text = string.Join(",", messageActionSecureValuesSentMe.Values.Select(type => type.GetType().Name)),
                Action = TelegramMessageAction.SecureValuesSent
            };
        }
    }
}