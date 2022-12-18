using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CodingDecoding
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow vm = new MainWindow();
            vm.Show();
            this.Close();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidEmail)
            {
                if (IsValidPassword)
                {


                    string username = txtUser.Text;
                    using (var ctx = new UserContext())
                    {
                        var users = ctx.Users.ToList();
                        var user = users.FirstOrDefault((u) => { return u.Username == username; });
                        if (user != null)
                        {
                            MessageBox.Show($"There is username with {username}");
                        }
                        else
                        {
                            string password = txtPass.Text;
                            string email = txtEmail.Text;

                            User newuser = new User
                            {
                                Password = password,
                                Username = username,
                                Email = email
                            };

                            string body = $@"
Hello {username},
You registered succesfully in our app
You can sign in now
Thanks ...
";

                            ctx.Users.Add(newuser);
                            ctx.SaveChanges();

                            SendEmail(email, body);

                            MessageBox.Show("Registered succesfully");
                            MainWindow vm = new MainWindow();
                            vm.Show();
                            this.Close();

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email");
                }
            }
            else
            {
                MessageBox.Show("Invalid password");
            }
        }

        public bool IsValidEmail { get; set; } = false;
        public bool IsValidPassword { get; set; } = false;
        public void SendEmail(string receiver, string body)
        {
            try
            {
                string fromEmail = "cooding002@gmail.com";
                MailMessage mailMessage = new MailMessage(fromEmail, receiver, "Registered Succesfully", body);
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(fromEmail, "kxndtkcoktskhvrt");
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {

            }
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            string txt = txtEmail.Text;
            var valid = true;
            try
            {
                var emailAddress = new MailAddress(txt);
            }
            catch
            {
                valid = false;
            }
            try
            {
                if (valid && txt.Split('@')[1].Split('.')[1].Length > 0)
                {
                    alertTxt.Text = "";
                    IsValidEmail = true;
                }
                else
                {
                    IsValidEmail = false;
                    alertTxt.Text = "invalid email";
                }
            }
            catch (Exception)
            {

            }

        }

        private void txtPass_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pass = txtPass.Text;
            if (pass.Length > 7)
            {
                alertTxt2.Text = "";
                IsValidPassword = true;
            }
            else
            {
                alertTxt2.Text = "Invalid password";
                IsValidPassword = false;
            }
        }
    }
}
