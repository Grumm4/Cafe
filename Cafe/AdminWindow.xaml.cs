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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void ToUsersList_Click(object sender, RoutedEventArgs e)
        {
            Animation.Animate(this, new ListUsers());
        }

        private void ToShiftWindowBt_Click(object sender, RoutedEventArgs e)
        {
            Animation.Animate(this, new ShiftWindow());
        }

        private void ToOrderWindowBt_Click(object sender, RoutedEventArgs e)
        {
            Animation.Animate(this, new WaiterWindow());
        }

        private void LogoutBt_Click(object sender, RoutedEventArgs e)
        {
            Animation.Animate(this, new LoginWindow());
        }
    }
}
