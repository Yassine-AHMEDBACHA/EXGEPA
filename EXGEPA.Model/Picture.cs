// <copyright file="Picture.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Model
{
    public class Picture
    {
        public int Id { get; set; }

        public int ObjectId { get; set; }

        public PictureType PictureType { get; set; }

        public string Path { get; set; }
    }

    public enum PictureType
    {
        Item,
        Reference
    }
}
