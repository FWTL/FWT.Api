using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using System;
using System.Collections.Generic;

namespace FWT.Infrastructure.Telegram.Parsers
{
    internal class DocumentAttributeParser
    {
        private static readonly Dictionary<string, Func<IDocumentAttribute, DocumentAttribute>> Switch = new Dictionary<string, Func<IDocumentAttribute, DocumentAttribute>>()
        {
            { typeof(TDocumentAttributeFilename).FullName, x => { return Parse(x as TDocumentAttributeFilename); } },
            { typeof(TDocumentAttributeSticker).FullName, x => { return Parse(x as TDocumentAttributeSticker); } },
            { typeof(TDocumentAttributeAnimated).FullName, x => { return Parse(x as TDocumentAttributeAnimated); } },
            { typeof(TDocumentAttributeHasStickers).FullName, x => { return Parse(x as TDocumentAttributeHasStickers); } },
            { typeof(TDocumentAttributeAudio).FullName, x => { return Parse(x as TDocumentAttributeAudio); } },
            { typeof(TDocumentAttributeVideo).FullName, x => { return Parse(x as TDocumentAttributeVideo); } },
            { typeof(TDocumentAttributeImageSize).FullName, x => { return Parse(x as TDocumentAttributeImageSize); } },
        };

        private static DocumentAttribute Parse(TDocumentAttributeImageSize documentAttributeImageSize)
        {
            throw new NotImplementedException();
        }

        private static DocumentAttribute Parse(TDocumentAttributeVideo documentAttributeVideo)
        {
            throw new NotImplementedException();
        }

        private static DocumentAttribute Parse(TDocumentAttributeAudio documentAttributeAudio)
        {
            throw new NotImplementedException();
        }

        private static DocumentAttribute Parse(TDocumentAttributeHasStickers documentAttributeHasStickers)
        {
            throw new NotImplementedException();
        }

        private static DocumentAttribute Parse(TDocumentAttributeAnimated documentAttributeAnimated)
        {
            throw new NotImplementedException();
        }

        private static DocumentAttribute Parse(TDocumentAttributeSticker documentAttributeSticker)
        {
            throw new NotImplementedException();
        }

        private static DocumentAttribute Parse(TDocumentAttributeFilename documentAttributeFilename)
        {
            throw new NotImplementedException();
        }
    }
}