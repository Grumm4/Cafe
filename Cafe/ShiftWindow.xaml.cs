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
    /// Логика взаимодействия для ShiftWindow.xaml
    /// </summary>
    public partial class ShiftWindow : Window
    {
        public ShiftWindow()
        {
            InitializeComponent();
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

        private void CreateNewShift_Click(object sender, RoutedEventArgs e)
        {
            if (new CreateNewShift().ShowDialog() == true)
            {
                Window_Loaded(sender, e);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            new AdminWindow().Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedShift = (ShiftModel)ShiftDG.SelectedItem;
            if (selectedShift != null)
            {
                using (var context = new DBContext())
                {
                    if (MessageBox.Show("Вы действительно хотите удалить эту смену?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var shift = context.Shifts.SingleOrDefault(s => s.Id == selectedShift.Id);
                        var usersToUpdate = context.Users.Where(u => u.ActiveShiftId == shift.Id);
                        if (usersToUpdate.Any()) 
                        {
                            foreach (var user in usersToUpdate)
                            {
                                user.ActiveShiftId = null;
                            }
                        }
                        context.SaveChanges();
                        context.Shifts.Remove(shift);

                        context.SaveChanges();
                    }
                }
            }
            Window_Loaded(this, e);
        }
    }
}
