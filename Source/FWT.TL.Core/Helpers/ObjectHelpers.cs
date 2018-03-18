using FWT.TL.Core.Extensions;
using System;
using System.Linq.Expressions;

namespace FWT.TL.Core.Helpers
{
    public static class ObjectHelpers
    {
        public static string GetName(Expression<Func<object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body.IsNull())
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }
    }
}
