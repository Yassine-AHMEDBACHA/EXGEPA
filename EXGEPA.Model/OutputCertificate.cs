// <copyright file="OutputCertificate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    public class OutputCertificate : Certificate
    {
        public OutputType OutputType { get; set; }
    }

    public enum OutputType
    {
        Cession = 1,
        Destruction,
        Disparition,
        Vente
    }
}
