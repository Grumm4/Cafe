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
    /// Логика взаимодействия для WaiterWindow.xaml
    /// </summary>
    public partial class WaiterWindow : Window
    {
        public WaiterWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //сокрытие кнопки создания заказа
            if (LoginWindow.currentUser.Role == "Admin" | LoginWindow.currentUser.Role == "Повар")
            {
                CreateNewOrderBt.Visibility = Visibility.Collapsed;
            }

            //заполнение combobox'a статусами, в зависимости от роли и сокрытие его, если роль равна Admin
            if (LoginWindow.currentUser.Role == "Официант")
            {
                StatusCb.ItemsSource = new List<string>() { "Принят", "Оплачен" };
                OrderStatusCm.Visibility = Visibility.Visible;
            }
            else if (LoginWindow.currentUser.Role == "Повар")
            {
                StatusCb.ItemsSource = new List<string>() { "Готовится", "Готов" };
                OrderStatusCm.Visibility = Visibility.Visible;
            }
            else
            {
                LogoutBt.Visibility = Visibility.Collapsed;
                OrderStatusCm.Visibility = Visibility.Collapsed;
            }

            using (var context = new DBContext())
            {
                var orders = context.Orders.Where(x => x.DateAndTime.Value.Day == DateTime.Now.Day).ToList();
                if (orders.Any() == false || LoginWindow.currentUser.Role == "Admin")
                {
                    OrderStatusCm.Visibility = Visibility.Collapsed;
                }
                if (orders != null)
                {
                    OrdersDG.ItemsSource = orders;
                }
                
            }
        }

        private void CreateNewOrderBt_Click(object sender, RoutedEventArgs e)
        {
            if (new CreateNewOrder().ShowDialog() == true)
            {
                Window_Loaded(sender, e);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LoginWindow.currentUser.Role == "Admin")
                new AdminWindow().Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var order = (OrderModel)OrdersDG.SelectedItem;
                using (var context = new DBContext())
                {
                    context.Orders.Single(o => o.Id == order.Id).OrderStatus = StatusCb.SelectedValue.ToString();
                    context.SaveChanges();
                }
                Window_Loaded(sender, e);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("Клик должен производиться по строке в таблице!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LogoutBt_Click(object sender, RoutedEventArgs e)
        {
            Animation.Animate(this, new LoginWindow());
        }
    }
}
