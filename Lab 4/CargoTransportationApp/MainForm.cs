using CargoTransportationApp.Forms;
using CargoTransportationApp.Models;
using CargoTransportationApp.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargoTransportationApp
{
    public partial class MainForm : Form
    {
        private List<Client> clients;
        private List<Tariff> tariffs;

        public MainForm()
        {
            InitializeComponent();

            // Загрузка данных при запуске
            clients = DataManager.LoadClients();
            tariffs = DataManager.LoadTariffs();

            // Если данных нет, инициализируем пустые списки
            if (clients == null || clients.Count == 0)
            {
                clients = new List<Client>
                {
                    new Client { Id = 1, Name = "Иван Иванов", ContactInfo = "ivanov@example.com" },
                    new Client { Id = 2, Name = "Петр Петров", ContactInfo = "petrov@example.com" }
                };
            }

            if (tariffs == null || tariffs.Count == 0)
            {
                tariffs = new List<Tariff>
                {
                    new Tariff { Id = 1, Name = "Эконом", PricePerUnit = 10.0 },
                    new Tariff { Id = 2, Name = "Бизнес", PricePerUnit = 20.0 }
                };
            }
        }

        private void buttonOrders_Click(object sender, EventArgs e)
        {
            // Открытие формы заказов
            OrdersForm ordersForm = new OrdersForm(clients, tariffs);
            ordersForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Сохранение данных перед закрытием
            DataManager.SaveClients(clients);
            DataManager.SaveTariffs(tariffs);
        }

        private void MainForm_FormClosing(object sender, EventArgs e)
        {

        }

        private void тарифыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TariffsForm tariffsForm = new TariffsForm();
            tariffsForm.Show();
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientsForm clientsForm = new ClientsForm();
            clientsForm.Show();
        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrdersForm ordersForm = new OrdersForm(clients, tariffs);
            ordersForm.Show();
        }
    }
}
