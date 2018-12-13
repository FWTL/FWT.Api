using System.Collections.Generic;
using FWT.Core.Services.Hash;
using HashidsNet;

namespace FWT.Infrastructure.Hash
{
    public class ShortenService : IShortenService
    {
        private readonly Dictionary<string, Hashids> _hashIds = new Dictionary<string, Hashids>();

        public Hashids Hash<TModel>()
        {
            const string modelName = nameof(TModel);
            if (_hashIds.ContainsKey(modelName))
            {
                return _hashIds[modelName];
            }

            var hashId = new Hashids(modelName, 12);
            _hashIds.Add(modelName, hashId);
            return hashId;
        }
    }
}
