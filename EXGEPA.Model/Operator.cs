// <copyright file="Operator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    using CORESI.Data;
    using CORESI.Security;

    public class Operator : KeyRow, IOperator
    {
        [DataAttribute(IsNullable = false)]
        public new string Key { get; set; }

        public string Name { get; set; }

        [DataAttribute(IsNullable = false)]
        public string Password { get; set; }

        public bool ExpiredPassword { get; set; }

        public Person Person { get; set; }

        [DataAttribute(IsNullable = false)]
        public Role Role { get; set; }
    }
}
