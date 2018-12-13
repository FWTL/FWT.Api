using System;
using FWTL.Core.Services.Unique;

namespace FWTL.Infrastructure.Services.Unique
{
    public class RandomService : IRandomService
    {
        public RandomService()
        {
        }

        public Random Random { get; } = new Random();
    }
}
