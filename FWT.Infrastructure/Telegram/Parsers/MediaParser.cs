using FWT.Core.Extensions;
using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using System;
using System.Collections.Generic;

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
            document.Attributes.
        }

        private static MessageMedia Parse(TMessageMediaUnsupported messageMediaUnsupported)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaContact messageMediaContact)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaVenue messageMediaVenue)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaInvoice messageMediaInvoice)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaGeo messageMediaGeo)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaEmpty messageMediaEmpty)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaPhoto messageMediaPhoto)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaGeoLive messageMediaGeoLive)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaWebPage messageMediaWebPage)
        {
            throw new NotImplementedException();
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