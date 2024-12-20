using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CargoTransportationApp.Models;

namespace CargoTransportationApp.Forms
{
    public partial class OrderEditForm : Form
    {
        public Order Order { get; private set; }
        private List<Client> clients;
        private List<Tariff> tariffs;

        // Максимальное значение объема груза
        private const double MaxVolume = 10000;

        // Конструктор для создания нового заказа
        public OrderEditForm(List<Client> clients, List<Tariff> tariffs)
        {
            InitializeComponent();
            this.clients = clients;
            this.tariffs = tariffs;

            comboBoxClient.DataSource = clients;
            comboBoxClient.DisplayMember = "Name";

            comboBoxTariff.DataSource = tariffs;
            comboBoxTariff.DisplayMember = "Name";

            // Подписка на события
            textBoxVolume.TextChanged += textBoxVolume_TextChanged;
            comboBoxTariff.SelectedIndexChanged += comboBoxTariff_SelectedIndexChanged;
        }

        // Конструктор для редактирования существующего заказа
        public OrderEditForm(List<Client> clients, List<Tariff> tariffs, Order order) : this(clients, tariffs)
        {
            this.Order = order;

            comboBoxClient.SelectedItem = order.Client;
            comboBoxTariff.SelectedItem = order.Tariff;
            textBoxVolume.Text = order.Volume.ToString();
            UpdateTotalCost();
        }

        private void UpdateTotalCost()
        {
            if (comboBoxTariff.SelectedItem != null && double.TryParse(textBoxVolume.Text, out double volume))
            {
                if (volume > 0 && volume <= MaxVolume)
                {
                    Tariff selectedTariff = (Tariff)comboBoxTariff.SelectedItem;
                    double totalCost = selectedTariff.PricePerUnit * volume;
                    labelTotalCost.Text = $"Общая стоимость: {totalCost:C2}";
                }
                else
                {
                    labelTotalCost.Text = "Общая стоимость: N/A";
                }
            }
            else
            {
                labelTotalCost.Text = "Общая стоимость: N/A";
            }
        }

        private void textBoxVolume_TextChanged(object sender, EventArgs e)
        {
            UpdateTotalCost();
        }

        private void comboBoxTariff_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTotalCost();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxTariff.SelectedItem == null)
            {
                MessageBox.Show("Выберите тариф.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(textBoxVolume.Text, out double volume))
            {
                MessageBox.Show("Введите корректный объем груза.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка, что объем в пределах допустимого диапазона
            if (volume <= 0 || volume > MaxVolume)
            {
                MessageBox.Show($"Объем груза должен быть в пределах от 1 до {MaxVolume}.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Order == null)
            {
                // Создание нового заказа
                Order = new Order
                {
                    Id = 0, // ID будет присвоен позже
                    Client = (Client)comboBoxClient.SelectedItem,
                    Tariff = (Tariff)comboBoxTariff.SelectedItem,
                    Volume = volume
                };
            }
            else
            {
                // Обновление существующего заказа
                Order.Client = (Client)comboBoxClient.SelectedItem;
                Order.Tariff = (Tariff)comboBoxTariff.SelectedItem;
                Order.Volume = volume;
            }

            Order.CalculateTotalCost();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
