using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;
namespace Ashpro
{
    public class ProductBarcode
    {
        #region enum
        enum bCodeData
        {
            Sl_No,
            Company_Name,
            Product_Code,
            Product_Name,
            Product_Description,
            Group_Name,
            Batch,
            Unit,
            MultiRate_Name,
            Purchase_Rate,
            Sales_Rate,
            Inclusive_Rate,
            Unit_Price,
            MRP,
            UserField_1,
            UserField_2,
            UserField_3,
            UserField_4,
            Price_Code,
            Barcode,
            Batch_Barecode,
            Unit_Barcode,
            Ledger_Code,
            Mfg_Date,
            Exparing_Date,
            item_Note,
            User_Type_1,
            User_Type_2,
            User_Type_3,
            User_Type_4,
            Custome_String_1,
            Custome_String_2,
            Custome_String_3,
            Custome_String_4,
            Logo,
            Barcode_Image
        }
        #endregion

        #region Private Variable
        string NumberFormat; 
        PrintDocument pDocu = new PrintDocument();
        List<BarcodeData> barcodeList = new List<BarcodeData>();
        StringFormat fNear = new StringFormat();
        StringFormat fCenter = new StringFormat();
        StringFormat fFar = new StringFormat();
        StringFormat fVertical;
        StringFormat fFormat = new StringFormat();
        int  intPrintedLabelCount, intRowIndex, intNoOFClm, intItemPrinted, xPoint, yPoint, intAcsNo;
        int intTotal;
        bool isFirst = false;
        string strBarCode;
        Image img;
        BarcodeHDR bHdr = new BarcodeHDR();
        List<BarcodeDTL> _dtls = new List<BarcodeDTL>();
        PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
        int _intStartingPoint;
        int intPrintableWth, intColmWdth, intColmHt;
        FontStyle fStyle;
        #endregion

        #region constructor
        public ProductBarcode()
        {
            NumberFormat = "N" + 2;
            pDocu.PrintPage += new PrintPageEventHandler(pDocu_PrintPage);
        }

       
        #endregion

        #region Functions
        public void printBarcode(List<BarcodeData> proBarcode, List<BarcodeDTL> dtls, BarcodeHDR _bHdr, int intStartingPoint)
        {
            if (intStartingPoint> 0)
            {
                _intStartingPoint = intStartingPoint - 1;
            }
            else
            {
                _intStartingPoint = 0;
            }
            barcodeList = proBarcode;
            bHdr = _bHdr;
            _dtls = dtls;
            intTotal = 0;
            foreach (var items in barcodeList)
            {
                intTotal = intTotal + Convert.ToInt32(Math.Round(items.Quantity,0));
            }
            intPrintedLabelCount = 0;
            isFirst = true;
            intRowIndex = 0;
            intNoOFClm = 0;
            intItemPrinted = 0;
            pDocu.PrinterSettings.PrinterName = bHdr.PrinterName;
            pDocu.DefaultPageSettings.PaperSize = new PaperSize(bHdr.PaperName, bHdr.PSWidth, bHdr.PSHieght);
            pDocu.Print();
        }
        public void printBarcode(List<BarcodeData> proBarcode, string strFileName, int intStartingPoint)
        {
            _intStartingPoint = intStartingPoint;
            barcodeList = proBarcode;
            string sPatch = strFileName;
            if (!File.Exists(sPatch))
            {
                return;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(BarcodeFormatClass));
            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
            // Declare an object variable of the type to be deserialized.
            BarcodeFormatClass _objFormat;
            // A FileStream is needed to read the XML document.
            using (FileStream fs = new FileStream(sPatch, FileMode.Open))
            {
                /* Use the Deserialize method to restore the object's state with
                data from the XML document. */
                _objFormat = (BarcodeFormatClass)serializer.Deserialize(fs);
                fs.Close();
            }
            bHdr = _objFormat._barcodeHDR;
            _dtls = _objFormat._barcodeDTLs;
            printBarcode(proBarcode, _dtls, bHdr, intStartingPoint);
        }
        private void getStringFormat(string sfName, string strFStylName)
        {
            fNear.Alignment = StringAlignment.Near;
            fCenter.Alignment = StringAlignment.Center;
            fFar.Alignment = StringAlignment.Near;
            fFar.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            fVertical = new StringFormat(StringFormatFlags.DirectionVertical);
            fFormat = null;
            if (sfName == "Left")
            {
                fFormat = fNear;
            }
            else if (sfName == "Center")
            {
                fFormat = fCenter;
            }
            else if (sfName == "Right")
            {
                fFormat = fFar;
            }
            else
            {
                fFormat = fVertical;
            }
            if (strFStylName == "Regular")
            {
                fStyle = FontStyle.Regular;
            }
            else if (strFStylName == "Bold")
            {
                fStyle = FontStyle.Bold;
            }
            else
            {
                fStyle = FontStyle.Italic;
            }
        }
        public void printBarcodePreview(List<BarcodeData> proBarcode, List<BarcodeDTL> dtls, BarcodeHDR _bHdr, int intStartingPoint)
        {
            _intStartingPoint = intStartingPoint;
            barcodeList = proBarcode;
            bHdr = _bHdr;
            _dtls = dtls;
            intTotal = 0;
            foreach (var items in barcodeList)
            {
                intTotal = intTotal + Convert.ToInt32(Math.Round(items.Quantity,0));
            }
            intPrintedLabelCount = 0;
            isFirst = true;
            intRowIndex = 0;
            intNoOFClm = 0;
            intItemPrinted = 0;
            pDocu.PrinterSettings.PrinterName = bHdr.PrinterName;
            pDocu.DefaultPageSettings.PaperSize = new PaperSize(bHdr.PaperName, bHdr.PSWidth, bHdr.PSHieght);
            printPreviewDialog1.Document = pDocu;
            printPreviewDialog1.ShowDialog();

           
        }
        public void printBarcodePreview(List<BarcodeData> proBarcode, string strFileName, int intStartingPoint)
        {
            _intStartingPoint = intStartingPoint;
            barcodeList = proBarcode;
            string sPatch = strFileName;
            if (!File.Exists(sPatch))
            {
                return;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(BarcodeFormatClass));
            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
            // Declare an object variable of the type to be deserialized.
            BarcodeFormatClass _objFormat;
            // A FileStream is needed to read the XML document.
            using (FileStream fs = new FileStream(sPatch, FileMode.Open))
            {
                /* Use the Deserialize method to restore the object's state with
                data from the XML document. */
                _objFormat = (BarcodeFormatClass)serializer.Deserialize(fs);
                fs.Close();
            }
            _dtls = _objFormat._barcodeDTLs;
            bHdr = _objFormat._barcodeHDR;
            intTotal = 0;
            foreach (var items in barcodeList)
            {
                intTotal = intTotal + Convert.ToInt32(Math.Round(items.Quantity,0));
            }
            intPrintedLabelCount = 0;
            isFirst = true;
            intRowIndex = 0;
            intNoOFClm = 0;
            intItemPrinted = 0;
            pDocu.PrinterSettings.PrinterName = bHdr.PrinterName;
            pDocu.DefaultPageSettings.PaperSize = new PaperSize(bHdr.PaperName, bHdr.PSWidth, bHdr.PSHieght);
            printPreviewDialog1.Document = pDocu;
            printPreviewDialog1.ShowDialog();
        }

        public void getBarcodeImage(List<BarcodeData> proBarcode, List<BarcodeDTL> _dtls, int index, int wt, int ht, Color forcolor, Color backcolor, int bField)
        {
            Bitmap bitmap = new Bitmap(wt, ht);
            Image bCodeImage = null;
            try
            {
                if (bField == 0 && proBarcode[index].Product_Code != null)
                {
                    bCodeImage = BarcodeImageEncodeMethod(_dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height, _dtls[(int)bCodeData.Barcode_Image].Format, _dtls[(int)bCodeData.Barcode_Image].RotationPoint, proBarcode[index].Product_Code);
                }
                else if (bField == 1 && proBarcode[index].Barcode != null)
                {
                    bCodeImage = BarcodeImageEncodeMethod(_dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height, _dtls[(int)bCodeData.Barcode_Image].Format, _dtls[(int)bCodeData.Barcode_Image].RotationPoint, proBarcode[index].Barcode);
                }
                else if (bField == 2 && proBarcode[index].Batch_Barecode != null)
                {
                    bCodeImage = BarcodeImageEncodeMethod(_dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height, _dtls[(int)bCodeData.Barcode_Image].Format, _dtls[(int)bCodeData.Barcode_Image].RotationPoint, proBarcode[index].Batch_Barecode);
                }
                else if (bField == 3 && proBarcode[index].Unit_Barcode != null)
                {
                    bCodeImage = BarcodeImageEncodeMethod(_dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height, _dtls[(int)bCodeData.Barcode_Image].Format, _dtls[(int)bCodeData.Barcode_Image].RotationPoint, proBarcode[index].Unit_Barcode);
                }
                else
                {
                    bCodeImage = null;
                }
                if (bCodeImage != null)
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.Clear(backcolor);
                        g.DrawImage(bCodeImage, _dtls[(int)bCodeData.Barcode_Image].Position_x, _dtls[(int)bCodeData.Barcode_Image].Position_y, _dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height);
                        if (_dtls[(int)bCodeData.Sl_No].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Sl_No].Alignment, _dtls[(int)bCodeData.Sl_No].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Sl_No].Value + proBarcode[index].Sl_No.ToString(), new Font(_dtls[(int)bCodeData.Sl_No].fontFamily, _dtls[(int)bCodeData.Sl_No].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Sl_No].Position_x, _dtls[(int)bCodeData.Sl_No].Position_y, _dtls[(int)bCodeData.Sl_No].Width, _dtls[(int)bCodeData.Sl_No].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Company_Name].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Company_Name].Alignment, _dtls[(int)bCodeData.Company_Name].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Company_Name].Value, new Font(_dtls[(int)bCodeData.Company_Name].fontFamily, _dtls[(int)bCodeData.Company_Name].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Company_Name].Position_x, _dtls[(int)bCodeData.Company_Name].Position_y, _dtls[(int)bCodeData.Company_Name].Width, _dtls[(int)bCodeData.Company_Name].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Product_Code].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Product_Code].Alignment, _dtls[(int)bCodeData.Product_Code].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Product_Code].Value + proBarcode[index].Product_Code, new Font(_dtls[(int)bCodeData.Product_Code].fontFamily, _dtls[(int)bCodeData.Product_Code].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Product_Code].Position_x, _dtls[(int)bCodeData.Product_Code].Position_y, _dtls[(int)bCodeData.Product_Code].Width, _dtls[(int)bCodeData.Product_Code].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Product_Name].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Product_Name].Alignment, _dtls[(int)bCodeData.Product_Name].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Product_Name].Value + proBarcode[index].Product_Name, new Font(_dtls[(int)bCodeData.Product_Name].fontFamily, _dtls[(int)bCodeData.Product_Name].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Product_Name].Position_x, _dtls[(int)bCodeData.Product_Name].Position_y, _dtls[(int)bCodeData.Product_Name].Width, _dtls[(int)bCodeData.Product_Name].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Product_Description].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Product_Description].Alignment, _dtls[(int)bCodeData.Product_Description].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Product_Description].Value.Trim() + proBarcode[index].Product_Description, new Font(_dtls[(int)bCodeData.Product_Description].fontFamily, _dtls[(int)bCodeData.Product_Description].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Product_Description].Position_x, _dtls[(int)bCodeData.Product_Description].Position_y, _dtls[(int)bCodeData.Product_Description].Width, _dtls[(int)bCodeData.Product_Description].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Group_Name].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Group_Name].Alignment, _dtls[(int)bCodeData.Group_Name].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Group_Name].Value + proBarcode[index].Group_Name, new Font(_dtls[(int)bCodeData.Group_Name].fontFamily, _dtls[(int)bCodeData.Group_Name].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Group_Name].Position_x, _dtls[(int)bCodeData.Group_Name].Position_y, _dtls[(int)bCodeData.Group_Name].Width, _dtls[(int)bCodeData.Group_Name].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Batch].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Batch].Alignment, _dtls[(int)bCodeData.Batch].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Batch].Value + proBarcode[index].Batch, new Font(_dtls[(int)bCodeData.Batch].fontFamily, _dtls[(int)bCodeData.Batch].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Batch].Position_x, _dtls[(int)bCodeData.Batch].Position_y, _dtls[(int)bCodeData.Batch].Width, _dtls[(int)bCodeData.Batch].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Unit].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Unit].Alignment, _dtls[(int)bCodeData.Unit].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Unit].Value + proBarcode[index].Unit, new Font(_dtls[(int)bCodeData.Unit].fontFamily, _dtls[(int)bCodeData.Unit].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Unit].Position_x, _dtls[(int)bCodeData.Unit].Position_y, _dtls[(int)bCodeData.Unit].Width, _dtls[(int)bCodeData.Unit].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.MultiRate_Name].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.MultiRate_Name].Alignment, _dtls[(int)bCodeData.MultiRate_Name].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.MultiRate_Name].Value + proBarcode[index].MultiRate_Name, new Font(_dtls[(int)bCodeData.MultiRate_Name].fontFamily, _dtls[(int)bCodeData.MultiRate_Name].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.MultiRate_Name].Position_x, _dtls[(int)bCodeData.MultiRate_Name].Position_y, _dtls[(int)bCodeData.MultiRate_Name].Width, _dtls[(int)bCodeData.MultiRate_Name].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Purchase_Rate].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Purchase_Rate].Alignment, _dtls[(int)bCodeData.Purchase_Rate].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Purchase_Rate].Value + proBarcode[index].Purchase_Rate.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.Purchase_Rate].fontFamily, _dtls[(int)bCodeData.Purchase_Rate].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.Purchase_Rate].Position_x, _dtls[(int)bCodeData.Purchase_Rate].Position_y), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Sales_Rate].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Sales_Rate].Alignment, _dtls[(int)bCodeData.Sales_Rate].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Sales_Rate].Value + proBarcode[index].Sales_Rate.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.Sales_Rate].fontFamily, _dtls[(int)bCodeData.Sales_Rate].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.Sales_Rate].Position_x, _dtls[(int)bCodeData.Sales_Rate].Position_y), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Inclusive_Rate].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Inclusive_Rate].Alignment, _dtls[(int)bCodeData.Inclusive_Rate].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Inclusive_Rate].Value + proBarcode[index].Inclusive_Rate.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.Inclusive_Rate].fontFamily, _dtls[(int)bCodeData.Inclusive_Rate].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.Inclusive_Rate].Position_x, _dtls[(int)bCodeData.Inclusive_Rate].Position_y), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Unit_Price].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Unit_Price].Alignment, _dtls[(int)bCodeData.Unit_Price].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Unit_Price].Value + proBarcode[index].Unit_Price.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.Unit_Price].fontFamily, _dtls[(int)bCodeData.Unit_Price].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.Unit_Price].Position_x, _dtls[(int)bCodeData.Unit_Price].Position_y), fFormat);
                        }
                        if (_dtls[(int)bCodeData.MRP].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.MRP].Alignment, _dtls[(int)bCodeData.MRP].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.MRP].Value + proBarcode[index].MRP.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.MRP].fontFamily, _dtls[(int)bCodeData.MRP].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.MRP].Position_x, _dtls[(int)bCodeData.MRP].Position_y), fFormat);
                        }
                        if (_dtls[(int)bCodeData.UserField_1].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.UserField_1].Alignment, _dtls[(int)bCodeData.UserField_1].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.UserField_1].Value + proBarcode[index].UserField_1, new Font(_dtls[(int)bCodeData.UserField_1].fontFamily, _dtls[(int)bCodeData.UserField_1].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.UserField_1].Position_x, _dtls[(int)bCodeData.UserField_1].Position_y, _dtls[(int)bCodeData.UserField_1].Width, _dtls[(int)bCodeData.UserField_1].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.UserField_2].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.UserField_2].Alignment, _dtls[(int)bCodeData.UserField_2].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.UserField_2].Value + proBarcode[index].UserField_2, new Font(_dtls[(int)bCodeData.UserField_2].fontFamily, _dtls[(int)bCodeData.UserField_2].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.UserField_2].Position_x, _dtls[(int)bCodeData.UserField_2].Position_y, _dtls[(int)bCodeData.UserField_2].Width, _dtls[(int)bCodeData.UserField_2].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.UserField_3].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.UserField_3].Alignment, _dtls[(int)bCodeData.UserField_3].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.UserField_3].Value + proBarcode[index].UserField_3, new Font(_dtls[(int)bCodeData.UserField_3].fontFamily, _dtls[(int)bCodeData.UserField_3].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.UserField_3].Position_x, _dtls[(int)bCodeData.UserField_3].Position_y, _dtls[(int)bCodeData.UserField_3].Width, _dtls[(int)bCodeData.UserField_3].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.UserField_4].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.UserField_4].Alignment, _dtls[(int)bCodeData.UserField_4].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.UserField_4].Value + proBarcode[index].UserField_4, new Font(_dtls[(int)bCodeData.UserField_4].fontFamily, _dtls[(int)bCodeData.UserField_4].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.UserField_4].Position_x, _dtls[(int)bCodeData.UserField_4].Position_y, _dtls[(int)bCodeData.UserField_4].Width, _dtls[(int)bCodeData.UserField_4].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Price_Code].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Price_Code].Alignment, _dtls[(int)bCodeData.Price_Code].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Price_Code].Value + proBarcode[index].Price_Code, new Font(_dtls[(int)bCodeData.Price_Code].fontFamily, _dtls[(int)bCodeData.Price_Code].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Price_Code].Position_x, _dtls[(int)bCodeData.Price_Code].Position_y, _dtls[(int)bCodeData.Price_Code].Width, _dtls[(int)bCodeData.Price_Code].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Barcode].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Barcode].Alignment, _dtls[(int)bCodeData.Barcode].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Barcode].Value + proBarcode[index].Barcode, new Font(_dtls[(int)bCodeData.Barcode].fontFamily, _dtls[(int)bCodeData.Barcode].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Barcode].Position_x, _dtls[(int)bCodeData.Barcode].Position_y, _dtls[(int)bCodeData.Barcode].Width, _dtls[(int)bCodeData.Barcode].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Batch_Barecode].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Batch_Barecode].Alignment, _dtls[(int)bCodeData.Batch_Barecode].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Batch_Barecode].Value + proBarcode[index].Batch_Barecode, new Font(_dtls[(int)bCodeData.Batch_Barecode].fontFamily, _dtls[(int)bCodeData.Batch_Barecode].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Batch_Barecode].Position_x, _dtls[(int)bCodeData.Batch_Barecode].Position_y, _dtls[(int)bCodeData.Batch_Barecode].Width, _dtls[(int)bCodeData.Batch_Barecode].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Unit_Barcode].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Unit_Barcode].Alignment, _dtls[(int)bCodeData.Unit_Barcode].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Unit_Barcode].Value + proBarcode[index].Unit_Barcode, new Font(_dtls[(int)bCodeData.Unit_Barcode].fontFamily, _dtls[(int)bCodeData.Unit_Barcode].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Unit_Barcode].Position_x, _dtls[(int)bCodeData.Unit_Barcode].Position_y, _dtls[(int)bCodeData.Unit_Barcode].Width, _dtls[(int)bCodeData.Unit_Barcode].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Ledger_Code].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Ledger_Code].Alignment, _dtls[(int)bCodeData.Ledger_Code].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Ledger_Code].Value + proBarcode[index].Ledger_Code, new Font(_dtls[(int)bCodeData.Ledger_Code].fontFamily, _dtls[(int)bCodeData.Ledger_Code].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Ledger_Code].Position_x, _dtls[(int)bCodeData.Ledger_Code].Position_y, _dtls[(int)bCodeData.Ledger_Code].Width, _dtls[(int)bCodeData.Ledger_Code].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Mfg_Date].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Mfg_Date].Alignment, _dtls[(int)bCodeData.Mfg_Date].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Mfg_Date].Value + proBarcode[index].Mfg_Date, new Font(_dtls[(int)bCodeData.Mfg_Date].fontFamily, _dtls[(int)bCodeData.Mfg_Date].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Mfg_Date].Position_x, _dtls[(int)bCodeData.Mfg_Date].Position_y, _dtls[(int)bCodeData.Mfg_Date].Width, _dtls[(int)bCodeData.Mfg_Date].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Exparing_Date].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Exparing_Date].Alignment, _dtls[(int)bCodeData.Exparing_Date].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Exparing_Date].Value + proBarcode[index].Exparing_Date, new Font(_dtls[(int)bCodeData.Exparing_Date].fontFamily, _dtls[(int)bCodeData.Exparing_Date].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Exparing_Date].Position_x, _dtls[(int)bCodeData.Exparing_Date].Position_y, _dtls[(int)bCodeData.Exparing_Date].Width, _dtls[(int)bCodeData.Exparing_Date].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.item_Note].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.item_Note].Alignment, _dtls[(int)bCodeData.item_Note].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.item_Note].Value + proBarcode[index].item_Note, new Font(_dtls[(int)bCodeData.item_Note].fontFamily, _dtls[(int)bCodeData.item_Note].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.item_Note].Position_x, _dtls[(int)bCodeData.item_Note].Position_y, _dtls[(int)bCodeData.item_Note].Width, _dtls[(int)bCodeData.item_Note].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.User_Type_1].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.User_Type_1].Alignment, _dtls[(int)bCodeData.User_Type_1].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.User_Type_1].Value + proBarcode[index].User_Type_1, new Font(_dtls[(int)bCodeData.User_Type_1].fontFamily, _dtls[(int)bCodeData.User_Type_1].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.User_Type_1].Position_x, _dtls[(int)bCodeData.User_Type_1].Position_y, _dtls[(int)bCodeData.User_Type_1].Width, _dtls[(int)bCodeData.User_Type_1].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.User_Type_2].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.User_Type_2].Alignment, _dtls[(int)bCodeData.User_Type_2].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.User_Type_2].Value + proBarcode[index].User_Type_2, new Font(_dtls[(int)bCodeData.User_Type_2].fontFamily, _dtls[(int)bCodeData.User_Type_2].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.User_Type_2].Position_x, _dtls[(int)bCodeData.User_Type_2].Position_y, _dtls[(int)bCodeData.User_Type_2].Width, _dtls[(int)bCodeData.User_Type_2].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.User_Type_3].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.User_Type_3].Alignment, _dtls[(int)bCodeData.User_Type_3].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.User_Type_3].Value + proBarcode[index].User_Type_3, new Font(_dtls[(int)bCodeData.User_Type_3].fontFamily, _dtls[(int)bCodeData.User_Type_3].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.User_Type_3].Position_x, _dtls[(int)bCodeData.User_Type_3].Position_y, _dtls[(int)bCodeData.User_Type_3].Width, _dtls[(int)bCodeData.User_Type_3].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.User_Type_4].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.User_Type_4].Alignment, _dtls[(int)bCodeData.User_Type_4].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.User_Type_4].Value + proBarcode[index].User_Type_4, new Font(_dtls[(int)bCodeData.User_Type_4].fontFamily, _dtls[(int)bCodeData.User_Type_4].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.User_Type_4].Position_x, _dtls[(int)bCodeData.User_Type_4].Position_y, _dtls[(int)bCodeData.User_Type_4].Width, _dtls[(int)bCodeData.User_Type_4].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Custome_String_1].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Custome_String_1].Alignment, _dtls[(int)bCodeData.Custome_String_1].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Custome_String_1].Value, new Font(_dtls[(int)bCodeData.Custome_String_1].fontFamily, _dtls[(int)bCodeData.Custome_String_1].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Custome_String_1].Position_x, _dtls[(int)bCodeData.Custome_String_1].Position_y, _dtls[(int)bCodeData.Custome_String_1].Width, _dtls[(int)bCodeData.Custome_String_1].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Custome_String_2].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Custome_String_2].Alignment, _dtls[(int)bCodeData.Custome_String_2].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Custome_String_2].Value, new Font(_dtls[(int)bCodeData.Custome_String_2].fontFamily, _dtls[(int)bCodeData.Custome_String_2].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Custome_String_2].Position_x, _dtls[(int)bCodeData.Custome_String_2].Position_y, _dtls[(int)bCodeData.Custome_String_2].Width, _dtls[(int)bCodeData.Custome_String_2].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Custome_String_3].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Custome_String_3].Alignment, _dtls[(int)bCodeData.Custome_String_3].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Custome_String_3].Value, new Font(_dtls[(int)bCodeData.Custome_String_3].fontFamily, _dtls[(int)bCodeData.Custome_String_3].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Custome_String_3].Position_x, _dtls[(int)bCodeData.Custome_String_3].Position_y, _dtls[(int)bCodeData.Custome_String_3].Width, _dtls[(int)bCodeData.Custome_String_3].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Custome_String_4].isIncluded)
                        {
                            getStringFormat(_dtls[(int)bCodeData.Custome_String_4].Alignment, _dtls[(int)bCodeData.Custome_String_4].fStyle);
                            g.DrawString(_dtls[(int)bCodeData.Custome_String_4].Value, new Font(_dtls[(int)bCodeData.Custome_String_4].fontFamily, _dtls[(int)bCodeData.Custome_String_4].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Custome_String_4].Position_x, _dtls[(int)bCodeData.Custome_String_4].Position_y, _dtls[(int)bCodeData.Custome_String_4].Width, _dtls[(int)bCodeData.Custome_String_4].Height), fFormat);
                        }
                        if (_dtls[(int)bCodeData.Logo].isIncluded)
                        {
                            Image imgLogo = Image.FromFile(_dtls[(int)bCodeData.Logo].Value);
                            g.DrawImage(imgLogo, _dtls[(int)bCodeData.Logo].Position_x, _dtls[(int)bCodeData.Logo].Position_y, _dtls[(int)bCodeData.Logo].Width, _dtls[(int)bCodeData.Logo].Height);
                        }
                    }
                    img= (Image)bitmap;
                }
                else
                {
                    img=null; 
                }
            }
            catch (Exception)
            {
                //throw;
            }
            
        }
        public Image getBarcodeImageTwo(List<BarcodeData> proBarcode, List<BarcodeDTL> _dtls, int index, int wt, int ht, Color forcolor, Color backcolor, int bField)
        {
            Bitmap bitmap = new Bitmap(wt, ht);
            Image bCodeImage = null;
            try
            {
                if (bField == 0 )
                {
                    bCodeImage = BarcodeImageEncodeMethod(_dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height, _dtls[(int)bCodeData.Barcode_Image].Format, _dtls[(int)bCodeData.Barcode_Image].RotationPoint, proBarcode[index].Product_Code);
                }
                else if (bField == 1)
                {
                    bCodeImage = BarcodeImageEncodeMethod(_dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height, _dtls[(int)bCodeData.Barcode_Image].Format, _dtls[(int)bCodeData.Barcode_Image].RotationPoint, proBarcode[index].Barcode);
                }
                else if (bField == 2 )
                {
                    bCodeImage = BarcodeImageEncodeMethod(_dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height, _dtls[(int)bCodeData.Barcode_Image].Format, _dtls[(int)bCodeData.Barcode_Image].RotationPoint, proBarcode[index].Batch_Barecode);
                }
                else 
                {
                    bCodeImage = BarcodeImageEncodeMethod(_dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height, _dtls[(int)bCodeData.Barcode_Image].Format, _dtls[(int)bCodeData.Barcode_Image].RotationPoint, proBarcode[index].Unit_Barcode);
                }

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(backcolor);
                    g.DrawImage(bCodeImage, _dtls[(int)bCodeData.Barcode_Image].Position_x, _dtls[(int)bCodeData.Barcode_Image].Position_y, _dtls[(int)bCodeData.Barcode_Image].Width, _dtls[(int)bCodeData.Barcode_Image].Height);
                    if (_dtls[(int)bCodeData.Sl_No].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Sl_No].Alignment, _dtls[(int)bCodeData.Sl_No].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Sl_No].Value + proBarcode[index].Sl_No.ToString(), new Font(_dtls[(int)bCodeData.Sl_No].fontFamily, _dtls[(int)bCodeData.Sl_No].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Sl_No].Position_x, _dtls[(int)bCodeData.Sl_No].Position_y, _dtls[(int)bCodeData.Sl_No].Width, _dtls[(int)bCodeData.Sl_No].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Company_Name].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Company_Name].Alignment, _dtls[(int)bCodeData.Company_Name].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Company_Name].Value, new Font(_dtls[(int)bCodeData.Company_Name].fontFamily, _dtls[(int)bCodeData.Company_Name].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Company_Name].Position_x, _dtls[(int)bCodeData.Company_Name].Position_y, _dtls[(int)bCodeData.Company_Name].Width, _dtls[(int)bCodeData.Company_Name].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Product_Code].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Product_Code].Alignment, _dtls[(int)bCodeData.Product_Code].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Product_Code].Value + proBarcode[index].Product_Code, new Font(_dtls[(int)bCodeData.Product_Code].fontFamily, _dtls[(int)bCodeData.Product_Code].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Product_Code].Position_x, _dtls[(int)bCodeData.Product_Code].Position_y, _dtls[(int)bCodeData.Product_Code].Width, _dtls[(int)bCodeData.Product_Code].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Product_Name].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Product_Name].Alignment, _dtls[(int)bCodeData.Product_Name].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Product_Name].Value + proBarcode[index].Product_Name, new Font(_dtls[(int)bCodeData.Product_Name].fontFamily, _dtls[(int)bCodeData.Product_Name].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Product_Name].Position_x, _dtls[(int)bCodeData.Product_Name].Position_y, _dtls[(int)bCodeData.Product_Name].Width, _dtls[(int)bCodeData.Product_Name].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Product_Description].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Product_Description].Alignment, _dtls[(int)bCodeData.Product_Description].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Product_Description].Value.Trim() + proBarcode[index].Product_Description, new Font(_dtls[(int)bCodeData.Product_Description].fontFamily, _dtls[(int)bCodeData.Product_Description].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Product_Description].Position_x, _dtls[(int)bCodeData.Product_Description].Position_y, _dtls[(int)bCodeData.Product_Description].Width, _dtls[(int)bCodeData.Product_Description].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Group_Name].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Group_Name].Alignment, _dtls[(int)bCodeData.Group_Name].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Group_Name].Value + proBarcode[index].Group_Name, new Font(_dtls[(int)bCodeData.Group_Name].fontFamily, _dtls[(int)bCodeData.Group_Name].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Group_Name].Position_x, _dtls[(int)bCodeData.Group_Name].Position_y, _dtls[(int)bCodeData.Group_Name].Width, _dtls[(int)bCodeData.Group_Name].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Batch].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Batch].Alignment, _dtls[(int)bCodeData.Batch].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Batch].Value + proBarcode[index].Batch, new Font(_dtls[(int)bCodeData.Batch].fontFamily, _dtls[(int)bCodeData.Batch].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Batch].Position_x, _dtls[(int)bCodeData.Batch].Position_y, _dtls[(int)bCodeData.Batch].Width, _dtls[(int)bCodeData.Batch].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Unit].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Unit].Alignment, _dtls[(int)bCodeData.Unit].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Unit].Value + proBarcode[index].Unit, new Font(_dtls[(int)bCodeData.Unit].fontFamily, _dtls[(int)bCodeData.Unit].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Unit].Position_x, _dtls[(int)bCodeData.Unit].Position_y, _dtls[(int)bCodeData.Unit].Width, _dtls[(int)bCodeData.Unit].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.MultiRate_Name].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.MultiRate_Name].Alignment, _dtls[(int)bCodeData.MultiRate_Name].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.MultiRate_Name].Value + proBarcode[index].MultiRate_Name, new Font(_dtls[(int)bCodeData.MultiRate_Name].fontFamily, _dtls[(int)bCodeData.MultiRate_Name].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.MultiRate_Name].Position_x, _dtls[(int)bCodeData.MultiRate_Name].Position_y, _dtls[(int)bCodeData.MultiRate_Name].Width, _dtls[(int)bCodeData.MultiRate_Name].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Purchase_Rate].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Purchase_Rate].Alignment, _dtls[(int)bCodeData.Purchase_Rate].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Purchase_Rate].Value + proBarcode[index].Purchase_Rate.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.Purchase_Rate].fontFamily, _dtls[(int)bCodeData.Purchase_Rate].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.Purchase_Rate].Position_x, _dtls[(int)bCodeData.Purchase_Rate].Position_y), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Sales_Rate].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Sales_Rate].Alignment, _dtls[(int)bCodeData.Sales_Rate].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Sales_Rate].Value + proBarcode[index].Sales_Rate.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.Sales_Rate].fontFamily, _dtls[(int)bCodeData.Sales_Rate].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.Sales_Rate].Position_x, _dtls[(int)bCodeData.Sales_Rate].Position_y), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Inclusive_Rate].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Inclusive_Rate].Alignment, _dtls[(int)bCodeData.Inclusive_Rate].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Inclusive_Rate].Value + proBarcode[index].Inclusive_Rate.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.Inclusive_Rate].fontFamily, _dtls[(int)bCodeData.Inclusive_Rate].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.Inclusive_Rate].Position_x, _dtls[(int)bCodeData.Inclusive_Rate].Position_y), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Unit_Price].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Unit_Price].Alignment, _dtls[(int)bCodeData.Unit_Price].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Unit_Price].Value + proBarcode[index].Unit_Price.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.Unit_Price].fontFamily, _dtls[(int)bCodeData.Unit_Price].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.Unit_Price].Position_x, _dtls[(int)bCodeData.Unit_Price].Position_y), fFormat);
                    }
                    if (_dtls[(int)bCodeData.MRP].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.MRP].Alignment, _dtls[(int)bCodeData.MRP].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.MRP].Value + proBarcode[index].MRP.ToString(NumberFormat), new Font(_dtls[(int)bCodeData.MRP].fontFamily, _dtls[(int)bCodeData.MRP].fontSize, fStyle), new SolidBrush(Color.Black), new Point(_dtls[(int)bCodeData.MRP].Position_x, _dtls[(int)bCodeData.MRP].Position_y), fFormat);
                    }
                    if (_dtls[(int)bCodeData.UserField_1].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.UserField_1].Alignment, _dtls[(int)bCodeData.UserField_1].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.UserField_1].Value + proBarcode[index].UserField_1, new Font(_dtls[(int)bCodeData.UserField_1].fontFamily, _dtls[(int)bCodeData.UserField_1].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.UserField_1].Position_x, _dtls[(int)bCodeData.UserField_1].Position_y, _dtls[(int)bCodeData.UserField_1].Width, _dtls[(int)bCodeData.UserField_1].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.UserField_2].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.UserField_2].Alignment, _dtls[(int)bCodeData.UserField_2].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.UserField_2].Value + proBarcode[index].UserField_2, new Font(_dtls[(int)bCodeData.UserField_2].fontFamily, _dtls[(int)bCodeData.UserField_2].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.UserField_2].Position_x, _dtls[(int)bCodeData.UserField_2].Position_y, _dtls[(int)bCodeData.UserField_2].Width, _dtls[(int)bCodeData.UserField_2].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.UserField_3].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.UserField_3].Alignment, _dtls[(int)bCodeData.UserField_3].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.UserField_3].Value + proBarcode[index].UserField_3, new Font(_dtls[(int)bCodeData.UserField_3].fontFamily, _dtls[(int)bCodeData.UserField_3].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.UserField_3].Position_x, _dtls[(int)bCodeData.UserField_3].Position_y, _dtls[(int)bCodeData.UserField_3].Width, _dtls[(int)bCodeData.UserField_3].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.UserField_4].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.UserField_4].Alignment, _dtls[(int)bCodeData.UserField_4].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.UserField_4].Value + proBarcode[index].UserField_4, new Font(_dtls[(int)bCodeData.UserField_4].fontFamily, _dtls[(int)bCodeData.UserField_4].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.UserField_4].Position_x, _dtls[(int)bCodeData.UserField_4].Position_y, _dtls[(int)bCodeData.UserField_4].Width, _dtls[(int)bCodeData.UserField_4].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Price_Code].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Price_Code].Alignment, _dtls[(int)bCodeData.Price_Code].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Price_Code].Value + proBarcode[index].Price_Code, new Font(_dtls[(int)bCodeData.Price_Code].fontFamily, _dtls[(int)bCodeData.Price_Code].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Price_Code].Position_x, _dtls[(int)bCodeData.Price_Code].Position_y, _dtls[(int)bCodeData.Price_Code].Width, _dtls[(int)bCodeData.Price_Code].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Barcode].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Barcode].Alignment, _dtls[(int)bCodeData.Barcode].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Barcode].Value + proBarcode[index].Barcode, new Font(_dtls[(int)bCodeData.Barcode].fontFamily, _dtls[(int)bCodeData.Barcode].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Barcode].Position_x, _dtls[(int)bCodeData.Barcode].Position_y, _dtls[(int)bCodeData.Barcode].Width, _dtls[(int)bCodeData.Barcode].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Batch_Barecode].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Batch_Barecode].Alignment, _dtls[(int)bCodeData.Batch_Barecode].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Batch_Barecode].Value + proBarcode[index].Batch_Barecode, new Font(_dtls[(int)bCodeData.Batch_Barecode].fontFamily, _dtls[(int)bCodeData.Batch_Barecode].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Batch_Barecode].Position_x, _dtls[(int)bCodeData.Batch_Barecode].Position_y, _dtls[(int)bCodeData.Batch_Barecode].Width, _dtls[(int)bCodeData.Batch_Barecode].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Unit_Barcode].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Unit_Barcode].Alignment, _dtls[(int)bCodeData.Unit_Barcode].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Unit_Barcode].Value + proBarcode[index].Unit_Barcode, new Font(_dtls[(int)bCodeData.Unit_Barcode].fontFamily, _dtls[(int)bCodeData.Unit_Barcode].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Unit_Barcode].Position_x, _dtls[(int)bCodeData.Unit_Barcode].Position_y, _dtls[(int)bCodeData.Unit_Barcode].Width, _dtls[(int)bCodeData.Unit_Barcode].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Ledger_Code].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Ledger_Code].Alignment, _dtls[(int)bCodeData.Ledger_Code].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Ledger_Code].Value + proBarcode[index].Ledger_Code, new Font(_dtls[(int)bCodeData.Ledger_Code].fontFamily, _dtls[(int)bCodeData.Ledger_Code].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Ledger_Code].Position_x, _dtls[(int)bCodeData.Ledger_Code].Position_y, _dtls[(int)bCodeData.Ledger_Code].Width, _dtls[(int)bCodeData.Ledger_Code].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Mfg_Date].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Mfg_Date].Alignment, _dtls[(int)bCodeData.Mfg_Date].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Mfg_Date].Value + proBarcode[index].Mfg_Date, new Font(_dtls[(int)bCodeData.Mfg_Date].fontFamily, _dtls[(int)bCodeData.Mfg_Date].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Mfg_Date].Position_x, _dtls[(int)bCodeData.Mfg_Date].Position_y, _dtls[(int)bCodeData.Mfg_Date].Width, _dtls[(int)bCodeData.Mfg_Date].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Exparing_Date].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Exparing_Date].Alignment, _dtls[(int)bCodeData.Exparing_Date].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Exparing_Date].Value + proBarcode[index].Exparing_Date, new Font(_dtls[(int)bCodeData.Exparing_Date].fontFamily, _dtls[(int)bCodeData.Exparing_Date].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Exparing_Date].Position_x, _dtls[(int)bCodeData.Exparing_Date].Position_y, _dtls[(int)bCodeData.Exparing_Date].Width, _dtls[(int)bCodeData.Exparing_Date].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.item_Note].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.item_Note].Alignment, _dtls[(int)bCodeData.item_Note].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.item_Note].Value + proBarcode[index].item_Note, new Font(_dtls[(int)bCodeData.item_Note].fontFamily, _dtls[(int)bCodeData.item_Note].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.item_Note].Position_x, _dtls[(int)bCodeData.item_Note].Position_y, _dtls[(int)bCodeData.item_Note].Width, _dtls[(int)bCodeData.item_Note].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.User_Type_1].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.User_Type_1].Alignment, _dtls[(int)bCodeData.User_Type_1].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.User_Type_1].Value + proBarcode[index].User_Type_1, new Font(_dtls[(int)bCodeData.User_Type_1].fontFamily, _dtls[(int)bCodeData.User_Type_1].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.User_Type_1].Position_x, _dtls[(int)bCodeData.User_Type_1].Position_y, _dtls[(int)bCodeData.User_Type_1].Width, _dtls[(int)bCodeData.User_Type_1].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.User_Type_2].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.User_Type_2].Alignment, _dtls[(int)bCodeData.User_Type_2].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.User_Type_2].Value + proBarcode[index].User_Type_2, new Font(_dtls[(int)bCodeData.User_Type_2].fontFamily, _dtls[(int)bCodeData.User_Type_2].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.User_Type_2].Position_x, _dtls[(int)bCodeData.User_Type_2].Position_y, _dtls[(int)bCodeData.User_Type_2].Width, _dtls[(int)bCodeData.User_Type_2].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.User_Type_3].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.User_Type_3].Alignment, _dtls[(int)bCodeData.User_Type_3].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.User_Type_3].Value + proBarcode[index].User_Type_3, new Font(_dtls[(int)bCodeData.User_Type_3].fontFamily, _dtls[(int)bCodeData.User_Type_3].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.User_Type_3].Position_x, _dtls[(int)bCodeData.User_Type_3].Position_y, _dtls[(int)bCodeData.User_Type_3].Width, _dtls[(int)bCodeData.User_Type_3].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.User_Type_4].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.User_Type_4].Alignment, _dtls[(int)bCodeData.User_Type_4].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.User_Type_4].Value + proBarcode[index].User_Type_4, new Font(_dtls[(int)bCodeData.User_Type_4].fontFamily, _dtls[(int)bCodeData.User_Type_4].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.User_Type_4].Position_x, _dtls[(int)bCodeData.User_Type_4].Position_y, _dtls[(int)bCodeData.User_Type_4].Width, _dtls[(int)bCodeData.User_Type_4].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Custome_String_1].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Custome_String_1].Alignment, _dtls[(int)bCodeData.Custome_String_1].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Custome_String_1].Value, new Font(_dtls[(int)bCodeData.Custome_String_1].fontFamily, _dtls[(int)bCodeData.Custome_String_1].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Custome_String_1].Position_x, _dtls[(int)bCodeData.Custome_String_1].Position_y, _dtls[(int)bCodeData.Custome_String_1].Width, _dtls[(int)bCodeData.Custome_String_1].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Custome_String_2].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Custome_String_2].Alignment, _dtls[(int)bCodeData.Custome_String_2].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Custome_String_2].Value, new Font(_dtls[(int)bCodeData.Custome_String_2].fontFamily, _dtls[(int)bCodeData.Custome_String_2].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Custome_String_2].Position_x, _dtls[(int)bCodeData.Custome_String_2].Position_y, _dtls[(int)bCodeData.Custome_String_2].Width, _dtls[(int)bCodeData.Custome_String_2].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Custome_String_3].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Custome_String_3].Alignment, _dtls[(int)bCodeData.Custome_String_3].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Custome_String_3].Value, new Font(_dtls[(int)bCodeData.Custome_String_3].fontFamily, _dtls[(int)bCodeData.Custome_String_3].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Custome_String_3].Position_x, _dtls[(int)bCodeData.Custome_String_3].Position_y, _dtls[(int)bCodeData.Custome_String_3].Width, _dtls[(int)bCodeData.Custome_String_3].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Custome_String_4].isIncluded)
                    {
                        getStringFormat(_dtls[(int)bCodeData.Custome_String_4].Alignment, _dtls[(int)bCodeData.Custome_String_4].fStyle);
                        g.DrawString(_dtls[(int)bCodeData.Custome_String_4].Value, new Font(_dtls[(int)bCodeData.Custome_String_4].fontFamily, _dtls[(int)bCodeData.Custome_String_4].fontSize, fStyle), new SolidBrush(Color.Black), new RectangleF(_dtls[(int)bCodeData.Custome_String_4].Position_x, _dtls[(int)bCodeData.Custome_String_4].Position_y, _dtls[(int)bCodeData.Custome_String_4].Width, _dtls[(int)bCodeData.Custome_String_4].Height), fFormat);
                    }
                    if (_dtls[(int)bCodeData.Logo].isIncluded)
                    {
                        Image imgLogo = Image.FromFile(_dtls[(int)bCodeData.Logo].Value);
                        g.DrawImage(imgLogo, _dtls[(int)bCodeData.Logo].Position_x, _dtls[(int)bCodeData.Logo].Position_y, _dtls[(int)bCodeData.Logo].Width, _dtls[(int)bCodeData.Logo].Height);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return (Image)bitmap;
        }
        public Image BarcodeImageEncodeMethod(int bWidth, int bHeight,  string bEncodeType, string strRotateType, string strData)
        {
            Barcode bCode = new Barcode();
            bCode.Alignment = AlignmentPositions.CENTER;
            Image bCodeImage = null;
            TYPE type = TYPE.UNSPECIFIED;
            switch (bEncodeType)
            {
                case "UPC-A": type = TYPE.UPCA; break;
                case "UPC-E": type = TYPE.UPCE; break;
                case "UPC 2 Digit Ext.": type = TYPE.UPC_SUPPLEMENTAL_2DIGIT; break;
                case "UPC 5 Digit Ext.": type = TYPE.UPC_SUPPLEMENTAL_5DIGIT; break;
                case "EAN-13": type = TYPE.EAN13; break;
                case "JAN-13": type = TYPE.JAN13; break;
                case "EAN-8": type = TYPE.EAN8; break;
                case "ITF-14": type = TYPE.ITF14; break;
                case "Codabar": type = TYPE.Codabar; break;
                case "PostNet": type = TYPE.PostNet; break;
                case "Bookland/ISBN": type = TYPE.BOOKLAND; break;
                case "Code 11": type = TYPE.CODE11; break;
                case "Code 39": type = TYPE.CODE39; break;
                case "Code 39 Extended": type = TYPE.CODE39Extended; break;
                case "Code 39 Mod 43": type = TYPE.CODE39_Mod43; break;
                case "Code 93": type = TYPE.CODE93; break;
                case "LOGMARS": type = TYPE.LOGMARS; break;
                case "MSI": type = TYPE.MSI_Mod10; break;
                case "Interleaved 2 of 5": type = TYPE.Interleaved2of5; break;
                case "Standard 2 of 5": type = TYPE.Standard2of5; break;
                case "Code 128": type = TYPE.CODE128; break;
                case "Code 128-A": type = TYPE.CODE128A; break;
                case "Code 128-B": type = TYPE.CODE128B; break;
                case "Code 128-C": type = TYPE.CODE128C; break;
                case "Telepen": type = TYPE.TELEPEN; break;
                case "FIM": type = TYPE.FIM; break;
                default: type = TYPE.PHARMACODE; break;
            }//switch
            try
            {
                if (type != TYPE.UNSPECIFIED)
                {
                    bCode.IncludeLabel = true;
                    //bCode.RotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), strRotateType, true);
                    //label alignment and position
                    bCode.LabelPosition = LabelPositions.BOTTOMCENTER;
                    //===== Encoding performed here =====
                    bCodeImage = bCode.Encode(type, strData, Color.Black, Color.White, bWidth, bHeight);
                }
            }//try
            catch (Exception)
            {
                throw;
            }//catch
            return (Image)bCodeImage;
        }
        #endregion

        #region Class Events
        void pDocu_PrintPage(object sender, PrintPageEventArgs e)
        {
            int intAcrossStarting = 0;
            int intDownStarting = 0;
            int intOdd = 0;
            double dmlTr = 0;
            int x = 0;
            int y = 0;
            int intAcsNo = 0;
            intColmWdth = bHdr.Width + bHdr.RightMargin;
            intColmHt = bHdr.Height + bHdr.BottomMargin;
            intPrintableWth = (intColmWdth * bHdr.Accross) - bHdr.FirstMargin;
            x = bHdr.FirstMargin;
            y = bHdr.TopMargin;
            while (intPrintedLabelCount < intTotal)
            {
                if (isFirst)
                {
                    if (_intStartingPoint != 0)
                    {
                        dmlTr = _intStartingPoint / bHdr.Accross;
                        intDownStarting = (Convert.ToInt32(Math.Truncate(dmlTr)) * intColmHt) + bHdr.TopMargin;
                        intOdd = _intStartingPoint % bHdr.Accross;
                        if (intOdd != 0)
                        {
                            intAcrossStarting = (intOdd * intColmWdth) + bHdr.FirstMargin;
                            intAcsNo = intOdd;
                        }
                        else
                        {
                            intAcrossStarting = bHdr.FirstMargin;
                        }
                        x = intAcrossStarting;
                        y = intDownStarting;
                        intItemPrinted = _intStartingPoint;
                    }
                    intNoOFClm = Convert.ToInt32(Math.Round(barcodeList[intRowIndex].Quantity,0));
                    isFirst = false;
                }
                getBarcodeImage(barcodeList, _dtls, intRowIndex, bHdr.Width, bHdr.Height, Color.Black, Color.White, bHdr.BarcodeField);

                if (intPrintedLabelCount + 1 == intNoOFClm)
                {
                    if (intRowIndex < barcodeList.Count - 1)
                    {
                        intRowIndex++;
                        intNoOFClm = intNoOFClm + Convert.ToInt32(Math.Round(barcodeList[intRowIndex].Quantity,0));
                    }
                }
                if (img != null)
                {
                    if (bHdr.isBorderIncluded)
                    {
                        e.Graphics.DrawRectangle(Pens.Black, x, y, bHdr.Width, bHdr.Height);
                    }
                    e.Graphics.DrawImage(img, new RectangleF(x, y, bHdr.Width, bHdr.Height));
                    if (intAcsNo < bHdr.Accross - 1)
                    {
                        intAcsNo = intAcsNo + 1;
                        x = x + intColmWdth;
                    }
                    else
                    {
                        intAcsNo = 0;
                        x = bHdr.FirstMargin;
                        y = y + intColmHt;
                    }
                }
                intItemPrinted++;
                intPrintedLabelCount++;
                if (intItemPrinted < bHdr.TotalPerPage)
                {
                    e.HasMorePages = false;
                }
                else
                {
                    if (intPrintedLabelCount != intTotal)
                    {
                        intItemPrinted = 0;
                        e.HasMorePages = true;
                    }
                    return;
                }
            }

            intPrintedLabelCount = 0;
        }
        void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
        }
        void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
        }
        #endregion
    }
}
