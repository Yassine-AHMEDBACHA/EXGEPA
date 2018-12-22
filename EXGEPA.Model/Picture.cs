using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
