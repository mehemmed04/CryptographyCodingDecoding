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
        
        public MainWindow()
        {
            InitializeComponent();
        }
       

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(txtUser.Text=="ashraf" && txtPass.Password == "12345678")
            {
                CryptograpyhWindow cw = new CryptograpyhWindow();
                cw.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Yanlisdir.Tekrar yoxlayin");
            }
        }
    }
}
