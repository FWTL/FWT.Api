using System;
using FWT.Core.Services.Unique;

namespace FWT.Infrastructure.Unique
{
    public class GuidService : IGuidService
    {
        public Guid New()
        {
            return Guid.NewGuid();
        }
    }
}
