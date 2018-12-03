using FWT.Core.Services.Unique;
using System;

namespace FWT.Infrastructure.Services.Unique
{
    public class RandomService : IRandomService
    {
        public Random Random { get; } = new Random();

        public RandomService()
        {
        }
    }
}