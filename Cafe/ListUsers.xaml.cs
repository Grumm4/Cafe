using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
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

namespace Cafe
{
    /// <summary>
    /// Логика взаимодействия для ListUsers.xaml
    /// </summary>
    public partial class ListUsers : Window
    {
        public ListUsers()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new DBContext())
            {
                List<UserModel> users = context.Users.ToList();
                List<UserModel> newUsers = new List<UserModel>();
                foreach (var user in users)
                {
                    UserModel u = new UserModel { 
                        Login = user.Login, 
                        FullName = user.FullName, 
                        Role = user.Role, 
                        Status = user.Status, 
                        ActiveShiftId = user.ActiveShiftId, 
                        Id = user.Id };
                    newUsers.Add(u);
                }
                UsersDG.ItemsSource = newUsers;
            }
        }



        private void RegNewUserBt_Click(object sender, RoutedEventArgs e)
        {
            RegNewUser regWindow = new RegNewUser();
            regWindow.Closing += ChildWindowClosing;
            regWindow.ShowDialog();
        }

        private void ChildWindowClosing(object sender, EventArgs e) 
        {
            Window_Loaded(sender, new RoutedEventArgs());
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            new AdminWindow().Show();
        }

        //изменить статус
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedUser = (UserModel)UsersDG.SelectedItem;
                if (selectedUser != null)
                {
                    using (var context = new DBContext())
                    {
                        if (selectedUser.Status == "Работает")
                        {
                            if (MessageBox.Show("Вы действительно хотите поменять статус на \"Уволен\" ?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                var user = context.Users.Where(u => u.Id == selectedUser.Id).FirstOrDefault();
                                user.Status = "Уволен";
                                context.SaveChanges();
                            }
                        }
                        else if (selectedUser.Status == "Уволен")
                        {
                            if (MessageBox.Show("Вы действительно хотите поменять статус на \"Работает\" ?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                var user = context.Users.Where(u => u.Id == selectedUser.Id).FirstOrDefault();
                                user.Status = "Работает";
                                context.SaveChanges();
                            }
                        }
                    }
                    Window_Loaded(sender, e);
                }
                
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Клик должен производиться по строке в таблице!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Назначить на смену
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedUser = (UserModel)UsersDG.SelectedItem;
                if (selectedUser != null)
                {
                    if (new UserToShift(selectedUser).ShowDialog() == true)
                    {
                        Window_Loaded(sender, e);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Клик должен производиться по строке в таблице!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //Удалить пользователя
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedUser = (UserModel)UsersDG.SelectedItem;
                using (var context = new DBContext())
                {
                    if (selectedUser != null)
                    {
                        var user = context.Users.SingleOrDefault(u => u.Id == selectedUser.Id);
                        if (user.Id == LoginWindow.currentUser.Id)
                        {
                            MessageBox.Show("Вы не можете удалить того же пользователя, под которым авторизованы!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            if (MessageBox.Show("Вы действительно хотите удалить этого пользователя?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                context.Users.Remove(user);
                                context.SaveChanges();
                                Window_Loaded(sender, e);
                            }

                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Клик должен производиться по строке в таблице!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
