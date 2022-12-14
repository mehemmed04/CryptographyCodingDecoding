using System;
using System.Collections.Generic;
using System.IO;
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
        static string hash { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";
        private void MD5Btn_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(GetEnCodeFromRichtextbox());
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    SetDecodeRichtextbox(Convert.ToBase64String(results, 0, results.Length));
                }
            }
        }

        private void SHA256Btn_Click(object sender, RoutedEventArgs e)
        {
            string value = GetEnCodeFromRichtextbox();

            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(value);
            byte[] passwordBytes = Encoding.UTF8.GetBytes("Password");

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string encryptedResult = Convert.ToBase64String(bytesEncrypted);

            SetDecodeRichtextbox(encryptedResult);
        }

        private void BASE64Btn_Click(object sender, RoutedEventArgs e)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(GetEnCodeFromRichtextbox());
            SetDecodeRichtextbox(System.Convert.ToBase64String(plainTextBytes));
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

        private void DeCodeSHA256Btn_Click(object sender, RoutedEventArgs e)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(GetDeCodeFromRichtextbox());
            byte[] passwordBytesdecrypt = Encoding.UTF8.GetBytes("Password");
            passwordBytesdecrypt = SHA256.Create().ComputeHash(passwordBytesdecrypt);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytesdecrypt);

            string decryptedResult = Encoding.UTF8.GetString(bytesDecrypted);
            /***********************End*Decryption******************************************/

            SetEncodeRichtextbox(decryptedResult);
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 2, 1, 7, 3, 6, 4, 8, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 2, 1, 7, 3, 6, 4, 8, 5 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private void DeCodeMD5Btn_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = Convert.FromBase64String(GetDeCodeFromRichtextbox()); // decrypt the incrypted text
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                  SetEncodeRichtextbox( UTF8Encoding.UTF8.GetString(results));
                }
            }
        }
    }
}
