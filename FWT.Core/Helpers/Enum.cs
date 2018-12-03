namespace FWT.Core.Helpers
{
    public class Enum
    {
        public enum SortDirection
        {
            DESC = 0,
            ASC = 1,
        }

        public enum PageSize
        {
            p10 = 10,
            p25 = 25,
            p50 = 50,
            p100 = 100
        }

        public enum TelegramEntity
        {
            Unknown = 0,
            Mention = 1,
            Hashtag = 2,
            BotCommand = 3,
            Url = 4,
            Email = 5,
            Bold = 6,
            Italic = 7,
            Code = 8,
            Pre = 9,
            TextUrl = 10,
            MentionName = 11,
            InputMentionName = 12,
        }

        public enum TelegramMessageAction
        {
            Empty = 0,
            ChatCreate = 1,
            ChatEditTitle = 2,
            ChatEditPhoto = 3,
            ChatDeletePhoto = 4,
            ChatAddUser = 5,
            ChatDeleteUser = 6,
            ChatJoinedByLink = 7,
            ChannelCreate = 8,
            ChatMigrateTo = 9,
            MigrateFrom = 10,
            PinMessage = 11,
            HistoryClear = 12,
            GameScore = 13,
            PaymentSentMe = 14,
            PaymentSent = 15,
            PhoneCall = 16,
            ScreenshotTaken = 17,
            ActionCustomAction = 18,
        }
    }
}