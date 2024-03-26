using System.Text;
using System.Windows;
using System.Security.Cryptography;

namespace Cafe
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public static UserModel currentUser;

        public LoginWindow()
        {
            InitializeComponent();
            currentUser = new UserModel();
            
        }

        private void LoginBt_Click(object sender, RoutedEventArgs e)
        {
            //using (var c = new DBContext())
            //{
            //    //c.Shifts.Add(new ShiftModel
            //    //{
            //    //    TimeBeginning = new TimeOnly(07, 00),
            //    //    TimeEnd = new TimeOnly(15, 00),
            //    //    Date = new DateOnly(2024, 03, 25),
            //    //});
            //    //c.Users.Add(new UserModel
            //    //{
            //    //    Login = "Admin",
            //    //    ActiveShift = c.Shifts.ToList()[0],
            //    //    FullName = "FIO",
            //    //    Role = "Admin",
            //    //    Status = "Ok",
            //    //    HashedPwd = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3"
            //    //});
            //    //c.SaveChanges();
            //}
            using (var context = new DBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Login == LoginBox.Text && u.HashedPwd == HashPwd(PwdBox.Password));
                if (user != null)
                {
                    currentUser = new UserModel
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Role = user.Role,
                        Status = user.Status,
                        ActiveShiftId = user.ActiveShiftId,
                        FullName = user.FullName
                    };
                    switch (user.Role)
                    {
                        case "Admin":
                            Animation.Animate(this, new AdminWindow());
                            break;
                        case "Официант":
                            Animation.Animate(this, new WaiterWindow());
                            break;
                        case "Повар":
                            Animation.Animate(this, new WaiterWindow());
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин/пароль");
                }
            }
        }

        internal static string HashPwd(string password)
        {
            // Создание экземпляра класса SHA256Managed
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                // Преобразование пароля в массив байтов
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Вычисление хеша пароля
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Преобразование хеша в строку
                string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                return hashedPassword;
            }
        }
    }
}
