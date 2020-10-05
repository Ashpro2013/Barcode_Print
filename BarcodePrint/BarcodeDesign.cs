using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Xml.Serialization;

namespace Ashpro
{
    public partial class BarcodeDesign : Form
    {
        #region constructor
        public BarcodeDesign()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Variable
        ToolTip toolTip;
        Barcode b = new Barcode();
        bool mouseClickedOnBCode = false;
        bool mouseClickedOnPbBarcode = false;
        bool mouseClickedOnBLabel = false;
        bool mouseClickedOnCstmTBox = false;
        bool mouseClickedOnCstmLogo = false;
        Point PanelMouseDownLocation;
        BarcodeData proBarcode;
        List<BarcodeData> barcodeList;
        BarcodeFormatClass _objFormat = new BarcodeFormatClass();
        List<BarcodeDTL> _dtls = new List<BarcodeDTL>();
        double PaperWidth, PaperHieght, intTtlnoPerPage, intWidth, lw, lh;
        int intDown, intAcross;
        RotatingLabel txtBox;
        List<string> itemName = new List<string>();
        string strControl = string.Empty;
        ProductBarcode pBarcode;
        bool isIncluded = true;
        Font font;
        string strCntrlName;
        int intIndex;
        int intMsr = 0;
        BarcodeHDR bhdr;
        FontStyle fStyle = new FontStyle();
        int intLabelWidth, intLabelHeight, intRtMgn, intBtmMgn;
        double dblRtMargin, dblBtmMargin;
        bool isMrgnKeyPressed = false;
        bool isLblKeyPressed = false;
        bool isTestingPanelClicked = false;
        string strFileName = string.Empty;
        PictureBox pbLogo;
        TextBox tBox;
        #endregion

        #region Functions
        void ShowToolTip()
        {
            toolTip = new ToolTip();
            toolTip.SetToolTip(btnSave,"Save " + " [F8]");
            toolTip.SetToolTip(btnOpen, "Open " + " [F11]");
            toolTip.SetToolTip(btnEncode, "Generate " + " [F2]");
            toolTip.SetToolTip(btnAdd, "Add");
            toolTip.SetToolTip(btnDelete,"Delete");
        }
        private bool ValidationMwthod()
        {
            errProvider.Clear();
            if (lblPrintPerPage.Text.Trim() == "0")
            {
                errProvider.SetError(lblPrintPerPage, "Value Must be greater than zero");
                return false;
            }
            if (txtLabelWth.Text == "0" || txtLabelWth.Text == string.Empty)
            {
                errProvider.SetError(txtLabelWth, "Value Must be greater than zero");
                txtLabelWth.Focus();
                return false;
            }
            if (txtLabelHt.Text == "0" || txtLabelHt.Text == string.Empty)
            {
                errProvider.SetError(txtLabelHt, "Value Must be greater than zero");
                txtLabelHt.Focus();
                return false;
            }
            return true;
        }
        private void EditMethod()
        {
            try
            {
                if (strControl != string.Empty)
                {
                    if (strControl == "35")
                    {
                        intIndex = 35;
                        _dtls[intIndex].Name = "35";
                        _dtls[intIndex].Text = strControl;
                        _dtls[intIndex].Value = txtPrefixVal.Text;
                        _dtls[intIndex].isIncluded = true;
                        if (rdbPx.Checked)
                        {
                            _dtls[intIndex].Position_x = Convert.ToInt32(txtBarcodeX.Text);
                            _dtls[intIndex].Position_y = Convert.ToInt32(txtBarcodeY.Text);
                            _dtls[intIndex].Width = Convert.ToInt32(txtWidth.Text);
                            _dtls[intIndex].Height = Convert.ToInt32(txtHeight.Text);
                        }
                        else if (rdbCM.Checked)
                        {
                            _dtls[intIndex].Position_x = Convert.ToInt32(Convert.ToDouble(txtBarcodeX.Text) * 39.3701);
                            _dtls[intIndex].Position_y = Convert.ToInt32(Convert.ToDouble(txtBarcodeY.Text) * 39.3701);
                            _dtls[intIndex].Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 39.3701);
                            _dtls[intIndex].Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 39.3701);
                        }
                        else
                        {
                            _dtls[intIndex].Position_x = Convert.ToInt32(Convert.ToDouble(txtBarcodeX.Text) * 100);
                            _dtls[intIndex].Position_y = Convert.ToInt32(Convert.ToDouble(txtBarcodeY.Text) * 100);
                            _dtls[intIndex].Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 100);
                            _dtls[intIndex].Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 100);
                        }
                        _dtls[intIndex].bAlignment = cmbAlignment.Text;
                        _dtls[intIndex].RotationPoint = string.Empty;
                        _dtls[intIndex].fontFamily = Barcode_Panel.Font.FontFamily.ToString();
                        _dtls[intIndex].fontSize = Convert.ToInt32(Barcode_Panel.Font.Size);
                        _dtls[intIndex].fStyle = Barcode_Panel.Font.Style.ToString();
                        _dtls[intIndex].Format = cbEncodeType.Text;
                    }
                    else if (strControl == "34")
                    {
                        intIndex = 34;
                        _dtls[intIndex].Name = "34";
                        _dtls[intIndex].Text = strControl;
                        _dtls[intIndex].Value = txtPrefixVal.Text;
                        _dtls[intIndex].isIncluded = true;
                        if (rdbPx.Checked)
                        {
                            _dtls[intIndex].Position_x = Convert.ToInt32(txtBarcodeX.Text);
                            _dtls[intIndex].Position_y = Convert.ToInt32(txtBarcodeY.Text);
                            _dtls[intIndex].Width = Convert.ToInt32(txtWidth.Text);
                            _dtls[intIndex].Height = Convert.ToInt32(txtHeight.Text);
                        }
                        else if (rdbCM.Checked)
                        {
                            _dtls[intIndex].Position_x = Convert.ToInt32(Convert.ToDouble(txtBarcodeX.Text) * 39.3701);
                            _dtls[intIndex].Position_y = Convert.ToInt32(Convert.ToDouble(txtBarcodeY.Text) * 39.3701);
                            _dtls[intIndex].Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 39.3701);
                            _dtls[intIndex].Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 39.3701);
                        }
                        else
                        {
                            _dtls[intIndex].Position_x = Convert.ToInt32(Convert.ToDouble(txtBarcodeX.Text) * 100);
                            _dtls[intIndex].Position_y = Convert.ToInt32(Convert.ToDouble(txtBarcodeY.Text) * 100);
                            _dtls[intIndex].Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 100);
                            _dtls[intIndex].Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 100);
                        }
                        _dtls[intIndex].bAlignment = cmbAlignment.Text;
                        _dtls[intIndex].RotationPoint = string.Empty;
                        _dtls[intIndex].fontFamily = Barcode_Panel.Font.FontFamily.ToString();
                        _dtls[intIndex].fontSize = Convert.ToInt32(Barcode_Panel.Font.Size);
                        _dtls[intIndex].fStyle = Barcode_Panel.Font.Style.ToString();
                        _dtls[intIndex].Format = cbEncodeType.Text;
                    }
                    else
                    {
                        intIndex = Convert.ToInt32(strControl);
                        _dtls[intIndex].Name = txtBox.Name;
                        _dtls[intIndex].Text = cmbParameter.Items[intIndex].ToString();
                        _dtls[intIndex].Value = txtPrefixVal.Text;
                        _dtls[intIndex].isIncluded = isIncluded;
                        if (rdbPx.Checked)
                        {
                            _dtls[intIndex].Position_x = Convert.ToInt32(txtBarcodeX.Text);
                            _dtls[intIndex].Position_y = Convert.ToInt32(txtBarcodeY.Text);
                            _dtls[intIndex].Width = Convert.ToInt32(txtWidth.Text);
                            _dtls[intIndex].Height = Convert.ToInt32(txtHeight.Text);
                        }
                        else if (rdbCM.Checked)
                        {
                            _dtls[intIndex].Position_x = Convert.ToInt32(Convert.ToDouble(txtBarcodeX.Text) * 39.3701);
                            _dtls[intIndex].Position_y = Convert.ToInt32(Convert.ToDouble(txtBarcodeY.Text) * 39.3701);
                            _dtls[intIndex].Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 39.3701);
                            _dtls[intIndex].Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 39.3701);
                        }
                        else
                        {
                            _dtls[intIndex].Position_x = Convert.ToInt32(Convert.ToDouble(txtBarcodeX.Text) * 100);
                            _dtls[intIndex].Position_y = Convert.ToInt32(Convert.ToDouble(txtBarcodeY.Text) * 100);
                            _dtls[intIndex].Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 100);
                            _dtls[intIndex].Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 100);
                        }
                        if (cmbAlignment.SelectedIndex == 0)
                        {
                            _dtls[intIndex].Alignment = "Left";
                        }
                        else if (cmbAlignment.SelectedIndex == 1)
                        {
                            _dtls[intIndex].Alignment = "Center";
                        }
                        else if (cmbAlignment.SelectedIndex == 2)
                        {
                            _dtls[intIndex].Alignment = "Right";
                        }
                        else
                        {
                            _dtls[intIndex].Alignment = "Vertical";
                        }
                        _dtls[intIndex].bAlignment = string.Empty;
                        _dtls[intIndex].RotationPoint = string.Empty;
                        if (font != null)
                        {
                            _dtls[intIndex].fontFamily = font.FontFamily.ToString();
                            _dtls[intIndex].fontSize = Convert.ToInt32(font.Size);

                            if (font.Style.ToString() == "Regular")
                            {
                                _dtls[intIndex].fStyle = "Regular";
                            }
                            else if (font.Style.ToString() == "Bold")
                            {
                                _dtls[intIndex].fStyle = "Bold";
                            }
                            else
                            {
                                _dtls[intIndex].fStyle = "Italic";
                            }
                        }
                        else
                        {
                            _dtls[intIndex].fontFamily = txtBox.Font.FontFamily.ToString();
                            _dtls[intIndex].fontSize = Convert.ToInt32(txtBox.Font.Size);
                            _dtls[intIndex].fStyle = txtBox.Font.Style.ToString();
                        }
                        _dtls[intIndex].Format = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                strControl = string.Empty;
                txtControl.Text = strControl;
                foreach (Control item in pnlLabel.Controls.OfType<Label>())
                {
                    item.ForeColor = Color.Black;
                }
            }
        }
        private void AddMethod(string text, string Name, string value, int intX, int intY, int Wdth, int Htt)
        {
            if (Name != "34")
            {
                txtBox = new RotatingLabel();
                txtBox.AutoSize = false;
                txtBox.TextAlign = ContentAlignment.TopCenter;
                txtBox.Name = Name;
                txtBox.NewText = text;
                txtWidth.Text = txtBox.Width.ToString();
                txtHeight.Text = txtBox.Font.Height.ToString();
                txtBarcodeX.Text = intX.ToString();
                txtBarcodeY.Text = intY.ToString();
                txtBox.Location = new System.Drawing.Point(intX, intY);
                txtBox.MouseDown += new MouseEventHandler(txtBox_MouseDown);
                txtBox.MouseUp += new MouseEventHandler(txtBox_MouseUp);
                txtBox.MouseMove += new MouseEventHandler(txtBox_MouseMove);
                pnlLabel.Controls.Add(txtBox);
            }
            else
            {
                pbLogo = new PictureBox();
                if (value == null || value == string.Empty)
                {
                    pbLogo.BackgroundImage = Properties.Resources.Background_Image;
                }
                else
                {
                    pbLogo.BackgroundImage = Image.FromFile(value);
                }
                pbLogo.BackgroundImageLayout = ImageLayout.Stretch;
                pbLogo.Name = Name;
                pbLogo.BorderStyle = BorderStyle.FixedSingle;
                pbLogo.Location = new Point(intX, intY);
                pbLogo.Size = new Size(Wdth, Htt);
                pbLogo.MouseDown += new MouseEventHandler(pbLogo_MouseDown);
                pbLogo.MouseUp += new MouseEventHandler(pbLogo_MouseUp);
                pbLogo.MouseMove += new MouseEventHandler(pbLogo_MouseMove);
                txtWidth.Text = Wdth.ToString();
                txtHeight.Text = Htt.ToString();
                txtBarcodeX.Text = intX.ToString();
                txtBarcodeY.Text = intY.ToString();
                pnlLabel.Controls.Add(pbLogo);
            }
        }
        void pbLogo_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseClickedOnCstmLogo)
            {
                pbLogo.Left += e.X - PanelMouseDownLocation.X;
                pbLogo.Top += e.Y - PanelMouseDownLocation.Y;
                if (intMsr == 0)
                {
                    txtBarcodeX.Text = pbLogo.Location.X.ToString();
                    txtBarcodeY.Text = pbLogo.Location.Y.ToString();
                }
                else if (intMsr == 1)
                {
                    txtBarcodeX.Text = (pbLogo.Location.X / 39.3701).ToString("F");
                    txtBarcodeY.Text = (pbLogo.Location.Y / 39.3701).ToString("F");
                }
                else
                {
                    txtBarcodeX.Text = (pbLogo.Location.X / 100).ToString("F");
                    txtBarcodeY.Text = (pbLogo.Location.Y / 100).ToString("F");
                }
            }
        }
        void pbLogo_MouseUp(object sender, MouseEventArgs e)
        {
            mouseClickedOnCstmLogo = false;
        }
        void pbLogo_MouseDown(object sender, MouseEventArgs e)
        {
            btnLogo.Visible = true;
            btnFont.Visible = false;
            lblControlPrefix.Text = "File Path";
            mouseClickedOnCstmLogo = true;
            if (strControl != string.Empty)
            {
                if (strControl == "34" && txtPrefixVal.Text != string.Empty)
                {
                    EditMethod();
                }
                else
                {
                    EditMethod();
                }
            }
            pbLogo = (PictureBox)sender;
            btnLogo.Visible = true;
            strControl = pbLogo.Name;
            strCntrlName = "Logo";
            txtControl.Text = strCntrlName;
            cmbBarcodeValue.Visible = false;
            txtPrefixVal.Visible = true;
            if (e.Button == MouseButtons.Left) PanelMouseDownLocation = e.Location;
            if (_dtls[Convert.ToInt32(34)].Value != string.Empty)
            {
                txtPrefixVal.Text = _dtls[Convert.ToInt32(34)].Value;
            }
            if (intMsr == 0)
            {
                txtBarcodeX.Text = _dtls[Convert.ToInt32(34)].Position_x.ToString();
                txtBarcodeY.Text = _dtls[Convert.ToInt32(34)].Position_y.ToString();
                txtHeight.Text = _dtls[Convert.ToInt32(34)].Height.ToString();
                txtWidth.Text = _dtls[Convert.ToInt32(34)].Width.ToString();
            }
            else if (intMsr == 1)
            {
                txtBarcodeX.Text = (_dtls[Convert.ToInt32(34)].Position_x / 39.3701).ToString("F");
                txtBarcodeY.Text = (_dtls[Convert.ToInt32(34)].Position_y / 39.3701).ToString("F");
                txtHeight.Text = (_dtls[Convert.ToInt32(34)].Height / 39.3701).ToString("F");
                txtWidth.Text = (_dtls[Convert.ToInt32(34)].Width / 39.3701).ToString("F");
            }
            else
            {
                txtBarcodeX.Text = (_dtls[Convert.ToInt32(34)].Position_x * 100).ToString("F");
                txtBarcodeY.Text = (_dtls[Convert.ToInt32(34)].Position_y * 100).ToString("F");
                txtHeight.Text = (_dtls[Convert.ToInt32(34)].Height * 100).ToString("F");
                txtWidth.Text = (_dtls[Convert.ToInt32(34)].Width * 100).ToString("F");
            }
        }
        private void BarcodeHeaderMethod()
        {
            bhdr = new BarcodeHDR();
            bhdr.DesignName = "New";
            bhdr.Accross = Convert.ToInt32(txtAcross.Text);
            bhdr.Down = Convert.ToInt32(txtDown.Text);
            bhdr.TotalPerPage = Convert.ToInt32(lblPrintPerPage.Text);
            bhdr.PrinterName = cmbPrinter.Text;
            bhdr.PaperName = cmbPaperSize.Text;
            if (rdbPx.Checked)
            {
                bhdr.FirstMargin = Convert.ToInt32(txtLeft.Text);
                bhdr.TopMargin = Convert.ToInt32(txtTop.Text);
                bhdr.RightMargin = Convert.ToInt32(txtRight.Text);
                bhdr.BottomMargin = Convert.ToInt32(txtBottom.Text);
                bhdr.Width = Convert.ToInt32(txtLabelWth.Text);
                bhdr.Height = Convert.ToInt32(txtLabelHt.Text);
                bhdr.PSWidth = Convert.ToInt32(txtPSWdth.Text);
                bhdr.PSHieght = Convert.ToInt32(txtPSHt.Text);
            }
            else if (rdbCM.Checked)
            {
                bhdr.FirstMargin = Convert.ToInt32(Convert.ToDouble(txtLeft.Text) * 39.3701);
                bhdr.TopMargin = Convert.ToInt32(Convert.ToDouble(txtTop.Text) * 39.3701);
                bhdr.RightMargin = Convert.ToInt32(Convert.ToDouble(txtRight.Text) * 39.3701);
                bhdr.BottomMargin = Convert.ToInt32(Convert.ToDouble(txtBottom.Text) * 39.3701);
                bhdr.Width = Convert.ToInt32(Convert.ToDouble(txtLabelWth.Text) * 39.3701);
                bhdr.Height = Convert.ToInt32(Convert.ToDouble(txtLabelHt.Text) * 39.3701);
                bhdr.PSWidth = Convert.ToInt32(Convert.ToDouble(txtPSWdth.Text) * 39.3701);
                bhdr.PSHieght = Convert.ToInt32(Convert.ToDouble(txtPSHt.Text) * 39.3701);
            }
            else
            {
                bhdr.FirstMargin = Convert.ToInt32(Convert.ToDouble(txtLeft.Text) * 100);
                bhdr.TopMargin = Convert.ToInt32(Convert.ToDouble(txtTop.Text) * 100);
                bhdr.RightMargin = Convert.ToInt32(Convert.ToDouble(txtRight.Text) * 100);
                bhdr.BottomMargin = Convert.ToInt32(Convert.ToDouble(txtBottom.Text) * 100);
                bhdr.Width = Convert.ToInt32(Convert.ToDouble(txtLabelWth.Text) * 100);
                bhdr.Height = Convert.ToInt32(Convert.ToDouble(txtLabelHt.Text) * 100);
                bhdr.PSWidth = Convert.ToInt32(Convert.ToDouble(txtPSWdth.Text) * 100);
                bhdr.PSHieght = Convert.ToInt32(Convert.ToDouble(txtPSHt.Text) * 100);
            }
            bhdr.BarcodeField = cmbBarcodeValue.SelectedIndex;
            bhdr.isBorderIncluded = cbBorder.Checked;
        }
        private void LoadControlsMwthod()
        {
            itemName.Clear();
            foreach (Control item in pnlLabel.Controls.OfType<Label>())
            {
                pnlLabel.Controls.Remove(item);
            }
            foreach (Control item in pnlLabel.Controls.OfType<PictureBox>())
            {
                pnlLabel.Controls.Remove(item);
            }
            for (int i = 0; i < _dtls.Count; i++)
            {
                if (_dtls[i].isIncluded)
                {
                    if (_dtls[i].Name != "35")
                    {
                        AddMethod(_dtls[i].Text, _dtls[i].Name, _dtls[i].Value, _dtls[i].Position_x, _dtls[i].Position_y, _dtls[i].Width, _dtls[i].Height);
                        if (_dtls[i].Alignment == "Vertical")
                        {
                            txtBox.AutoSize = false;      // adjust according to your text
                            txtBox.Text = "";
                            txtBox.ForeColor = Color.Black;  // color to display
                            txtBox.RotateAngle = 90;
                            txtBox.Refresh();
                        }
                    }
                    else
                    {
                        Barcode_Panel.Size = new Size(_dtls[i].Width, _dtls[i].Height);
                        Barcode_Panel.Location = new Point(_dtls[i].Position_x, _dtls[i].Position_y);
                    }
                }
            }
        }
        private void AddDetailListMethod()
        {
            _dtls.Clear();

            #region SerialNumbner
            BarcodeDTL _dtl0 = new BarcodeDTL();
            _dtl0.Name = "0";
            _dtl0.Text = "SL No";
            _dtl0.Value = "Sl.No :";
            _dtl0.isIncluded = false;
            _dtl0.Position_x = 0;
            _dtl0.Position_y = 0;
            _dtl0.Width = 100;
            _dtl0.Height = 15;
            _dtl0.Alignment = "Left";
            _dtl0.RotationPoint = "360";
            _dtl0.fontFamily = ("Open Sans");
            _dtl0.fontSize = 9;
            _dtl0.fStyle = "Regular";
            _dtl0.Format = "test";
            _dtl0.bAlignment = "test";
            _dtls.Add(_dtl0);
            #endregion

            #region Company Name
            BarcodeDTL _dtl1 = new BarcodeDTL();
            _dtl1.Name = "1";
            _dtl1.Text = "Company Name";
            _dtl1.Value = "Company :";
            _dtl1.isIncluded = false;
            _dtl1.Position_x = 0;
            _dtl1.Position_y = 0;
            _dtl1.Width = 100;
            _dtl1.Height = 15;
            _dtl1.Alignment = "Left";
            _dtl1.RotationPoint = "360";
            _dtl1.fontFamily = ("Open Sans");
            _dtl1.fontSize = 9;
            _dtl1.fStyle = "Regular";
            _dtl1.Format = "test";
            _dtl1.bAlignment = "test";
            _dtls.Add(_dtl1);
            #endregion

            #region Product Code
            BarcodeDTL _dtl2 = new BarcodeDTL();
            _dtl2.Name = "2";
            _dtl2.Text = "Product Code";
            _dtl2.Value = "Product Code : ";
            _dtl2.isIncluded = false;
            _dtl2.Position_x = 0;
            _dtl2.Position_y = 0;
            _dtl2.Width = 200;
            _dtl2.Height = 15;
            _dtl2.Alignment = "Left";
            _dtl2.RotationPoint = "360";
            _dtl2.fontFamily = ("Open Sans");
            _dtl2.fontSize = 9;
            _dtl2.fStyle = "Regular";
            _dtl2.Format = "test";
            _dtl2.bAlignment = "test";
            _dtls.Add(_dtl2);
            #endregion

            #region Product Name
            BarcodeDTL _dtl3 = new BarcodeDTL();
            _dtl3.Name = "3";
            _dtl3.Text = "Product Name";
            _dtl3.Value = "Product Name:";
            _dtl3.isIncluded = false;
            _dtl3.Position_x = 0;
            _dtl3.Position_y = 0;
            _dtl3.Width = 200;
            _dtl3.Height = 15;
            _dtl3.Alignment = "Center";
            _dtl3.RotationPoint = "360";
            _dtl3.fontFamily = ("Open Sans");
            _dtl3.fontSize = 9;
            _dtl3.fStyle = "Regular";
            _dtl3.Format = "test";
            _dtl3.bAlignment = "test";
            _dtls.Add(_dtl3);
            #endregion

            #region Product Description
            BarcodeDTL _dtl4 = new BarcodeDTL();
            _dtl4.Name = "4";
            _dtl4.Text = "Product Description";
            _dtl4.Value = "";
            _dtl4.isIncluded = false;
            _dtl4.Position_x = 0;
            _dtl4.Position_y = 0;
            _dtl4.Width = 200;
            _dtl4.Height = 15;
            _dtl4.Alignment = "Left";
            _dtl4.RotationPoint = "360";
            _dtl4.fontFamily = ("Open Sans");
            _dtl4.fontSize = 9;
            _dtl4.fStyle = "Regular";
            _dtl4.Format = "test";
            _dtl4.bAlignment = "test";
            _dtls.Add(_dtl4);
            #endregion

            #region Group Name
            BarcodeDTL _dtl5 = new BarcodeDTL();
            _dtl5.Name = "5";
            _dtl5.Text = "Group Name";
            _dtl5.Value = "Group :";
            _dtl5.isIncluded = false;
            _dtl5.Position_x = 0;
            _dtl5.Position_y = 0;
            _dtl5.Width = 100;
            _dtl5.Height = 15;
            _dtl5.Alignment = "Right";
            _dtl5.RotationPoint = "360";
            _dtl5.fontFamily = ("Open Sans");
            _dtl5.fontSize = 9;
            _dtl5.fStyle = "Regular";
            _dtl5.Format = "test";
            _dtl5.bAlignment = "test";
            _dtls.Add(_dtl5);
            #endregion

            #region Batch
            BarcodeDTL _dtl6 = new BarcodeDTL();
            _dtl6.Name = "6";
            _dtl6.Text = "Batch";
            _dtl6.Value = "Batch:";
            _dtl6.isIncluded = false;
            _dtl6.Position_x = 0;
            _dtl6.Position_y = 0;
            _dtl6.Width = 100;
            _dtl6.Height = 15;
            _dtl6.Alignment = "Right";
            _dtl6.RotationPoint = "360";
            _dtl6.fontFamily = ("Open Sans");
            _dtl6.fontSize = 9;
            _dtl6.fStyle = "Regular";
            _dtl6.Format = "test";
            _dtl6.bAlignment = "test";
            _dtls.Add(_dtl6);
            #endregion

            #region Unit
            BarcodeDTL _dtl7 = new BarcodeDTL();
            _dtl7.Name = "7";
            _dtl7.Text = "Unit";
            _dtl7.Value = "Unit:";
            _dtl7.isIncluded = false;
            _dtl7.Position_x = 0;
            _dtl7.Position_y = 0;
            _dtl7.Width = 100;
            _dtl7.Height = 15;
            _dtl7.Alignment = "Right";
            _dtl7.RotationPoint = "360";
            _dtl7.fontFamily = ("Open Sans");
            _dtl7.fontSize = 9;
            _dtl7.fStyle = "Regular";
            _dtl7.Format = "test";
            _dtl7.bAlignment = "test";
            _dtls.Add(_dtl7);
            #endregion

            #region Multirate Name
            BarcodeDTL _dtl8 = new BarcodeDTL();
            _dtl8.Name = "8";
            _dtl8.Text = "Multirate Name";
            _dtl8.Value = "Rate Name";
            _dtl8.isIncluded = false;
            _dtl8.Position_x = 0;
            _dtl8.Position_y = 0;
            _dtl8.Width = 100;
            _dtl8.Height = 15;
            _dtl8.Alignment = "Right";
            _dtl8.RotationPoint = "360";
            _dtl8.fontFamily = ("Open Sans");
            _dtl8.fontSize = 9;
            _dtl8.fStyle = "Regular";
            _dtl8.Format = "test";
            _dtl8.bAlignment = "test";
            _dtls.Add(_dtl8);
            #endregion

            #region Purchase Rate
            BarcodeDTL _dtl9 = new BarcodeDTL();
            _dtl9.Name = "9";
            _dtl9.Text = "Purchase Rate";
            _dtl9.Value = "P.Rate :";
            _dtl9.isIncluded = false;
            _dtl9.Position_x = 0;
            _dtl9.Position_y = 0;
            _dtl9.Width = 100;
            _dtl9.Height = 15;
            _dtl9.Alignment = "Right";
            _dtl9.RotationPoint = "360";
            _dtl9.fontFamily = ("Open Sans");
            _dtl9.fontSize = 9;
            _dtl9.fStyle = "Regular";
            _dtl9.Format = "test";
            _dtl9.bAlignment = "test";
            _dtls.Add(_dtl9);
            #endregion

            #region Sales Rate
            BarcodeDTL _dtl10 = new BarcodeDTL();
            _dtl10.Name = "10";
            _dtl10.Text = "Sales Rate";
            _dtl10.Value = "S.Rate :";
            _dtl10.isIncluded = false;
            _dtl10.Position_x = 0;
            _dtl10.Position_y = 0;
            _dtl10.Width = 100;
            _dtl10.Height = 15;
            _dtl10.Alignment = "Right";
            _dtl10.RotationPoint = "360";
            _dtl10.fontFamily = ("Open Sans");
            _dtl10.fontSize = 9;
            _dtl10.fStyle = "Regular";
            _dtl10.Format = "test";
            _dtl10.bAlignment = "test";
            _dtls.Add(_dtl10);
            #endregion

            #region Inclusive Rate
            BarcodeDTL _dtl11 = new BarcodeDTL();
            _dtl11.Name = "11";
            _dtl11.Text = "Inclusive Rate";
            _dtl11.Value = "Inc.Rate :";
            _dtl11.isIncluded = false;
            _dtl11.Position_x = 0;
            _dtl11.Position_y = 0;
            _dtl11.Width = 100;
            _dtl11.Height = 15;
            _dtl11.Alignment = "Right";
            _dtl11.RotationPoint = "360";
            _dtl11.fontFamily = ("Open Sans");
            _dtl11.fontSize = 9;
            _dtl11.fStyle = "Regular";
            _dtl11.Format = "test";
            _dtl11.bAlignment = "test";
            _dtls.Add(_dtl11);
            #endregion

            #region Unit Price
            BarcodeDTL _dtl12 = new BarcodeDTL();
            _dtl12.Name = "12";
            _dtl12.Text = "Unit Price";
            _dtl12.Value = "";
            _dtl12.isIncluded = false;
            _dtl12.Position_x = 0;
            _dtl12.Position_y = 0;
            _dtl12.Width = 100;
            _dtl12.Height = 15;
            _dtl12.Alignment = "Right";
            _dtl12.RotationPoint = "360";
            _dtl12.fontFamily = ("Open Sans");
            _dtl12.fontSize = 9;
            _dtl12.fStyle = "Regular";
            _dtl12.Format = "test";
            _dtl12.bAlignment = "test";
            _dtls.Add(_dtl12);
            #endregion

            #region MRP
            BarcodeDTL _dtl13 = new BarcodeDTL();
            _dtl13.Name = "13";
            _dtl13.Text = "MRP";
            _dtl13.Value = "MRP :";
            _dtl13.isIncluded = false;
            _dtl13.Position_x = 0;
            _dtl13.Position_y = 0;
            _dtl13.Width = 100;
            _dtl13.Height = 15;
            _dtl13.Alignment = "Right";
            _dtl13.RotationPoint = "360";
            _dtl13.fontFamily = ("Open Sans");
            _dtl13.fontSize = 9;
            _dtl13.fStyle = "Regular";
            _dtl13.Format = "test";
            _dtl13.bAlignment = "test";
            _dtls.Add(_dtl13);
            #endregion

            #region User Field 1
            BarcodeDTL _dtl14 = new BarcodeDTL();
            _dtl14.Name = "14";
            _dtl14.Text = "User Field 1";
            _dtl14.Value = "";
            _dtl14.isIncluded = false;
            _dtl14.Position_x = 0;
            _dtl14.Position_y = 0;
            _dtl14.Width = 100;
            _dtl14.Height = 15;
            _dtl14.Alignment = "Right";
            _dtl14.RotationPoint = "360";
            _dtl14.fontFamily = ("Open Sans");
            _dtl14.fontSize = 9;
            _dtl14.fStyle = "Regular";
            _dtl14.Format = "test";
            _dtl14.bAlignment = "test";
            _dtls.Add(_dtl14);
            #endregion

            #region User Field 2
            BarcodeDTL _dtl15 = new BarcodeDTL();
            _dtl15.Name = "15";
            _dtl15.Text = "User Field 2";
            _dtl15.Value = "";
            _dtl15.isIncluded = false;
            _dtl15.Position_x = 0;
            _dtl15.Position_y = 0;
            _dtl15.Width = 100;
            _dtl15.Height = 15;
            _dtl15.Alignment = "Right";
            _dtl15.RotationPoint = "360";
            _dtl15.fontFamily = ("Open Sans");
            _dtl15.fontSize = 9;
            _dtl15.fStyle = "Regular";
            _dtl15.Format = "test";
            _dtl15.bAlignment = "test";
            _dtls.Add(_dtl15);
            #endregion

            #region User Field 3
            BarcodeDTL _dtl16 = new BarcodeDTL();
            _dtl16.Name = "16";
            _dtl16.Text = "User Field-3";
            _dtl16.Value = "";
            _dtl16.isIncluded = false;
            _dtl16.Position_x = 0;
            _dtl16.Position_y = 0;
            _dtl16.Width = 100;
            _dtl16.Height = 15;
            _dtl16.Alignment = "Vertical";
            _dtl16.RotationPoint = "90";
            _dtl16.fontFamily = ("Open Sans");
            _dtl16.fontSize = 9;
            _dtl16.fStyle = "Regular";
            _dtl16.Format = "test";
            _dtl16.bAlignment = "test";
            _dtls.Add(_dtl16);
            #endregion

            #region User Field 4
            BarcodeDTL _dtl17 = new BarcodeDTL();
            _dtl17.Name = "17";
            _dtl17.Text = "User Field-4";
            _dtl17.Value = "";
            _dtl17.isIncluded = false;
            _dtl17.Position_x = 0;
            _dtl17.Position_y = 0;
            _dtl17.Width = 100;
            _dtl17.Height = 15;
            _dtl17.Alignment = "Right";
            _dtl17.RotationPoint = "360";
            _dtl17.fontFamily = ("Open Sans");
            _dtl17.fontSize = 9;
            _dtl17.fStyle = "Regular";
            _dtl17.Format = "test";
            _dtl17.bAlignment = "test";
            _dtls.Add(_dtl17);
            #endregion

            #region Price Code
            BarcodeDTL _dtl18 = new BarcodeDTL();
            _dtl18.Name = "18";
            _dtl18.Text = "Price Code";
            _dtl18.Value = "Price Code :";
            _dtl18.isIncluded = false;
            _dtl18.Position_x = 0;
            _dtl18.Position_y = 0;
            _dtl18.Width = 200;
            _dtl18.Height = 15;
            _dtl18.Alignment = "Right";
            _dtl18.RotationPoint = "360";
            _dtl18.fontFamily = ("Open Sans");
            _dtl18.fontSize = 9;
            _dtl18.fStyle = "Regular";
            _dtl18.Format = "test";
            _dtl18.bAlignment = "test";
            _dtls.Add(_dtl18);
            #endregion

            #region Barcode
            BarcodeDTL _dtl19 = new BarcodeDTL();
            _dtl19.Name = "19";
            _dtl19.Text = "Barcode";
            _dtl19.Value = "";
            _dtl19.isIncluded = false;
            _dtl19.Position_x = 0;
            _dtl19.Position_y = 0;
            _dtl19.Width = 100;
            _dtl19.Height = 15;
            _dtl19.Alignment = "Right";
            _dtl19.RotationPoint = "360";
            _dtl19.fontFamily = ("Open Sans");
            _dtl19.fontSize = 9;
            _dtl19.fStyle = "Regular";
            _dtl19.Format = "test";
            _dtl19.bAlignment = "test";
            _dtls.Add(_dtl19);
            #endregion

            #region Batch Barcode
            BarcodeDTL _dtl20 = new BarcodeDTL();
            _dtl20.Name = "20";
            _dtl20.Text = "Batch Barcode ";
            _dtl20.Value = "";
            _dtl20.isIncluded = false;
            _dtl20.Position_x = 0;
            _dtl20.Position_y = 0;
            _dtl20.Width = 100;
            _dtl20.Height = 15;
            _dtl20.Alignment = "Left";
            _dtl20.RotationPoint = "360";
            _dtl20.fontFamily = ("Open Sans");
            _dtl20.fontSize = 9;
            _dtl20.fStyle = "Regular";
            _dtl20.Format = "Code 128";
            _dtl20.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl20);
            #endregion

            #region Unit Barcode
            BarcodeDTL _dtl21 = new BarcodeDTL();
            _dtl21.Name = "21";
            _dtl21.Text = "Unit Barcode ";
            _dtl21.Value = "";
            _dtl21.isIncluded = false;
            _dtl21.Position_x = 0;
            _dtl21.Position_y = 0;
            _dtl21.Width = 100;
            _dtl21.Height = 15;
            _dtl21.Alignment = "Left";
            _dtl21.RotationPoint = "360";
            _dtl21.fontFamily = ("Open Sans");
            _dtl21.fontSize = 9;
            _dtl21.fStyle = "Regular";
            _dtl21.Format = "Code 128";
            _dtl21.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl21);
            #endregion

            #region Ledger Code
            BarcodeDTL _dtl22 = new BarcodeDTL();
            _dtl22.Name = "22";
            _dtl22.Text = "Ledger Code ";
            _dtl22.Value = "";
            _dtl22.isIncluded = false;
            _dtl22.Position_x = 0;
            _dtl22.Position_y = 0;
            _dtl22.Width = 100;
            _dtl22.Height = 15;
            _dtl22.Alignment = "Left";
            _dtl22.RotationPoint = "360";
            _dtl22.fontFamily = ("Open Sans");
            _dtl22.fontSize = 9;
            _dtl22.fStyle = "Regular";
            _dtl22.Format = "Code 128";
            _dtl22.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl22);
            #endregion

            #region Manufacturing Date
            BarcodeDTL _dtl23 = new BarcodeDTL();
            _dtl23.Name = "23";
            _dtl23.Text = "Mfg.Date";
            _dtl23.Value = "Mfg.Date :";
            _dtl23.isIncluded = false;
            _dtl23.Position_x = 0;
            _dtl23.Position_y = 0;
            _dtl23.Width = 100;
            _dtl23.Height = 15;
            _dtl23.Alignment = "Left";
            _dtl23.RotationPoint = "360";
            _dtl23.fontFamily = ("Open Sans");
            _dtl23.fontSize = 9;
            _dtl23.fStyle = "Regular";
            _dtl23.Format = "Code 128";
            _dtl23.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl23);
            #endregion

            #region Expiry Date
            BarcodeDTL _dtl24 = new BarcodeDTL();
            _dtl24.Name = "24";
            _dtl24.Text = "Expiry Date";
            _dtl24.Value = "Exp.Date :";
            _dtl24.isIncluded = false;
            _dtl24.Position_x = 0;
            _dtl24.Position_y = 0;
            _dtl24.Width = 110;
            _dtl24.Height = 15;
            _dtl24.Alignment = "Left";
            _dtl24.RotationPoint = "360";
            _dtl24.fontFamily = ("Open Sans");
            _dtl24.fontSize = 9;
            _dtl24.fStyle = "Regular";
            _dtl24.Format = "Code 128";
            _dtl24.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl24);
            #endregion

            #region Item Note
            BarcodeDTL _dtl25 = new BarcodeDTL();
            _dtl25.Name = "25";
            _dtl25.Text = "Item Note";
            _dtl25.Value = "";
            _dtl25.isIncluded = false;
            _dtl25.Position_x = 0;
            _dtl25.Position_y = 0;
            _dtl25.Width = 100;
            _dtl25.Height = 15;
            _dtl25.Alignment = "Left";
            _dtl25.RotationPoint = "360";
            _dtl25.fontFamily = ("Open Sans");
            _dtl25.fontSize = 9;
            _dtl25.fStyle = "Regular";
            _dtl25.Format = "Code 128";
            _dtl25.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl25);
            #endregion

            #region User Type 1
            BarcodeDTL _dtl26 = new BarcodeDTL();
            _dtl26.Name = "26";
            _dtl26.Text = "User Type 1";
            _dtl26.Value = "";
            _dtl26.isIncluded = false;
            _dtl26.Position_x = 0;
            _dtl26.Position_y = 0;
            _dtl26.Width = 100;
            _dtl26.Height = 15;
            _dtl26.Alignment = "Left";
            _dtl26.RotationPoint = "360";
            _dtl26.fontFamily = ("Open Sans");
            _dtl26.fontSize = 9;
            _dtl26.fStyle = "Regular";
            _dtl26.Format = "Code 128";
            _dtl26.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl26);
            #endregion

            #region User Type 2
            BarcodeDTL _dtl27 = new BarcodeDTL();
            _dtl27.Name = "27";
            _dtl27.Text = "User Type 2";
            _dtl27.Value = "";
            _dtl27.isIncluded = false;
            _dtl27.Position_x = 0;
            _dtl27.Position_y = 0;
            _dtl27.Width = 100;
            _dtl27.Height = 15;
            _dtl27.Alignment = "Left";
            _dtl27.RotationPoint = "360";
            _dtl27.fontFamily = ("Open Sans");
            _dtl27.fontSize = 9;
            _dtl27.fStyle = "Regular";
            _dtl27.Format = "Code 128";
            _dtl27.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl27);
            #endregion

            #region User Type 3
            BarcodeDTL _dtl28 = new BarcodeDTL();
            _dtl28.Name = "28";
            _dtl28.Text = "User Type 3";
            _dtl28.Value = "";
            _dtl28.isIncluded = false;
            _dtl28.Position_x = 0;
            _dtl28.Position_y = 0;
            _dtl28.Width = 100;
            _dtl28.Height = 15;
            _dtl28.Alignment = "Left";
            _dtl28.RotationPoint = "360";
            _dtl28.fontFamily = ("Open Sans");
            _dtl28.fontSize = 9;
            _dtl28.fStyle = "Regular";
            _dtl28.Format = "Code 128";
            _dtl28.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl28);
            #endregion

            #region User Type 4
            BarcodeDTL _dtl29 = new BarcodeDTL();
            _dtl29.Name = "29";
            _dtl29.Text = "User Type 4";
            _dtl29.Value = "";
            _dtl29.isIncluded = false;
            _dtl29.Position_x = 0;
            _dtl29.Position_y = 0;
            _dtl29.Width = 100;
            _dtl29.Height = 15;
            _dtl29.Alignment = "Left";
            _dtl29.RotationPoint = "360";
            _dtl29.fontFamily = ("Open Sans");
            _dtl29.fontSize = 9;
            _dtl29.fStyle = "Regular";
            _dtl29.Format = "Code 128";
            _dtl29.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl29);
            #endregion

            #region Custom String 1
            BarcodeDTL _dtl30 = new BarcodeDTL();
            _dtl30.Name = "30";
            _dtl30.Text = "Custom String 1";
            _dtl30.Value = "Custom String 1";
            _dtl30.isIncluded = false;
            _dtl30.Position_x = 0;
            _dtl30.Position_y = 0;
            _dtl30.Width = 100;
            _dtl30.Height = 15;
            _dtl30.Alignment = "Left";
            _dtl30.RotationPoint = "360";
            _dtl30.fontFamily = ("Open Sans");
            _dtl30.fontSize = 9;
            _dtl30.fStyle = "Regular";
            _dtl30.Format = "Code 128";
            _dtl30.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl30);
            #endregion

            #region Custom String 2
            BarcodeDTL _dtl31 = new BarcodeDTL();
            _dtl31.Name = "31";
            _dtl31.Text = "Custom String 2";
            _dtl31.Value = "Custom String 2";
            _dtl31.isIncluded = false;
            _dtl31.Position_x = 0;
            _dtl31.Position_y = 0;
            _dtl31.Width = 100;
            _dtl31.Height = 15;
            _dtl31.Alignment = "Left";
            _dtl31.RotationPoint = "360";
            _dtl31.fontFamily = ("Open Sans");
            _dtl31.fontSize = 9;
            _dtl31.fStyle = "Regular";
            _dtl31.Format = "Code 128";
            _dtl31.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl31);
            #endregion

            #region Custom String 3
            BarcodeDTL _dtl32 = new BarcodeDTL();
            _dtl32.Name = "32";
            _dtl32.Text = "Custom String 3";
            _dtl32.Value = "Custom String 3";
            _dtl32.isIncluded = false;
            _dtl32.Position_x = 0;
            _dtl32.Position_y = 0;
            _dtl32.Width = 100;
            _dtl32.Height = 15;
            _dtl32.Alignment = "Left";
            _dtl32.RotationPoint = "360";
            _dtl32.fontFamily = ("Open Sans");
            _dtl32.fontSize = 9;
            _dtl32.fStyle = "Regular";
            _dtl32.Format = "Code 128";
            _dtl32.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl32);
            #endregion

            #region Custom String 4
            BarcodeDTL _dtl33 = new BarcodeDTL();
            _dtl33.Name = "33";
            _dtl33.Text = "Custom String 4";
            _dtl33.Value = "Custom String 4";
            _dtl33.isIncluded = false;
            _dtl33.Position_x = 0;
            _dtl33.Position_y = 0;
            _dtl33.Width = 100;
            _dtl33.Height = 15;
            _dtl33.Alignment = "Left";
            _dtl33.RotationPoint = "360";
            _dtl33.fontFamily = ("Open Sans");
            _dtl33.fontSize = 9;
            _dtl33.fStyle = "Regular";
            _dtl33.Format = "Code 128";
            _dtl33.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl33);
            #endregion

            #region Logo
            BarcodeDTL _dtl34 = new BarcodeDTL();
            _dtl34.Name = "34";
            _dtl34.Text = "Logo";
            _dtl34.Value = "";
            _dtl34.isIncluded = false;
            _dtl34.Position_x = 0;
            _dtl34.Position_y = 0;
            _dtl34.Width = 40;
            _dtl34.Height = 40;
            _dtl34.Alignment = "Left";
            _dtl34.RotationPoint = "360";
            _dtl34.fontFamily = ("Open Sans");
            _dtl34.fontSize = 9;
            _dtl34.fStyle = "Regular";
            _dtl34.Format = "Code 128";
            _dtl34.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl34);
            #endregion

            #region Barcode Image
            BarcodeDTL _dtl35 = new BarcodeDTL();
            _dtl35.Name = "35";
            _dtl35.Text = "Barcode_Image";
            _dtl35.Value = "";
            _dtl35.isIncluded = true;
            _dtl35.Position_x = 0;
            _dtl35.Position_y = 18;
            _dtl35.Width = 140;
            _dtl35.Height = 70;
            _dtl35.Alignment = "Left";
            _dtl35.RotationPoint = "360";
            _dtl35.fontFamily = ("Open Sans");
            _dtl35.fontSize = 9;
            _dtl35.fStyle = "Regular";
            _dtl35.Format = "Code 128";
            _dtl35.bAlignment = "Rotate180FlipXY";
            _dtls.Add(_dtl35);
            #endregion
        }
        private void AddPrintDataList()
        {
            barcodeList = new List<BarcodeData>();
            barcodeList.Clear();
            proBarcode = new BarcodeData();
            proBarcode.Sl_No = 1;
            proBarcode.Company_Name = "Company Name";
            proBarcode.Product_Code ="1001";
            proBarcode.Product_Name = "Product-1";
            proBarcode.Product_Description = "Description";
            proBarcode.Group_Name = "Group";
            proBarcode.Batch = "Batch";
            proBarcode.Unit = "Unit";
            proBarcode.MultiRate_Name = "Multirate";
            proBarcode.Purchase_Rate = 800;
            proBarcode.Sales_Rate = 1000;
            proBarcode.Inclusive_Rate = 1000;
            proBarcode.Unit_Price = 1000;
            proBarcode.MRP = 1100;
            proBarcode.UserField_1 = "U Field-1";
            proBarcode.UserField_2 = "U Field-2";
            proBarcode.UserField_3 = "U Field-3";
            proBarcode.UserField_4 = "U Field-4";
            proBarcode.Price_Code = "PC-1234";
            proBarcode.Barcode = "POS1234548";
            proBarcode.Batch_Barecode = "BB-1234";
            proBarcode.Unit_Barcode = "UB-1234";
            proBarcode.Ledger_Code = "LC-1234";
            proBarcode.Mfg_Date = "01-01-2019";
            proBarcode.Exparing_Date = "01-01-2019";
            proBarcode.item_Note = "Item Note";
            proBarcode.User_Type_1 = "UT-1";
            proBarcode.User_Type_2 = "UT-2";
            proBarcode.User_Type_3 = "UT-3";
            proBarcode.User_Type_4 = "UT-4";
            if (rdbSingle.Checked)
            {
                proBarcode.Quantity = 6.32M;
            }
            else
            {
                proBarcode.Quantity = 35;
            }
            barcodeList.Add(proBarcode);
            BarcodeData proBarcode1 = new BarcodeData();
            proBarcode1.Sl_No = 2;
            proBarcode1.Company_Name = "1002";
            proBarcode1.Product_Code = null;
            proBarcode1.Product_Name = "Product-2";
            proBarcode1.Product_Description = "Description";
            proBarcode1.Group_Name = "Group";
            proBarcode1.Batch = "Batch";
            proBarcode1.Unit = "Unit";
            proBarcode1.MultiRate_Name = "Multirate";
            proBarcode1.Purchase_Rate = 800;
            proBarcode1.Sales_Rate = 1000;
            proBarcode1.Inclusive_Rate = 1000;
            proBarcode1.Unit_Price = 1000;
            proBarcode1.MRP = 1100;
            proBarcode1.UserField_1 = "U Field-1";
            proBarcode1.UserField_2 = "U Field-2";
            proBarcode1.UserField_3 = "U Field-3";
            proBarcode1.UserField_4 = "U Field-4";
            proBarcode1.Price_Code = "PB-123456";
            proBarcode1.Barcode = "123456789";
            proBarcode1.Batch_Barecode = "BB-123456";
            proBarcode1.Unit_Barcode = "UB-123456";
            proBarcode1.Ledger_Code = "LC-123456";
            proBarcode1.Mfg_Date = "01-01-2019";
            proBarcode1.Exparing_Date = "01-01-2019";
            proBarcode1.item_Note = "Item-Note";
            proBarcode1.User_Type_1 = "UT-1";
            proBarcode1.User_Type_2 = "UT-2";
            proBarcode1.User_Type_3 = "UT-3";
            proBarcode1.User_Type_4 = "UT-4";
            if (rdbSingle.Checked)
            {
                proBarcode1.Quantity = 6;
            }
            else
            {
                proBarcode1.Quantity = 35;
            }
            barcodeList.Add(proBarcode1);
            BarcodeData proBarcode2 = new BarcodeData();
            proBarcode2.Sl_No = 3;
            proBarcode2.Company_Name = "";
            proBarcode2.Product_Code = "1003";
            proBarcode2.Product_Name = "Product-3";
            proBarcode2.Product_Description = "Description";
            proBarcode2.Group_Name = "Group";
            proBarcode2.Batch = "Batch";
            proBarcode2.Unit = "Unit";
            proBarcode2.MultiRate_Name = "Multirate";
            proBarcode2.Purchase_Rate = 800;
            proBarcode2.Sales_Rate = 1000;
            proBarcode2.Inclusive_Rate = 1000;
            proBarcode2.Unit_Price = 1000;
            proBarcode2.MRP = 1100;
            proBarcode2.UserField_1 = "U Field-1";
            proBarcode2.UserField_2 = "U Field-2";
            proBarcode2.UserField_3 = "U Field-3";
            proBarcode2.UserField_4 = "U Field-4";
            proBarcode2.Price_Code = "PC-123456";
            proBarcode2.Barcode = "223456789";
            proBarcode2.Batch_Barecode = "BB-123456";
            proBarcode2.Unit_Barcode = "UB-123456";
            proBarcode2.Ledger_Code = "LC-123456";
            proBarcode2.Mfg_Date = "01-01-2019";
            proBarcode2.Exparing_Date = "01-01-2019";
            proBarcode2.item_Note = "Item Note";
            proBarcode2.User_Type_1 = "UT-1";
            proBarcode2.User_Type_2 = "UT-2";
            proBarcode2.User_Type_3 = "UT-3";
            proBarcode2.User_Type_4 = "UT-4";
            if (rdbSingle.Checked)
            {
                proBarcode2.Quantity = 6;
            }
            else
            {
                proBarcode2.Quantity = 35;
            }
            barcodeList.Add(proBarcode2);
        }
        private string MsrConverting(string strTBText, double dmlMSRValue, bool isMultiple)
        {
            if (rdbPx.Checked)
            {
                if (isMultiple)
                {
                    return (Math.Truncate(Convert.ToDouble(strTBText) * dmlMSRValue)).ToString();
                }
                else
                {
                    return (Math.Truncate(Convert.ToDouble(strTBText) / dmlMSRValue)).ToString();
                }
            }
            else
            {
                if (isMultiple)
                {
                    return (Convert.ToDouble(strTBText) * dmlMSRValue).ToString("N" + "2");
                }
                else
                {
                    return (Convert.ToDouble(strTBText) / dmlMSRValue).ToString("N" + 2);
                }

            }
        }
        private void WidthAndHightFindMethod()
        {
            try
            {
                double dblAcross = 0;
                double dblDown = 0;
                if (txtLabelWth.Text != string.Empty && txtLabelHt.Text != string.Empty && txtPSHt.Text != string.Empty && txtPSWdth.Text != string.Empty && txtBottom.Text != string.Empty && txtRight.Text != string.Empty)
                {
                    PaperHieght = Convert.ToDouble(txtPSHt.Text) - Convert.ToDouble(txtTop.Text);
                    PaperWidth = Convert.ToDouble(txtPSWdth.Text) - Convert.ToDouble(txtLeft.Text);
                    lh = Convert.ToDouble(txtLabelHt.Text) + Convert.ToDouble(txtBottom.Text);
                    lw = Convert.ToDouble(txtLabelWth.Text) + Convert.ToDouble(txtRight.Text);
                    intAcross = 0;
                    intDown = 0;
                    intTtlnoPerPage = 0;
                    if (PaperWidth > 0 && lw > 0)
                    {
                        dblAcross = PaperWidth / lw;
                    }
                    if (PaperHieght > 0 && lh > 0)
                    {
                        dblDown = PaperHieght / lh;
                    }
                    intAcross = Convert.ToInt32(Math.Truncate(dblAcross));
                    intDown = Convert.ToInt32(Math.Truncate(dblDown));
                    intWidth = lw * intAcross;
                    intTtlnoPerPage = intAcross * intDown;
                    lblPrintPerPage.Text = intTtlnoPerPage.ToString();
                    txtAcross.Text = intAcross.ToString();
                    txtDown.Text = intDown.ToString();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
        }
        void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
        }
        #endregion

        #region Form Event
        private void pbBarcodeLabel_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClickedOnBCode = true;
        }
        private void pbBarcodeLabel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseClickedOnBCode = false;
        }
        private void pbBarcodeLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseClickedOnBCode)
            {
                pnlMargin.Height = pbBarcodeLabel.Top + e.Y;
                pnlMargin.Width = pbBarcodeLabel.Left + e.X;
                if (intMsr == 0)
                {
                    txtLabelHt.Text = pnlLabel.Height.ToString();
                    txtLabelWth.Text = pnlLabel.Width.ToString();
                }
                else if (intMsr == 1)
                {
                    txtLabelHt.Text = (pnlLabel.Height/39.3701).ToString("F");
                    txtLabelWth.Text = (pnlLabel.Width/39.3701).ToString("F");
                }
                else
                {
                    txtLabelHt.Text = (pnlLabel.Height/100).ToString("F");
                    txtLabelWth.Text = (pnlLabel.Width/100).ToString("F");
                }
            }
        }
        private void Barcode_Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseClickedOnBLabel)
            {
                Barcode_Panel.Left += e.X - PanelMouseDownLocation.X;
                Barcode_Panel.Top += e.Y - PanelMouseDownLocation.Y;
                if (intMsr == 0)
                {
                    txtBarcodeX.Text = Barcode_Panel.Location.X.ToString();
                    txtBarcodeY.Text = Barcode_Panel.Location.Y.ToString();
                }
                else if (intMsr == 1)
                {
                    txtBarcodeX.Text = (Barcode_Panel.Location.X/39.3701).ToString("F");
                    txtBarcodeY.Text = (Barcode_Panel.Location.Y/39.3701).ToString();
                }
                else
                {
                    txtBarcodeX.Text = (Barcode_Panel.Location.X/100).ToString("F");
                    txtBarcodeY.Text = (Barcode_Panel.Location.Y/100).ToString("F");
                }
            }
        }
        private void Barcode_Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (strControl != string.Empty)
            {
                EditMethod();
            }
            mouseClickedOnBLabel = true;
            lblControlPrefix.Text = "Value :";
            cmbBarcodeValue.Visible = true;
            txtPrefixVal.Visible = false;
            strControl = "35";
            strCntrlName = "Barcode";
            txtControl.Text = strCntrlName;
            txtPrefixVal.Text = string.Empty;
            if (e.Button == MouseButtons.Left) PanelMouseDownLocation = e.Location;
            if (intMsr == 0)
            {
                txtWidth.Text = Barcode_Panel.Width.ToString();
                txtHeight.Text = Barcode_Panel.Height.ToString();
                txtBarcodeX.Text = Barcode_Panel.Location.X.ToString();
                txtBarcodeY.Text = Barcode_Panel.Location.Y.ToString();
            }
            else if (intMsr == 1)
            {
                txtWidth.Text = (Barcode_Panel.Width/39.3701).ToString("F");
                txtHeight.Text = (Barcode_Panel.Height/39.3701).ToString("F");
                txtBarcodeX.Text = (Barcode_Panel.Location.X/39.3701).ToString("F");
                txtBarcodeY.Text = (Barcode_Panel.Location.Y/39.3701).ToString("F");
            }
            else
            {
                txtWidth.Text = (Barcode_Panel.Width/100).ToString("F");
                txtHeight.Text = (Barcode_Panel.Height/100).ToString("F");
                txtBarcodeX.Text = (Barcode_Panel.Location.X/100).ToString("F");
                txtBarcodeY.Text = (Barcode_Panel.Location.Y/100).ToString("F");
            }
        }
        private void pbBarcode_MouseDown(object sender, MouseEventArgs e)
        {
            mouseClickedOnPbBarcode = true;
            strControl = "35";
            strCntrlName = "Barcode";
            txtControl.Text = strCntrlName;
        }
        private void pbBarcode_MouseUp(object sender, MouseEventArgs e)
        {
            mouseClickedOnPbBarcode = false;
        }
        private void pbBarcode_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (mouseClickedOnPbBarcode)
                {
                    Barcode_Panel.Height = pbBarcode.Top + e.Y;
                    Barcode_Panel.Width = pbBarcode.Left + e.X;
                    if (intMsr == 0)
                    {
                        txtHeight.Text = Barcode_Panel.Height.ToString();
                        txtWidth.Text = Barcode_Panel.Width.ToString();
                    }
                    else if (intMsr == 1)
                    {
                        txtHeight.Text = (Barcode_Panel.Height / 39.3701).ToString("F");
                        txtWidth.Text = (Barcode_Panel.Width / 39.3701).ToString("F");
                    }
                    else
                    {
                        txtHeight.Text = (Barcode_Panel.Height / 100).ToString("F");
                        txtWidth.Text = (Barcode_Panel.Width / 100).ToString("F");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Barcode_Panel_MouseUp(object sender, MouseEventArgs e)
        {
            mouseClickedOnBLabel = false;
        }
        private void txtWidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (strControl == "35" && mouseClickedOnBLabel == false)
                {
                    if (txtHeight.Text != string.Empty && txtHeight.Text != "0" && txtWidth.Text != string.Empty && txtWidth.Text != "0" &&  mouseClickedOnPbBarcode == false)
                    {
                        if (rdbPx.Checked)
                        {
                            Barcode_Panel.Height = Convert.ToInt32(txtHeight.Text);
                            Barcode_Panel.Width = Convert.ToInt32(txtWidth.Text);
                        }
                        else if (rdbCM.Checked)
                        {
                            Barcode_Panel.Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 39.3701);
                            Barcode_Panel.Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 39.3701);
                        }
                        else
                        {
                            Barcode_Panel.Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 100);
                            Barcode_Panel.Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 100);
                        }
                    }
                }
                else if (strControl == "34")
                {

                    if (txtHeight.Text != string.Empty && txtHeight.Text != "0" && txtWidth.Text != string.Empty && txtWidth.Text != "0" && mouseClickedOnPbBarcode == false)
                    {
                        if (rdbPx.Checked)
                        {
                            pbLogo.Height = Convert.ToInt32(txtHeight.Text);
                            pbLogo.Width = Convert.ToInt32(txtWidth.Text);
                        }
                        else if (rdbCM.Checked)
                        {
                            pbLogo.Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 39.3701);
                            pbLogo.Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 39.3701);
                        }
                        else
                        {
                            pbLogo.Height = Convert.ToInt32(Convert.ToDouble(txtHeight.Text) * 100);
                            pbLogo.Width = Convert.ToInt32(Convert.ToDouble(txtWidth.Text) * 100);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void txtBarcodeX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(strControl!=string.Empty)
                {
                    if (txtBarcodeX.Text != string.Empty && txtBarcodeY.Text != string.Empty)
                    {
                        if (strControl == "35")
                        {
                            if (intMsr == 0)
                            {
                                Barcode_Panel.Location = new Point(Convert.ToInt32(txtBarcodeX.Text), Convert.ToInt32(txtBarcodeY.Text));
                            }
                            else if (intMsr == 1)
                            {
                                Barcode_Panel.Location = new Point(Convert.ToInt32(Convert.ToDouble(txtBarcodeX.Text) * 39.3701), Convert.ToInt32(Convert.ToDouble(txtBarcodeY.Text) * 39.3701));
                            }
                            else
                            {
                                Barcode_Panel.Location = new Point(Convert.ToInt32(txtBarcodeX.Text) * 100, Convert.ToInt32(txtBarcodeY.Text) * 100);
                            }
                        }
                        else if (strControl == "34")
                        {
                            if (intMsr == 0)
                            {
                                pbLogo.Location = new Point(Convert.ToInt32(txtBarcodeX.Text), Convert.ToInt32(txtBarcodeY.Text));
                            }
                            else if (intMsr == 1)
                            {
                                pbLogo.Location = new Point(Convert.ToInt32(Convert.ToDouble(txtBarcodeX.Text) * 39.3701), Convert.ToInt32(Convert.ToDouble(txtBarcodeY.Text) * 39.3701));
                            }
                            else
                            {
                                pbLogo.Location = new Point(Convert.ToInt32(txtBarcodeX.Text) * 100, Convert.ToInt32(txtBarcodeY.Text) * 100);
                            }
                        }
                        else
                        {
                            if (intMsr == 0)
                            {
                                txtBox.Location = new Point(Convert.ToInt32(txtBarcodeX.Text), Convert.ToInt32(txtBarcodeY.Text));
                            }
                            else if (intMsr == 1)
                            {
                                txtBox.Location = new Point(Convert.ToInt32(Convert.ToDouble(txtBarcodeX.Text) * 39.3701), Convert.ToInt32(Convert.ToDouble(txtBarcodeY.Text) * 39.3701));
                            }
                            else
                            {
                                txtBox.Location = new Point(Convert.ToInt32(txtBarcodeX.Text) * 100, Convert.ToInt32(txtBarcodeY.Text) * 100);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void txtLabelWth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtLabelHt.Text != string.Empty && txtLabelWth.Text != string.Empty && mouseClickedOnBCode == false && isLblKeyPressed)
                {
                    if (intMsr == 0)
                    {
                        pnlMargin.Height = Convert.ToInt32(txtLabelHt.Text) + 30;
                        pnlMargin.Width = Convert.ToInt32(txtLabelWth.Text) + 30;
                    }
                    else if (intMsr == 1)
                    {
                        pnlMargin.Height = Convert.ToInt32(Convert.ToDouble(txtLabelHt.Text) * 39.3701) + 30;
                        pnlMargin.Width = Convert.ToInt32(Convert.ToDouble(txtLabelWth.Text) * 39.3701) + 30;
                    }
                    else if (rdbIn.Checked)
                    {
                        pnlMargin.Height = Convert.ToInt32(Convert.ToDouble(txtLabelHt.Text) * 100) + 30;
                        pnlMargin.Width = Convert.ToInt32(Convert.ToDouble(txtLabelWth.Text) * 100) + 30;
                    }
                    isLblKeyPressed = false;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                WidthAndHightFindMethod();
            }
        }
        private void BarcodeDesign_Load(object sender, EventArgs e)
        {
            ShowToolTip();
            AddPrintDataList();
            AddDetailListMethod();
            cbEncodeType.SelectedIndex = 18;
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                cmbPrinter.Items.Add(printer);
            }
            if (cmbPrinter.Items.Count > 0)
            {
                cmbPrinter.SelectedIndex = 0;
            }
            cmbBarcodeValue.SelectedIndex = 1;
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = cmbPrinter.Text;
            foreach (PaperSize psize in ps.PaperSizes)
            {
                cmbPaperSize.Items.Add(psize.PaperName);
            }
            if (cmbPaperSize.Items.Contains("A4"))
            {
                cmbPaperSize.Text = "A4";
                txtPSWdth.Text = "827";
                txtPSHt.Text = "1169";
            }
            btnClear_Click(null, null);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbParameter.Text != string.Empty)
            {
                foreach (Control item in pnlLabel.Controls.OfType<Label>())
                {
                    item.ForeColor = Color.Black;
                }
                if (strControl != string.Empty)
                {
                    EditMethod();
                    cmbAlignment.SelectedIndex = 0;
                }
                strControl = cmbParameter.SelectedIndex.ToString();
                strCntrlName = cmbParameter.Text;
                txtControl.Text = strCntrlName;
                if (!itemName.Contains(strControl))
                {
                    isIncluded = true;
                    itemName.Add(strControl);
                    if (strControl == "34")
                    {
                        btnLogo.Visible = true;
                        btnFont.Visible = false;
                        lblControlPrefix.Text = "File Path";
                        AddMethod(cmbParameter.Text, strControl, string.Empty, 100, 2, 40, 40);
                    }
                    else
                    {
                        btnFont.Visible = true;
                        btnLogo.Visible = false;
                        lblControlPrefix.Text = "Prefix Value";
                        AddMethod(cmbParameter.Text, strControl, string.Empty, 1, 1, 15, 100);
                    }
                    if (strControl != "34")
                    {
                        txtPrefixVal.Text = _dtls[cmbParameter.SelectedIndex].Value;
                        txtBox.ForeColor = Color.Crimson;
                    }
                }
            }
        }
        void txtBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseClickedOnCstmTBox)
            {
                txtBox.Left += e.X - PanelMouseDownLocation.X;
                txtBox.Top += e.Y - PanelMouseDownLocation.Y;
                if (intMsr == 0)
                {
                    txtBarcodeX.Text = txtBox.Location.X.ToString();
                    txtBarcodeY.Text = txtBox.Location.Y.ToString();
                }
                else if (intMsr == 1)
                {
                    txtBarcodeX.Text = (txtBox.Location.X/39.3701).ToString("F");
                    txtBarcodeY.Text = (txtBox.Location.Y/39.3701).ToString("F");
                }
                else
                {
                    txtBarcodeX.Text = (txtBox.Location.X/100).ToString("F");
                    txtBarcodeY.Text = (txtBox.Location.Y/100).ToString("F");
                }
            }
        }
        void txtBox_MouseUp(object sender, MouseEventArgs e)
        {
            mouseClickedOnCstmTBox = false;
        }
        void txtBox_MouseDown(object sender, MouseEventArgs e)
        {
            btnFont.Visible = true;
            btnLogo.Visible = false;
            lblControlPrefix.Text = "Prefix Value";
            foreach (Control item in pnlLabel.Controls.OfType<Label>())
            {
                item.ForeColor = Color.Black;
            }
            if (strControl != string.Empty)
            {
                EditMethod();
            }
            txtBox = (RotatingLabel)sender;
            txtBox.ForeColor = Color.Crimson;
            strControl = txtBox.Name;
            strCntrlName = txtBox.NewText;
            txtControl.Text = strCntrlName;
            lblControlPrefix.Text = "Prefix Value";
            cmbBarcodeValue.Visible = false;
            txtPrefixVal.Visible = true;
            mouseClickedOnCstmTBox = true;
            if (e.Button == MouseButtons.Left) PanelMouseDownLocation = e.Location;
            txtPrefixVal.Text = _dtls[Convert.ToInt32(strControl)].Value;
            if (intMsr == 0)
            {
                txtBarcodeX.Text = _dtls[Convert.ToInt32(strControl)].Position_x.ToString();
                txtBarcodeY.Text = _dtls[Convert.ToInt32(strControl)].Position_y.ToString();
                txtHeight.Text = _dtls[Convert.ToInt32(strControl)].Height.ToString();
                txtWidth.Text = _dtls[Convert.ToInt32(strControl)].Width.ToString();
            }
            else if (intMsr == 1)
            {
                txtBarcodeX.Text = (_dtls[Convert.ToInt32(strControl)].Position_x / 39.3701).ToString("F");
                txtBarcodeY.Text = (_dtls[Convert.ToInt32(strControl)].Position_y / 39.3701).ToString("F");
                txtHeight.Text = (_dtls[Convert.ToInt32(strControl)].Height / 39.3701).ToString("F");
                txtWidth.Text = (_dtls[Convert.ToInt32(strControl)].Width / 39.3701).ToString("F");
            }
            else
            {
                txtBarcodeX.Text = (_dtls[Convert.ToInt32(strControl)].Position_x * 100).ToString("F");
                txtBarcodeY.Text = (_dtls[Convert.ToInt32(strControl)].Position_y * 100).ToString("F");
                txtHeight.Text = (_dtls[Convert.ToInt32(strControl)].Height * 100).ToString("F");
                txtWidth.Text = (_dtls[Convert.ToInt32(strControl)].Width * 100).ToString("F");
            }
            if (_dtls[Convert.ToInt32(strControl)].fStyle == "Regular")
            {
                fStyle = FontStyle.Regular;
            }
            else if (_dtls[Convert.ToInt32(strControl)].fStyle == "Bold")
            {
                fStyle = FontStyle.Bold;
            }
            else
            {
                fStyle = FontStyle.Italic;
            }
            font = new Font(_dtls[Convert.ToInt32(strControl)].fontFamily, _dtls[Convert.ToInt32(strControl)].fontSize, fStyle);
            if (_dtls[Convert.ToInt32(strControl)].Alignment == "Left")
            {
                cmbAlignment.SelectedIndex = 0;
            }
            else if (_dtls[Convert.ToInt32(strControl)].Alignment == "Center")
            {
                cmbAlignment.SelectedIndex = 1;
            }
            else if (_dtls[Convert.ToInt32(strControl)].Alignment == "Right")
            {
                cmbAlignment.SelectedIndex = 2;
            }
            else
            {
                cmbAlignment.SelectedIndex = 3;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (strControl != string.Empty)
            {
                isIncluded = false;
                if (strControl != "34")
                {
                    foreach (Control item in pnlLabel.Controls.OfType<Label>())
                    {
                        if (item.Name == strControl)
                            pnlLabel.Controls.Remove(item);
                        itemName.Remove(strControl);
                    }
                }
                else
                {
                    foreach (Control item in pnlLabel.Controls.OfType<PictureBox>())
                    {
                        if (item.Name == strControl)
                            pnlLabel.Controls.Remove(item);
                        itemName.Remove(strControl);
                    }
                }
                EditMethod();
                isIncluded = true;
            }
        }
        private void btnEncode_Click(object sender, EventArgs e)
        {
            btnLogo.Visible = false;
            btnFont.Visible = true;
            try
            {
                if (ValidationMwthod() == false)
                {
                    return;
                }
                EditMethod();
                BarcodeHeaderMethod();
                pBarcode = new ProductBarcode();
                grpBarcode.BackgroundImage = pBarcode.getBarcodeImageTwo(barcodeList, _dtls, 0, bhdr.Width, bhdr.Height, Color.Black, Color.White, bhdr.BarcodeField);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            btnLogo.Visible = false;
            btnFont.Visible = true;
            if (ValidationMwthod() == false)
            {
                return;
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            saveFileDialog1.Title = "Save Barcode Settings";
            saveFileDialog1.DefaultExt = "xml";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = saveFileDialog1.FileName;
            }
            if (txtFilePath.Text != string.Empty)
            {
                BarcodeHeaderMethod();
                BarcodeFormatClass _objFormat = new BarcodeFormatClass();
                _objFormat._barcodeHDR = bhdr;
                _objFormat._barcodeDTLs = _dtls;
                string sPatch = txtFilePath.Text;
                if (File.Exists(sPatch))
                {
                    File.Delete(sPatch);
                }
                XmlSerializer serializer = new XmlSerializer(typeof(BarcodeFormatClass));
                using (TextWriter writer = new StreamWriter(sPatch))
                {
                    serializer.Serialize(writer, _objFormat);
                    writer.Close();
                }
                MessageBox.Show("Saved Successfully");
            }
        }
        private void btnFont_Click(object sender, EventArgs e)
        {
            if (txtBox != null)
            {
                fontDialog1 = new FontDialog();
                fontDialog1.Font = txtBox.Font;
                if (fontDialog1.ShowDialog() == DialogResult.OK)
                {
                    font = fontDialog1.Font;
                    txtBox.Font = font;
                }
            }
        }
        private void txtPrefixVal_TextChanged(object sender, EventArgs e)
        {
            if (strControl == "1" || strControl == "30" || strControl == "31" || strControl == "32" || strControl == "33")
            {
                foreach (Control item in pnlLabel.Controls.OfType<RotatingLabel>())
                {
                    if (item.Name == strControl)
                        txtBox = (RotatingLabel)item;
                }
                txtBox.NewText = txtPrefixVal.Text;
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                BarcodeHeaderMethod();
                pBarcode = new ProductBarcode();
                pBarcode.printBarcode(barcodeList, _dtls, bhdr, Convert.ToInt32(txtStarting.Text));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtPSWdth_TextChanged(object sender, EventArgs e)
        {
            WidthAndHightFindMethod();
        }
        private void cmbPaperSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrinterSettings ps = new PrinterSettings();
            foreach (PaperSize psize in ps.PaperSizes)
            {
                if (cmbPaperSize.Text == psize.PaperName)
                {
                    if (rdbPx.Checked)
                    {
                        txtPSWdth.Text = psize.Width.ToString();
                        txtPSHt.Text = psize.Height.ToString();
                    }
                    else if (rdbCM.Checked)
                    {
                        txtPSWdth.Text = (psize.Width/39.3701).ToString("F");
                        txtPSHt.Text = (psize.Height/39.3701).ToString("F");
                    }
                    else
                    {
                        txtPSWdth.Text = (psize.Width / 100).ToString("F");
                        txtPSHt.Text = (psize.Height / 100).ToString("F");
                    }
                }
            }
        }
        private void txtDown_TextChanged(object sender, EventArgs e)
        {
            if (txtDown.Text != string.Empty && txtAcross.Text != string.Empty)
            {
                intAcross = Convert.ToInt32(txtAcross.Text);
                intDown = Convert.ToInt32(txtDown.Text);
                lblPrintPerPage.Text = (intAcross * intDown).ToString();
            }
        }
        private void btnPreview_Click(object sender, EventArgs e)
        {
            btnLogo.Visible = false;
            btnFont.Visible = true; 
            try
            {
                if (ValidationMwthod() == false)
                {
                    return;
                }
                BarcodeHeaderMethod();
                pBarcode = new ProductBarcode();
                pBarcode.printBarcodePreview(barcodeList, _dtls, bhdr, Convert.ToInt32(txtStarting.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cmbAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (strControl != string.Empty && strControl!="35")
            {
                double Ht = 100;
                if (cmbAlignment.SelectedIndex == 3)
                {
                    if (rdbPx.Checked)
                    {
                        Ht = 100;
                        txtHeight.Text = Ht.ToString();
                    }
                    else if (rdbCM.Checked)
                    {
                        Ht = 100 / 39.3701;
                        txtHeight.Text = Ht.ToString("F");
                    }
                    else
                    {
                        Ht = 1;
                        txtHeight.Text = Ht.ToString("F");
                    }
                    txtBox.AutoSize = false;      // adjust according to your text
                    txtBox.Text = "";
                    txtBox.RotateAngle = 90;
                    txtBox.Refresh();
                }
                else
                {
                    if (rdbPx.Checked)
                    {
                        Ht = 15;
                        txtHeight.Text = Ht.ToString();
                    }
                    else if (rdbCM.Checked)
                    {
                        Ht=15/39.3701;
                        txtHeight.Text = Ht.ToString("F");
                    }
                    else
                    {
                        Ht = 0.15;
                        txtHeight.Text = Ht.ToString("F");
                    }
                    txtBox.AutoSize = false;      // adjust according to your text
                    txtBox.Text = "";             //which can be changed by NewText property
                    txtBox.RotateAngle = 360;
                    txtBox.Refresh();
                }
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < 3; i++)
            {
                AddDetailListMethod();
                LoadControlsMwthod();
            }
            rdbPx.Checked = true;
            intMsr = 0;
            txtPrefixVal.Text = string.Empty;
            cmbAlignment.SelectedIndex = 0;
            txtControl.Text = string.Empty;
            txtLabelWth.Text = "250";
            txtLabelHt.Text = "150";
            grpBarcode.BackgroundImage = null;
            txtRight.Text = "10";
            txtBottom.Text = "10";
            txtLeft.Text = "0";
            txtTop.Text = "0";
            txtWidth.Text = "0";
            txtHeight.Text = "0";
            txtBarcodeX.Text = "0";
            txtBarcodeY.Text = "0";
            pnlMargin.Height = Convert.ToInt32(txtLabelHt.Text) + 30;
            pnlMargin.Width = Convert.ToInt32(txtLabelWth.Text) + 30;
            strControl = string.Empty;
            if (cmbPrinter.Items.Count > 0)
            {
                cmbPrinter.SelectedIndex = 0;
            }
            if (cmbPaperSize.Items.Contains("A4"))
            {
                cmbPaperSize.Text = "A4";
                txtPSWdth.Text = "827";
                txtPSHt.Text = "1169";
            }
            cmbParameter.SelectedIndex = -1;
            cbBorder.Checked = false;
            btnLogo.Visible = false;
            cmbParameter.Focus();
        }
        private void rdbSingle_CheckedChanged(object sender, EventArgs e)
        {
            AddPrintDataList();
        }
        private void rdbPx_Click(object sender, EventArgs e)
        {
            double dmlVal = 0;
            strControl = string.Empty;
            strCntrlName = string.Empty;
            txtControl.Text = strCntrlName;
            if (rdbPx.Checked)
            {
                if (intMsr == 1)
                {
                    dmlVal = 39.3701;
                    //from ccentimeter to pixel = cm/px X= 1*39.3701
                    txtLabelWth.Text = MsrConverting(txtLabelWth.Text, dmlVal, true);
                    txtLabelHt.Text = MsrConverting(txtLabelHt.Text, dmlVal, true);
                    txtHeight.Text = MsrConverting(txtHeight.Text, dmlVal, true);
                    txtWidth.Text = MsrConverting(txtWidth.Text, dmlVal, true);
                    txtBarcodeX.Text = MsrConverting(txtBarcodeX.Text, dmlVal, true);
                    txtBarcodeY.Text = MsrConverting(txtBarcodeY.Text, dmlVal, true);
                    txtLeft.Text = MsrConverting(txtLeft.Text, dmlVal, true);
                    txtRight.Text = MsrConverting(txtRight.Text, dmlVal, true);
                    txtTop.Text = MsrConverting(txtTop.Text, dmlVal, true);
                    txtBottom.Text = MsrConverting(txtBottom.Text, dmlVal, true);
                    txtPSWdth.Text = MsrConverting(txtPSWdth.Text, dmlVal, true);
                    txtPSHt.Text = MsrConverting(txtPSHt.Text, dmlVal, true);
                }
                else if (intMsr == 2)
                {
                    dmlVal = 100;
                    //from Inches to pixel = in/px X=1*100
                    txtLabelWth.Text = MsrConverting(txtLabelWth.Text, dmlVal, true);
                    txtLabelHt.Text = MsrConverting(txtLabelHt.Text, dmlVal, true);
                    txtHeight.Text = MsrConverting(txtHeight.Text, dmlVal, true);
                    txtWidth.Text = MsrConverting(txtWidth.Text, dmlVal, true);
                    txtBarcodeX.Text = MsrConverting(txtBarcodeX.Text, dmlVal, true);
                    txtBarcodeY.Text = MsrConverting(txtBarcodeY.Text, dmlVal, true);
                    txtLeft.Text = MsrConverting(txtLeft.Text, dmlVal, true);
                    txtRight.Text = MsrConverting(txtRight.Text, dmlVal, true);
                    txtTop.Text = MsrConverting(txtTop.Text, dmlVal, true);
                    txtBottom.Text = MsrConverting(txtBottom.Text, dmlVal, true);
                    txtPSWdth.Text = MsrConverting(txtPSWdth.Text, dmlVal, true);
                    txtPSHt.Text = MsrConverting(txtPSHt.Text, dmlVal, true);
                }
                else
                {
                    return;
                }
                intMsr = 0;
            }
            else if (rdbCM.Checked)
            {
                if (intMsr == 0)
                {
                    dmlVal = 39.3701;
                    //from pixel  to centimeter = px/cm X= 1/39.3701
                    txtLabelWth.Text = MsrConverting(txtLabelWth.Text, dmlVal, false);
                    txtLabelHt.Text = MsrConverting(txtLabelHt.Text, dmlVal, false);
                    txtHeight.Text = MsrConverting(txtHeight.Text, dmlVal, false);
                    txtWidth.Text = MsrConverting(txtWidth.Text, dmlVal, false);
                    txtBarcodeX.Text = MsrConverting(txtBarcodeX.Text, dmlVal, false);
                    txtBarcodeY.Text = MsrConverting(txtBarcodeY.Text, dmlVal, false);
                    txtLeft.Text = MsrConverting(txtLeft.Text, dmlVal, false);
                    txtRight.Text = MsrConverting(txtRight.Text, dmlVal, false);
                    txtTop.Text = MsrConverting(txtTop.Text, dmlVal, false);
                    txtBottom.Text = MsrConverting(txtBottom.Text, dmlVal, false);
                    txtPSWdth.Text = MsrConverting(txtPSWdth.Text, dmlVal, false);
                    txtPSHt.Text = MsrConverting(txtPSHt.Text, dmlVal, false);
                }
                else if (intMsr == 2)
                {
                    dmlVal = 2.54;
                    //from inches  to centimeter = in*cm 1= 2.54/1
                    txtLabelWth.Text = MsrConverting(txtLabelWth.Text, dmlVal, true);
                    txtLabelHt.Text = MsrConverting(txtLabelHt.Text, dmlVal, true);
                    txtHeight.Text = MsrConverting(txtHeight.Text, dmlVal, true);
                    txtWidth.Text = MsrConverting(txtWidth.Text, dmlVal, true);
                    txtBarcodeX.Text = MsrConverting(txtBarcodeX.Text, dmlVal, true);
                    txtBarcodeY.Text = MsrConverting(txtBarcodeY.Text, dmlVal, true);
                    txtLeft.Text = MsrConverting(txtLeft.Text, dmlVal, true);
                    txtRight.Text = MsrConverting(txtRight.Text, dmlVal, true);
                    txtTop.Text = MsrConverting(txtTop.Text, dmlVal, true);
                    txtBottom.Text = MsrConverting(txtBottom.Text, dmlVal, true);
                    txtPSWdth.Text = MsrConverting(txtPSWdth.Text, dmlVal, true);
                    txtPSHt.Text = MsrConverting(txtPSHt.Text, dmlVal, true);
                }
                else
                {
                    return;
                }
                intMsr = 1;
            }
            else
            {
                if (intMsr == 0)
                {
                    //from pixel  to Inches = px/in X= 1/100
                    dmlVal = 100;
                    txtLabelWth.Text = MsrConverting(txtLabelWth.Text, dmlVal, false);
                    txtLabelHt.Text = MsrConverting(txtLabelHt.Text, dmlVal, false);
                    txtHeight.Text = MsrConverting(txtHeight.Text, dmlVal, false);
                    txtWidth.Text = MsrConverting(txtWidth.Text, dmlVal, false);
                    txtBarcodeX.Text = MsrConverting(txtBarcodeX.Text, dmlVal, false);
                    txtBarcodeY.Text = MsrConverting(txtBarcodeY.Text, dmlVal, false);
                    txtLeft.Text = MsrConverting(txtLeft.Text, dmlVal, false);
                    txtRight.Text = MsrConverting(txtRight.Text, dmlVal, false);
                    txtTop.Text = MsrConverting(txtTop.Text, dmlVal, false);
                    txtBottom.Text = MsrConverting(txtBottom.Text, dmlVal, false);
                    txtPSWdth.Text = MsrConverting(txtPSWdth.Text, dmlVal, false);
                    txtPSHt.Text = MsrConverting(txtPSHt.Text, dmlVal, false);
                }
                else if (intMsr == 1)
                {
                    dmlVal = 2.54;
                    //from ccentimeter to Inches = cm/pin X= 1/2.541
                    txtLabelWth.Text = MsrConverting(txtLabelWth.Text, dmlVal, false);
                    txtLabelHt.Text = MsrConverting(txtLabelHt.Text, dmlVal, false);
                    txtHeight.Text = MsrConverting(txtHeight.Text, dmlVal, false);
                    txtWidth.Text = MsrConverting(txtWidth.Text, dmlVal, false);
                    txtBarcodeX.Text = MsrConverting(txtBarcodeX.Text, dmlVal, false);
                    txtBarcodeY.Text = MsrConverting(txtBarcodeY.Text, dmlVal, false);
                    txtLeft.Text = MsrConverting(txtLeft.Text, dmlVal, false);
                    txtRight.Text = MsrConverting(txtRight.Text, dmlVal, false);
                    txtTop.Text = MsrConverting(txtTop.Text, dmlVal, false);
                    txtBottom.Text = MsrConverting(txtBottom.Text, dmlVal, false);
                    txtPSWdth.Text = MsrConverting(txtPSWdth.Text, dmlVal, false);
                    txtPSHt.Text = MsrConverting(txtPSHt.Text, dmlVal, false);
                }
                else
                {
                    return;
                }
                intMsr = 2;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnClose_MouseEnter(object sender, EventArgs e)
        {
            this.btnClose.ForeColor = Color.White;
        }
        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            this.btnClose.ForeColor = Color.DarkGray;
        }
        private void btnExternal_Click(object sender, EventArgs e)
        {
           
        }
        private void txtBox_KeyDown(object sender, KeyEventArgs e)
        {
            tBox = (TextBox)sender;
            try
            {
                if (tBox.Text != string.Empty)
                {
                    double dmlVal = Convert.ToDouble(tBox.Text);
                    if (e.KeyCode == Keys.Down && dmlVal > 0)
                    {
                        if (rdbPx.Checked)
                        {
                            dmlVal = dmlVal - 1;
                        }
                        else
                        {
                            dmlVal = dmlVal - 0.01;
                        }
                    }
                    else if (e.KeyCode == Keys.Up)
                    {
                        if (rdbPx.Checked)
                        {
                            dmlVal = dmlVal + 1;
                        }
                        else
                        {
                            dmlVal = dmlVal + .01;
                        }
                    }
                    else return;
                    tBox.Text = dmlVal.ToString();
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (tBox == txtRight || tBox == txtBottom)
                {
                    isMrgnKeyPressed = true;
                }
                else if (tBox == txtLabelWth || tBox == txtLabelHt)
                {
                    isLblKeyPressed = true;
                }
            }
        }
        private void Margin_TextChanged(object sender, EventArgs e)
        {
            tBox = (TextBox)sender;
            try
            {
                if (isMrgnKeyPressed == true)
                {
                    if (rdbPx.Checked)
                    {
                        intBtmMgn = Convert.ToInt32(txtBottom.Text);
                        intRtMgn = Convert.ToInt32(txtRight.Text);
                    }
                    else if (rdbCM.Checked)
                    {
                        dblRtMargin = Convert.ToDouble(txtRight.Text) * 39.3701;
                        intRtMgn = Convert.ToInt32(Math.Round(dblRtMargin, 0));
                        dblBtmMargin = Convert.ToDouble(txtBottom.Text) * 39.3701;
                        intBtmMgn = Convert.ToInt32(Math.Round(dblBtmMargin, 0));
                    }
                    else
                    {
                        dblRtMargin = Convert.ToDouble(txtRight.Text) * 100;
                        intRtMgn = Convert.ToInt32(Math.Round(dblRtMargin, 0));
                        dblBtmMargin = Convert.ToDouble(txtBottom.Text) * 100;
                        intBtmMgn = Convert.ToInt32(Math.Round(dblBtmMargin, 0));
                    }
                    intLabelWidth = pnlLabel.Width;
                    intLabelHeight = pnlLabel.Height;
                    if (tBox == txtRight || tBox == txtBottom)
                    {
                        pnlMargin.Size = new Size(intRtMgn + intLabelWidth + pnlLabel.Location.X, intBtmMgn + intLabelHeight + pnlLabel.Location.Y);
                        pnlLabel.Height = intLabelHeight;
                        pnlLabel.Width = intLabelWidth;
                    }
                    isMrgnKeyPressed = false;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                WidthAndHightFindMethod();
            }
        }
        private void txtRight_KeyPress(object sender, KeyPressEventArgs e)
        {
            isMrgnKeyPressed = true;
        }
        private void txtLabelWth_KeyPress(object sender, KeyPressEventArgs e)
        {
            isLblKeyPressed = true;
        }
        private void BarcodeDesign_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Alt && e.KeyCode == Keys.T)
            {
                if (pnlTesting.Visible)
                {
                    pnlTesting.Visible = false;
                    pnlTesting.Location = new Point(315, 150);
                }
                else
                {
                    pnlTesting.Visible = true;
                }
            }
            else if (e.KeyCode == Keys.F8)
            {
                btnSave_Click(null,null);
            }
            else if (e.KeyCode == Keys.F11)
            {
                btnOpen_Click(null, null);
            }
            else if (e.KeyCode == Keys.F2)
            {
                btnEncode_Click(null, null);
            }
        }
        private void pnlTesting_MouseDown(object sender, MouseEventArgs e)
        {
            isTestingPanelClicked = true;
            if (e.Button == MouseButtons.Left) PanelMouseDownLocation = e.Location;
        }
        private void pnlTesting_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTestingPanelClicked)
            {
                pnlTesting.Left += e.X - PanelMouseDownLocation.X;
                pnlTesting.Top += e.Y - PanelMouseDownLocation.Y;
            }
        }
        private void pnlTesting_MouseUp(object sender, MouseEventArgs e)
        {
            isTestingPanelClicked = false;
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            bhdr = new BarcodeHDR();
            rdbPx.Checked = true;
            try
            {
                OpenFileDialog ofDialog = new OpenFileDialog();
                if (ofDialog.ShowDialog() == DialogResult.OK)
                {
                    strFileName = ofDialog.FileName;
                    txtFilePath.Text = strFileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (!File.Exists(strFileName))
            {
                return;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(BarcodeFormatClass));
            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
            // Declare an object variable of the type to be deserialized.
            BarcodeFormatClass _objFormat;
            // A FileStream is needed to read the XML document.
            using (FileStream fs = new FileStream(strFileName, FileMode.Open))
            {
                /* Use the Deserialize method to restore the object's state with
                data from the XML document. */
                _objFormat = (BarcodeFormatClass)serializer.Deserialize(fs);
                fs.Close();
            }
            bhdr = null;
            _dtls.Clear();
            _dtls = _objFormat._barcodeDTLs;
            bhdr = _objFormat._barcodeHDR;
            txtLeft.Text = bhdr.FirstMargin.ToString();
            txtTop.Text = bhdr.TopMargin.ToString();
            txtRight.Text = bhdr.RightMargin.ToString();
            txtBottom.Text = bhdr.BottomMargin.ToString();
            txtLabelWth.Text = bhdr.Width.ToString();
            txtLabelHt.Text = bhdr.Height.ToString();
            txtAcross.Text = bhdr.Accross.ToString();
            txtDown.Text = bhdr.Down.ToString();
            lblPrintPerPage.Text = bhdr.TotalPerPage.ToString();
            cmbPrinter.Text = bhdr.PrinterName.ToString();
            cmbPaperSize.Text = bhdr.PaperName.ToString();
            txtPSWdth.Text = bhdr.PSWidth.ToString();
            txtPSHt.Text = bhdr.PSHieght.ToString();
            cbBorder.Checked = bhdr.isBorderIncluded;
            LoadControlsMwthod();
            cmbBarcodeValue.SelectedIndex = bhdr.BarcodeField;
            pnlMargin.Width = bhdr.Width+20+bhdr.RightMargin;
            pnlMargin.Height = bhdr.Height + 20+ bhdr.BottomMargin;
            pnlLabel.Location = new Point(10, 10);
        }
        private void btnLogo_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofDialog = new OpenFileDialog();
                if (ofDialog.ShowDialog() == DialogResult.OK)
                {
                    strFileName = ofDialog.FileName;
                    txtPrefixVal.Text = strFileName;
                    foreach (Control item in pnlLabel.Controls.OfType<PictureBox>())
                    {
                        item.BackgroundImage = Image.FromFile(strFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void cmbPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPaperSize.Items.Clear();
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = cmbPrinter.Text;
            foreach (PaperSize psize in ps.PaperSizes)
            {
                cmbPaperSize.Items.Add(psize.PaperName);
            }
            if (cmbPaperSize.Items.Contains("A4"))
            {
                cmbPaperSize.Text = "A4";
                txtPSWdth.Text = "827";
                txtPSHt.Text = "1169";
            }
        }
        #endregion
    }
}
