using FWT.Core.Extensions;
using FWT.Core.Helpers;
using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using static FWT.Core.Helpers.Enum;

namespace FWT.Infrastructure.Telegram.Parsers
{
    public class MediaParser
    {
        private static readonly Dictionary<string, Func<IMessageMedia, MessageMedia>> Switch = new Dictionary<string, Func<IMessageMedia, MessageMedia>>()
        {
            { typeof(TMessageMediaWebPage).FullName, x => { return Parse(x as TMessageMediaWebPage); } },
            { typeof(TMessageMediaGeoLive).FullName, x => { return Parse(x as TMessageMediaGeoLive); } },
            { typeof(TMessageMediaPhoto).FullName, x => { return Parse(x as TMessageMediaPhoto); } },
            { typeof(TMessageMediaEmpty).FullName, x => { return Parse(x as TMessageMediaEmpty); } },
            { typeof(TMessageMediaGeo).FullName, x => { return Parse(x as TMessageMediaGeo); } },
            { typeof(TMessageMediaInvoice).FullName, x => { return Parse(x as TMessageMediaInvoice); } },
            { typeof(TMessageMediaVenue).FullName, x => { return Parse(x as TMessageMediaVenue); } },
            { typeof(TMessageMediaContact).FullName, x => { return Parse(x as TMessageMediaContact); } },
            { typeof(TMessageMediaUnsupported).FullName, x => { return Parse(x as TMessageMediaUnsupported); } },
            { typeof(TMessageMediaDocument).FullName, x => { return Parse(x as TMessageMediaDocument); } },
            { typeof(TMessageMediaGame).FullName, x => { return Parse(x as TMessageMediaGame); } },
        };

        private static MessageMedia Parse(TMessageMediaGame messageMediaGame)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaDocument messageMediaDocument)
        {
            var document = messageMediaDocument.Document.As<TDocument>();

            var messageMedia = new MessageMedia()
            {
                Type = TelegramMediaType.Document
            };

            List<DocumentAttribute> attrubutes = new List<DocumentAttribute>();
            var attributes = document.Attributes.ForEach(attribute => { return DocumentAttributeParser.Parse(attribute); }).SelectMany(list => list).ToList();

            return messageMedia;
        }

        private static MessageMedia Parse(TMessageMediaUnsupported messageMediaUnsupported)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Unknown
            };
        }

        private static MessageMedia Parse(TMessageMediaContact messageMediaContact)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.MediaContact
            };
        }

        private static MessageMedia Parse(TMessageMediaVenue messageMediaVenue)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Venue
            };
        }

        private static MessageMedia Parse(TMessageMediaInvoice messageMediaInvoice)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Invoice
            };
        }

        private static MessageMedia Parse(TMessageMediaGeo messageMediaGeo)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Geo
            };
        }

        private static MessageMedia Parse(TMessageMediaEmpty messageMediaEmpty)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Unknown
            };
        }

        private static MessageMedia Parse(TMessageMediaPhoto messageMediaPhoto)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Photo
            };
        }

        private static MessageMedia Parse(TMessageMediaGeoLive messageMediaGeoLive)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.GeoLive
            };
        }

        private static MessageMedia Parse(TMessageMediaWebPage messageMediaWebPage)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.WebPage
            };
        }

        public static MessageMedia Parse(IMessageMedia media)
        {
            if (media.IsNull())
            {
                return null;
            }

            string key = media.GetType().FullName;
            return Switch[key](media);
        }
    }
}