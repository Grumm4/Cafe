using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Cafe
{
    /// <summary>
    /// Логика взаимодействия для RegNewUser.xaml
    /// </summary>
    public partial class RegNewUser : Window
    {
        public RegNewUser()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<string> list = new List<string> {"Админ", "Повар", "Официант" };
            CbRole.ItemsSource = list;
        }

        void Registr()
        {
            try
            {
                var u = new UserModel
                {
                    Login = string.IsNullOrEmpty(TbLogin.Text) ? throw new NullReferenceException() : TbLogin.Text,
                    HashedPwd = string.IsNullOrEmpty(TbPass.Password) ? throw new NullReferenceException() : TbPass.Password,
                    Role = CbRole.SelectedValue.ToString(),
                    FullName = string.IsNullOrEmpty(TbName.Text) ? throw new NullReferenceException() : TbName.Text,
                    Status = "Работает"
                };

                using (var context = new DBContext())
                {
                    if ((context.Users.FirstOrDefault(u => u.Login == TbLogin.Text)) != null)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Успешная регистрация!");

                        TbLogin.Text = String.Empty; TbName.Text = String.Empty; TbPass.Password = String.Empty;
                        CbRole.SelectedValue = 0;
                        context.Users.Add(u);
                        context.SaveChanges();
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Все поля должны быть заполнены!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        private void RegBt_Click(object sender, RoutedEventArgs e)
        {
            Registr();
        }
    }
}
