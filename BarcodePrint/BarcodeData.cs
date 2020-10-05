using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Ashpro
{
    public class BarcodeData
    {
        public int Sl_No { get; set; }//0
        public string Company_Name { get; set; }//1
        public string Product_Code { get; set; }//2
        public string Product_Name { get; set; } //3 
        public string Product_Description { get; set; }//4
        public string Group_Name { get; set; }//5
        public string Batch { get; set; }//6
        public string Unit { get; set; }//7
        public string MultiRate_Name { get; set; }//8
        public decimal Purchase_Rate { get; set; }//9
        public decimal Sales_Rate { get; set; }//10
        public decimal Inclusive_Rate { get; set; }//11
        public decimal Unit_Price { get; set; }//12
        public decimal MRP { get; set; }//13
        public string UserField_1 { get; set; }//14
        public string UserField_2 { get; set; }//15
        public string UserField_3 { get; set; }//16
        public string UserField_4 { get; set; }//17
        public string Price_Code { get; set; }//18
        public string Barcode { get; set; }//19
        public string Batch_Barecode { get; set; }//20
        public string Unit_Barcode { get; set; }//21
        public string Ledger_Code { get; set; }//22
        public string Mfg_Date { get; set; }//23
        public string Exparing_Date { get; set; }//24
        public string item_Note { get; set; }//25
        public string User_Type_1 { get; set; }//26
        public string User_Type_2 { get; set; }//27
        public string User_Type_3 { get; set; }//28
        public string User_Type_4 { get; set; }//29
        public string Custome_String_1 { get; set; }//30
        public string Custome_String_2 { get; set; }//31
        public string Custome_String_3 { get; set; }//32
        public string Custome_String_4 { get; set; }//33
        public string Logo { get; set; }//34
        public string Barcode_Image { get; set; }//35
        public decimal Quantity { get; set; }//36
    }
}
