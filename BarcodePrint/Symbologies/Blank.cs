using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ashpro.Symbologies
{
    class Blank : BarcodeCommon, IBarcode
    {

        #region IBarcode Members

        public string Encoded_Value
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
