// <copyright file="Bus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools.Communication
{
    using System;

    internal class Bus : IBus
    {
        public void RaiseEvent(object sender, object args)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(object sender, object args)
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe(object sender, object args)
        {
            throw new NotImplementedException();
        }
    }
}
