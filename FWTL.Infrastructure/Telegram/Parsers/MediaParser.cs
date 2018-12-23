using System;
using System.Collections.Generic;
using System.Linq;
using FWTL.Core.Extensions;
using FWTL.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using static FWTL.Core.Helpers.Enum;

namespace FWTL.Infrastructure.Telegram.Parsers
{
    public static class MediaParser
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

        public static MessageMedia Parse(IMessageMedia media)
        {
            if (media.IsNull())
            {
                return null;
            }

            string key = media.GetType().FullName;
            return Switch[key](media);
        }

        private static MessageMedia Parse(TMessageMediaContact messageMediaContact)
        {
            var media = new MessageMedia()
            {
                Type = TelegramMediaType.MediaContact
            };

            media.Attibutes.Add(new DocumentAttribute(nameof(messageMediaContact.FirstName), messageMediaContact.FirstName));
            media.Attibutes.Add(new DocumentAttribute(nameof(messageMediaContact.PhoneNumber), messageMediaContact.PhoneNumber));
            return media;
        }

        private static MessageMedia Parse(TMessageMediaDocument messageMediaDocument)
        {
            var document = messageMediaDocument.Document.As<TDocument>();

            var messageMedia = new MessageMedia()
            {
                Files = new List<File>()
                {
                    new File()
                    {
                        Size = document.Size,
                        Location = new TInputDocumentFileLocation()
                        {
                            Id = document.Id,
                            AccessHash = document.AccessHash,
                        }
                    }
                },
                Type = TelegramMediaType.Document
            };

            messageMedia.Attibutes = document.Attributes.ForEach(attribute => { return DocumentAttributeParser.Parse(attribute); }).SelectMany(list => list).ToList();
            return messageMedia;
        }

        private static MessageMedia Parse(TMessageMediaEmpty messageMediaEmpty)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Unknown
            };
        }

        private static MessageMedia Parse(TMessageMediaGame messageMediaGame)
        {
            throw new NotImplementedException();
        }

        private static MessageMedia Parse(TMessageMediaGeo messageMediaGeo)
        {
            var point = messageMediaGeo.Geo.As<TGeoPoint>();
            var media = new MessageMedia()
            {
                Type = TelegramMediaType.Geo
            };

            media.Attibutes.Add(new DocumentAttribute("Lat", point.Lat.ToString()));
            media.Attibutes.Add(new DocumentAttribute("Long", point.Long.ToString()));
            return media;
        }

        private static MessageMedia Parse(TMessageMediaGeoLive messageMediaGeoLive)
        {
            var point = messageMediaGeoLive.Geo.As<TGeoPoint>();
            var media = new MessageMedia()
            {
                Type = TelegramMediaType.GeoLive
            };

            media.Attibutes.Add(new DocumentAttribute("Lat", point.Lat.ToString()));
            media.Attibutes.Add(new DocumentAttribute("Long", point.Long.ToString()));
            return media;
        }

        private static MessageMedia Parse(TMessageMediaInvoice messageMediaInvoice)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Invoice
            };
        }

        private static MessageMedia Parse(TMessageMediaPhoto messageMediaPhoto)
        {
            var photo = messageMediaPhoto.Photo.As<TPhoto>();
            var sizes = photo.Sizes.ForEach(size => { return PhotoSizeParser.Parse(size); });
            var files = sizes.ForEach(size =>
            {
                var location = size.Location.As<TFileLocation>();
                return new File()
                {
                    Size = size.Size,
                    Location = new TInputFileLocation()
                    {
                        LocalId = location.LocalId,
                        VolumeId = location.VolumeId,
                        Secret = location.Secret
                    }
                };
            });

            return new MessageMedia()
            {
                Files = files.ToList(),
                Type = TelegramMediaType.Photo
            };
        }

        private static MessageMedia Parse(TMessageMediaUnsupported messageMediaUnsupported)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Unknown
            };
        }

        private static MessageMedia Parse(TMessageMediaVenue messageMediaVenue)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.Venue
            };
        }

        private static MessageMedia Parse(TMessageMediaWebPage messageMediaWebPage)
        {
            return new MessageMedia()
            {
                Type = TelegramMediaType.WebPage
            };
        }
    }
}