using System;
using System.Linq.Expressions;

namespace FWT.TL.Core.Extensions
{
    public class OrderableColumn<TSource, TKey>
    {
        public Expression<Func<TSource, TKey>> Fn { get; set; }

        public int Id { get; set; }
    }
}