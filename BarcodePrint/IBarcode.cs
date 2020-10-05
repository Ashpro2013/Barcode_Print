using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ashpro
{
    interface IBarcode
    {
        string Encoded_Value
        {
            get;
        }//Encoded_Value

        string RawData
        {
            get;
        }//Raw_Data

        List<string> Errors
        {
            get;
        }//Errors
    }
}
