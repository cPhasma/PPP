using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CargoTransportationApp.Models;
using CargoTransportationApp.Utils;

namespace CargoTransportationApp.Forms
{
    public partial class OrdersForm : Form
    {
        private List<Order> orders;
        private List<Client> clients;
        private List<Tariff> tariffs;

        public OrdersForm(List<Client> clients, List<Tariff> tariffs)
        {
            InitializeComponent();
            this.clients = clients;
            this.tariffs = tariffs;

            // Загружаем заказы из файла
            orders = DataManager.LoadOrders();

            // Если заказов нет, создаем пустой список
            if (orders == null)
                orders = new List<Order>();

            UpdateDataGridView();
        }

        private void UpdateDataGridView()
        {
            dataGridViewOrders.DataSource = null;
            dataGridViewOrders.DataSource = orders;

            // Настраиваем отображение колонок
            if (dataGridViewOrders.Columns.Count > 0)
            {
                dataGridViewOrders.Columns["Client"].HeaderText = "Клиент";
                dataGridViewOrders.Columns["Tariff"].HeaderText = "Тариф";
                dataGridViewOrders.Columns["Volume"].HeaderText = "Объем груза";
                dataGridViewOrders.Columns["TotalCost"].HeaderText = "Общая стоимость";

                // Скрываем колонку Id, если она отображается
                if (dataGridViewOrders.Columns.Contains("Id"))
                {
                    dataGridViewOrders.Columns["Id"].Visible = false;
                }

                // Добавляем настройку авторазмера колонок
                dataGridViewOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // Открываем форму для добавления нового заказа
            OrderEditForm editForm = new OrderEditForm(clients, tariffs);
            if (editForm.ShowDialog() == DialogResult.OK)
            {
                // Генерация уникального ID для нового заказа
                int newId = orders.Count > 0 ? orders[orders.Count - 1].Id + 1 : 1;
                editForm.Order.Id = newId;

                orders.Add(editForm.Order);

                // Сохраняем данные после добавления
                DataManager.SaveOrders(orders);

                // Обновляем отображение таблицы
                UpdateDataGridView();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                int index = dataGridViewOrders.SelectedRows[0].Index;
                Order selectedOrder = orders[index];

                // Открываем форму для редактирования заказа
                OrderEditForm editForm = new OrderEditForm(clients, tariffs, selectedOrder);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // Обновляем заказ в списке
                    orders[index] = editForm.Order;

                    // Сохраняем данные после редактирования
                    DataManager.SaveOrders(orders);

                    // Обновляем отображение таблицы
                    UpdateDataGridView();
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для редактирования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewOrders.SelectedRows.Count > 0)
            {
                int index = dataGridViewOrders.SelectedRows[0].Index;

                var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный заказ?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Удаляем заказ из списка
                    orders.RemoveAt(index);

                    // Сохраняем данные после удаления
                    DataManager.SaveOrders(orders);

                    // Обновляем отображение таблицы
                    UpdateDataGridView();
                }
            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OrdersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Сохраняем заказы при закрытии формы
            DataManager.SaveOrders(orders);
        }
    }
}
