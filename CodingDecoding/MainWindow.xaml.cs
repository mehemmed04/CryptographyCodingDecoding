using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodingDecoding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string GetEnCodeFromRichtextbox()
        {
            TextRange textRange = new TextRange(
        EnCodeTxb.Document.ContentStart,
        EnCodeTxb.Document.ContentEnd
    );
            return textRange.Text;
        }
        public string GetDeCodeFromRichtextbox()
        {
            TextRange textRange = new TextRange(
        DecodeTxtb2.Document.ContentStart,
        DecodeTxtb2.Document.ContentEnd
    );
            return textRange.Text;
        }
        public void SetDecodeRichtextbox(string text)
        {
            DeCodeTxtb.Document.Blocks.Clear();
            DeCodeTxtb.Document.Blocks.Add(new Paragraph(new Run(text)));
        }
        public void SetEncodeRichtextbox(string text)
        {
            EncodeTxtb2.Document.Blocks.Clear();
            EncodeTxtb2.Document.Blocks.Add(new Paragraph(new Run(text)));
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MD5Btn_Click(object sender, RoutedEventArgs e)
        {
            string str_encode = GetEnCodeFromRichtextbox();
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str_encode));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            SetDecodeRichtextbox(sBuilder.ToString());
        }

        private void SHA256Btn_Click(object sender, RoutedEventArgs e)
        {
            string value = GetEnCodeFromRichtextbox();
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            SetDecodeRichtextbox(Sb.ToString());
        }

        private void BASE64Btn_Click(object sender, RoutedEventArgs e)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(GetEnCodeFromRichtextbox());
            SetDecodeRichtextbox(System.Convert.ToBase64String(plainTextBytes));
        }

        private void DeCodeMD5Btn_Click(object sender, RoutedEventArgs e)
        {

        }

  

        private void DeCodeBASE64Btn_Click(object sender, RoutedEventArgs e)
        {
            SetEncodeRichtextbox(Base64Decode(GetDeCodeFromRichtextbox()));
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
