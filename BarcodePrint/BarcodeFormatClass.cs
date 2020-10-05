using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ashpro
{
    public class BarcodeFormatClass
    {
        public BarcodeHDR _barcodeHDR { get; set; }
        public List<BarcodeDTL> _barcodeDTLs { get; set; } 
    }
}
