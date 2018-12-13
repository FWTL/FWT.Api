using System;
using FWT.Core.Services.Unique;

namespace FWT.Infrastructure.Services.Unique
{
    public class RandomService : IRandomService
    {
        public RandomService()
        {
        }

        public Random Random { get; } = new Random();
    }
}
