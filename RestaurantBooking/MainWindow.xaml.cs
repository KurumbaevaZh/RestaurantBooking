using System;
using System.Windows;
using Newtonsoft.Json;
using RestaurantBookingLibrary.Interfaces;
using RestaurantBookingLibrary.Managers;
using RestaurantBookingLibrary.Models;

namespace RestaurantBooking
{
    public partial class MainWindow : Window
    {
        private IUserManager userManager;
        private User currentUser;

        public MainWindow()
        {
            InitializeComponent();
            userManager = new UserManager();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            currentUser = userManager.Login(username, password);

            if (currentUser != null)
            {
                MessageBox.Show("Вы вошли в свою учетную запись");
                OpenReservationWindow();
            }
            else
            {
                MessageBox.Show("Неверные учетные данные");
            }
        }
        //регистрация
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            var newUser = new User { Username = username, Password = password };
            userManager.RegisterUser(newUser);
            MessageBox.Show("Регистрация прошла успешно");
        }

        private void OpenReservationWindow()
        {
            var reservationWindow = new ReservationWindow(currentUser);
            reservationWindow.Show();
            this.Close();
        }
    }
}