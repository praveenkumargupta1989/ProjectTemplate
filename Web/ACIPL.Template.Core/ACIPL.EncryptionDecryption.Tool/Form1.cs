using ACIPL.Template.Core.Utilities;
using System;
using System.Windows.Forms;

namespace ACIPL.EncryptionDecryption.Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            var result = CryptoEngine.Encrypt(txtInputText.Text, txtKey.Text);
            txtResultText.Text = result;
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            var result = CryptoEngine.Decrypt(txtInputText.Text, txtKey.Text);
            txtResultText.Text = result;
        }
    }
}
