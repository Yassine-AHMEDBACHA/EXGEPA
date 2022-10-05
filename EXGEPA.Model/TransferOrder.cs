// <copyright file="TransferOrder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    public class TransferOrder : Certificate
    {
        public AnalyticalAccount Sender { get; set; }
    }
}
