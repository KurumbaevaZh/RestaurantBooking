using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RestaurantBookingLibrary.Interfaces;
using RestaurantBookingLibrary.Managers;
using RestaurantBookingLibrary.Models;

namespace RestaurantBooking
{
    public partial class ReservationWindow : Window
    {
        private User currentUser; // Текущий пользователь
        private ITableManager tableManager; // Менеджер столов
        private IReservationManager reservationManager; // Менеджер бронирований

        public ReservationWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        // Загрузка окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var restaurant = DataManager.LoadData();
            tableManager = new TableManager(restaurant);
            reservationManager = new ReservationManager(restaurant);

            // Подписка на событие подтверждения бронирования
            reservationManager.ReservationConfirmed += ReservationManager_ReservationConfirmed;
            LoadReservations();
        }

        // Закрытие окна
        private void Window_Closed(object sender, EventArgs e)
        {
            var restaurant = new Restaurant
            {
                Tables = tableManager.GetAllTables(),
                Reservations = reservationManager.GetAllReservations()
            };
            DataManager.SaveData(restaurant);
        }

        // Поиск доступных столов
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(PeopleCountTextBox.Text, out int guests) && DateDatePicker.SelectedDate.HasValue && TimeComboBox.SelectedItem != null)
            {
                var selectedDate = DateDatePicker.SelectedDate.Value;
                var selectedTime = ((ComboBoxItem)TimeComboBox.SelectedItem).Content.ToString();
                var dateTime = selectedDate.Add(TimeSpan.Parse(selectedTime));
                var availableTables = tableManager.GetAvailableTablesByCapacityAndTime(guests, dateTime);

                TablesDataGrid.ItemsSource = availableTables;
            }
            else
            {
                MessageBox.Show("Введите данные.");
            }
        }
        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            if (TablesDataGrid.SelectedItem != null)
            {
                MessageBox.Show("Бронирование успешно выполнено!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите стол для бронирования.");
            }
        }


        // Обработка события подтверждения бронирования
        private void ReservationManager_ReservationConfirmed(object sender, Reservation e)
        {
            MessageBox.Show($"Reservation confirmed for table {e.Table.TableId} at {e.ReservationTime}");
        }

        // Загрузка списка бронирований
        private void LoadReservations()
        {
            var reservations = reservationManager.GetAllReservations()
                .Where(r => r.User.Username == currentUser.Username)
                .ToList();
        }
    }
}
