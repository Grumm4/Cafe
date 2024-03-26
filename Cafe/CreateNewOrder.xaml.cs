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
    /// Логика взаимодействия для CreateNewOrder.xaml
    /// </summary>
    public partial class CreateNewOrder : Window
    {
        bool HasNewOrder;
        public CreateNewOrder()
        {
            InitializeComponent();
        }

        private void CreateOrderBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new DBContext())
                {
                    context.Orders.Add(new OrderModel
                    {
                        ClientName = string.IsNullOrEmpty(ClientNameTb.Text) ? throw new NullReferenceException() : ClientNameTb.Text,
                        FoodList = string.IsNullOrEmpty(FoodListTb.Text) ? throw new NullReferenceException() : FoodListTb.Text,
                        СurrentPrice = string.IsNullOrEmpty(СurrentPriceTb.Text) ? throw new NullReferenceException() : Convert.ToDecimal(СurrentPriceTb.Text),
                        TableNumber = string.IsNullOrEmpty(TableNumberTb.Text) ? throw new NullReferenceException() : Convert.ToInt16(TableNumberTb.Text),
                        ClientCount = string.IsNullOrEmpty(ClientCountTb.Text) ? throw new NullReferenceException() : Convert.ToInt16(ClientCountTb.Text),
                        OrderStatus = "Создан",
                        DateAndTime = DateTime.Now,
                    });

                    context.SaveChanges();
                    HasNewOrder = true;

                    if (MessageBox.Show("Заказ создан, вернуться к списку заказов?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        DialogResult = true;
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Все поля должны быть заполнены!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (HasNewOrder)
            {
                DialogResult = true;
            }
        }
    }
}
