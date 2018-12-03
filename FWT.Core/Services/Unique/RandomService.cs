using System;

namespace FWT.Core.Services.Unique
{
    public class RandomService : IRandomService
    {
        public Random Random { get; } = new Random();

        public RandomService()
        {

        }
    }
}