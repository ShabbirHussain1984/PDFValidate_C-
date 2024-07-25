using iText.Kernel.Pdf;
using iText.Signatures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFValidation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void ValidateSignature(string pdfPath)
        {
            try
            {
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(pdfPath));
                SignatureUtil signUtil = new SignatureUtil(pdfDoc);
                IList<string> names = signUtil.GetSignatureNames();

                txtSignedData.Text = "N/A";

                if (names.Count == 0)
                {
                    txtSignedData.Text = "The selected PDF file has not been signed.";
                }
                else
                {
                    foreach (string name in names)
                    {
                        PdfPKCS7 pkcs7 = signUtil.ReadSignatureData(name);

                        bool isSignatureVerified = pkcs7.VerifySignatureIntegrityAndAuthenticity();

                        txtSignedData.Text = "";
                        txtSignedData.Text = txtSignedData.Text + ($"Signature {name} is {(isSignatureVerified ? "valid" : "invalid")}") + System.Environment.NewLine;

                        // Print signer info
                        txtSignedData.Text = txtSignedData.Text + ($"Subject: {pkcs7.GetSigningCertificate().GetSubjectDN()}") + System.Environment.NewLine;
                        txtSignedData.Text = txtSignedData.Text + ($"Issuer: {pkcs7.GetSigningCertificate().GetSubjectDN()}") + System.Environment.NewLine;
                        txtSignedData.Text = txtSignedData.Text + ($"Signed on: {pkcs7.GetSignDate()}") + System.Environment.NewLine;

                        // You can also print more detailed certificate information here if needed

                        return;
                    }

                    txtSignedData.Text = "The selected PDF file is signed.";
                }
            }
            catch (Exception ex)
            {
                txtSignedData.Text = ex.Message;
            }
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            DialogResult objDialogResult = openFileDialog1.ShowDialog();

            openFileDialog1.InitialDirectory = "C:\\Users\\hussain\\Desktop\\SignSample";
            openFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.RestoreDirectory = true;

            if (objDialogResult == DialogResult.OK)
            {
                txtFileName.Text = openFileDialog1.FileName;
                ValidateSignature(txtFileName.Text);
            }
        }
    }
}
