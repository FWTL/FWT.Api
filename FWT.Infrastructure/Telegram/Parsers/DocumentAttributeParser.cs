﻿using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using System;
using System.Collections.Generic;

namespace FWT.Infrastructure.Telegram.Parsers
{
    internal class DocumentAttributeParser
    {
        private static readonly Dictionary<string, Func<IDocumentAttribute, List<DocumentAttribute>>> Switch = new Dictionary<string, Func<IDocumentAttribute, List<DocumentAttribute>>>()
        {
            { typeof(TDocumentAttributeFilename).FullName, x => { return Parse(x as TDocumentAttributeFilename); } },
            { typeof(TDocumentAttributeSticker).FullName, x => { return Parse(x as TDocumentAttributeSticker); } },
            { typeof(TDocumentAttributeAnimated).FullName, x => { return Parse(x as TDocumentAttributeAnimated); } },
            { typeof(TDocumentAttributeHasStickers).FullName, x => { return Parse(x as TDocumentAttributeHasStickers); } },
            { typeof(TDocumentAttributeAudio).FullName, x => { return Parse(x as TDocumentAttributeAudio); } },
            { typeof(TDocumentAttributeVideo).FullName, x => { return Parse(x as TDocumentAttributeVideo); } },
            { typeof(TDocumentAttributeImageSize).FullName, x => { return Parse(x as TDocumentAttributeImageSize); } },
        };

        private static List<DocumentAttribute> Parse(TDocumentAttributeImageSize documentAttributeImageSize)
        {
            return new List<DocumentAttribute>();
        }

        private static List<DocumentAttribute> Parse(TDocumentAttributeVideo documentAttributeVideo)
        {
            var attributes = new List<DocumentAttribute>();
            attributes.Add(new DocumentAttribute(nameof(documentAttributeVideo.Duration), documentAttributeVideo.Duration.ToString()));
            return attributes;
        }

        private static List<DocumentAttribute> Parse(TDocumentAttributeAudio documentAttributeAudio)
        {
            var attributes = new List<DocumentAttribute>();
            attributes.Add(new DocumentAttribute(nameof(documentAttributeAudio.Duration), documentAttributeAudio.Duration.ToString()));
            attributes.Add(new DocumentAttribute(nameof(documentAttributeAudio.Performer), documentAttributeAudio.Performer));
            attributes.Add(new DocumentAttribute(nameof(documentAttributeAudio.Title), documentAttributeAudio.Title));
            attributes.Add(new DocumentAttribute(nameof(documentAttributeAudio.Voice), documentAttributeAudio.Voice.ToString()));
            return attributes;
        }

        private static List<DocumentAttribute> Parse(TDocumentAttributeHasStickers documentAttributeHasStickers)
        {
            return new List<DocumentAttribute>();
        }

        private static List<DocumentAttribute> Parse(TDocumentAttributeAnimated documentAttributeAnimated)
        {
            return new List<DocumentAttribute>();
        }

        private static List<DocumentAttribute> Parse(TDocumentAttributeSticker documentAttributeSticker)
        {
            var attributes = new List<DocumentAttribute>();
            attributes.Add(new DocumentAttribute(nameof(documentAttributeSticker.Alt), documentAttributeSticker.Alt));
            return attributes;
        }

        private static List<DocumentAttribute> Parse(TDocumentAttributeFilename documentAttributeFilename)
        {
            var attributes = new List<DocumentAttribute>();
            attributes.Add(new DocumentAttribute(nameof(documentAttributeFilename.FileName), documentAttributeFilename.FileName));
            return attributes;
        }

        public static List<DocumentAttribute> Parse(IDocumentAttribute attribute)
        {
            string key = attribute.GetType().FullName;
            return Switch[key](attribute);
        }
    }
}