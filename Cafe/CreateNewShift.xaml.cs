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
    /// Логика взаимодействия для CreateNewShift.xaml
    /// </summary>
    public partial class CreateNewShift : Window
    {
        bool HasNewShift;
        public CreateNewShift()
        {
            InitializeComponent();
        }

        private void CreateShiftBt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateOnly dateOnly = new DateOnly(DateShiftDp.SelectedDate.Value.Year, DateShiftDp.SelectedDate.Value.Month, DateShiftDp.SelectedDate.Value.Day);

                TimeOnly timeStart = new TimeOnly(TimeStartTp.Value.Value.Hour, TimeStartTp.Value.Value.Minute);

                TimeOnly timeEnd = new TimeOnly(TimeEndTp.Value.Value.Hour, TimeEndTp.Value.Value.Minute);

                using (var context = new DBContext())
                {
                    context.Shifts.Add(new ShiftModel
                    {
                        Date = string.IsNullOrEmpty(dateOnly.ToString()) ? throw new NullReferenceException() : dateOnly,
                        TimeBeginning = string.IsNullOrEmpty(timeStart.ToString()) ? throw new NullReferenceException() : timeStart,
                        TimeEnd = string.IsNullOrEmpty(timeEnd.ToString()) ? throw new NullReferenceException() : timeEnd
                    });
                    context.SaveChanges();
                    HasNewShift = true;
                    if (MessageBox.Show("Смена создана, вернуться к окну смен?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        DialogResult = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Все поля должны быть заполнены!", "", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (HasNewShift)
            {
                DialogResult = true;
            }
        }
    }
}
