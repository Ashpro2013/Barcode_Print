using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace Ashpro
{
    public class BarcodeDTL
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool isIncluded { get; set; }
        public int Position_x { get; set; }
        public int Position_y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Alignment { get; set; }
        public string RotationPoint { get; set; }
        public string fontFamily { get; set; }
        public int fontSize { get; set; }
        public string fStyle { get; set; }
        public string Format { get; set; }
        public string bAlignment { get; set; }
    }
}
