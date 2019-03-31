// <copyright file="PropertyTools.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System;

    public class PropertyTools
    {
        public static string GetPropertyName<T>(System.Linq.Expressions.Expression<Func<T, object>> property)
        {
            System.Linq.Expressions.LambdaExpression lambda = (System.Linq.Expressions.LambdaExpression)property;
            System.Linq.Expressions.MemberExpression memberExpression;

            if (lambda.Body is System.Linq.Expressions.UnaryExpression unaryExpression)
            {
                if (unaryExpression.Operand is System.Linq.Expressions.MemberExpression)
                {
                    memberExpression = (System.Linq.Expressions.MemberExpression)unaryExpression.Operand;
                }
                else
                {
                    return unaryExpression.Operand.ToString();
                }
            }
            else
            {
                memberExpression = (System.Linq.Expressions.MemberExpression)lambda.Body;
            }

            return memberExpression.ToString();
        }
    }
}
