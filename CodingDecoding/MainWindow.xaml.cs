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
        UserContext uc = new UserContext();
        public MainWindow()
        {
            InitializeComponent();
            uc.Database.CreateIfNotExists();
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;

            using (var ctx = new UserContext())
            {
                var users = ctx.Users.ToList();
                var user = users.FirstOrDefault((u) => { return u.Username == username; });
                if (user != null)
                {
                    if (user.Password==password)
                    {
                        CryptograpyhWindow cw = new CryptograpyhWindow();
                        cw.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Password wrong");
                    }
                }
                else
                {
                    MessageBox.Show($"There is not user with {username} username");
                }
            }
            
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow rw = new RegisterWindow();
            rw.Show();
            this.Close();
        }
    }
}
