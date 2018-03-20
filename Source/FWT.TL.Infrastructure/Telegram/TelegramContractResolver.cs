﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FWT.TL.Infrastructure.Telegram
{
    public class TelegramContractResolver : DefaultContractResolver
    {
        public TelegramContractResolver()
        {
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
            properties = properties.Where(p => !p.PropertyName.EndsWith("AsBinary")).Where(p=>p.PropertyName != "Flags").ToList();
            return properties;
        }
    }
}