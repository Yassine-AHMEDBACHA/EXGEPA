using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Tools
{
   public  class PropertyTools
    {
        public static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T, object>> property)
        {
            var lambda = (System.Linq.Expressions.LambdaExpression)property;
            System.Linq.Expressions.MemberExpression memberExpression;

            if (lambda.Body is System.Linq.Expressions.UnaryExpression)
            {
                System.Linq.Expressions.UnaryExpression unaryExpression = (System.Linq.Expressions.UnaryExpression)(lambda.Body);
                if (unaryExpression.Operand is System.Linq.Expressions.MemberExpression)
                {
                    memberExpression = (System.Linq.Expressions.MemberExpression)(unaryExpression.Operand);
                }
                else
                {
                    return unaryExpression.Operand.ToString();
                }
            }
            else
            {
                memberExpression = (System.Linq.Expressions.MemberExpression)(lambda.Body);
            }

            return memberExpression.ToString() ;
        }
    }
}
