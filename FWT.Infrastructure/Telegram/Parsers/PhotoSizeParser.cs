using FWT.Infrastructure.Telegram.Parsers.Models;
using OpenTl.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace FWT.Infrastructure.Telegram.Parsers
{
    public class PhotoSizeParser
    {
        private static readonly Dictionary<string, Func<IPhotoSize, PhotoSize>> Switch = new Dictionary<string, Func<IPhotoSize, PhotoSize>>()
        {
              { typeof(TPhotoSize).FullName, x => { return Parse(x as TPhotoSize); } },
              { typeof(TPhotoSizeEmpty).FullName, x => { return Parse(x as TPhotoSizeEmpty); } },
              { typeof(TPhotoCachedSize).FullName, x => { return Parse(x as TPhotoCachedSize); } },
        };

        private static PhotoSize Parse(TPhotoCachedSize photoCachedSize)
        {
            return new PhotoSize()
            {
                Size = 0,
                Location = photoCachedSize.Location,
            };
        }

        private static PhotoSize Parse(TPhotoSizeEmpty photoSizeEmpty)
        {
            throw new NotImplementedException();
        }

        private static PhotoSize Parse(TPhotoSize photoSize)
        {
            return new PhotoSize()
            {
                Size = photoSize.Size,
                Location = photoSize.Location
            };
        }

        public static PhotoSize Parse(IPhotoSize photoSize)
        {
            string key = photoSize.GetType().FullName;
            return Switch[key](photoSize);
        }
    }
}
