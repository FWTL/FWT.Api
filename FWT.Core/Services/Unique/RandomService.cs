using System;

namespace FWT.Core.Services.Unique
{
    public class RandomService : IRandomService
    {
        public RandomService()
        {
        }

        public Random Random { get; } = new Random();
    }
}
