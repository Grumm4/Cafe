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
    /// Логика взаимодействия для UserToShift.xaml
    /// </summary>
    public partial class UserToShift : Window
    {
        UserModel selectedUser;
        bool HasToShiftAdded;
        public UserToShift(UserModel user)
        {
            InitializeComponent();
            this.selectedUser = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new DBContext())
            {
                List<ShiftModel> models = context.Shifts.ToList();
                ShiftDG.ItemsSource = models;
                foreach (var item in models)
                {
                    if (item.Date.Value.Day == DateTime.Now.Day)
                    {
                        ShiftDG.SelectedIndex = item.Id.Value - 1;
                    }
                }
            }
        }

        private void UserToShiftBt_Click(object sender, RoutedEventArgs e)
        {
            if (ShiftDG.SelectedIndex >= 0)
            {
                using (var context = new DBContext())
                {
                    var user = context.Users.SingleOrDefault(u => u.Id == selectedUser.Id);
                    user.ActiveShiftId = context.Shifts.SingleOrDefault(s => s.Id == ((ShiftModel)ShiftDG.SelectedItem).Id).Id;
                    context.SaveChanges();
                    HasToShiftAdded = true;
                    if (MessageBox.Show("Смена назначена, вернуться к списку пользователей?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        DialogResult = true;
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (HasToShiftAdded)
            {
                DialogResult = true;
            }
        }
    }
}
