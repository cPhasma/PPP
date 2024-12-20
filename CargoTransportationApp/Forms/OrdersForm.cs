using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CargoTransportationApp.Models;
using CargoTransportationApp.Utils;

namespace CargoTransportationApp.Forms
{
    public partial class OrdersForm : Form
    {
        private List<Order> orders;

        public OrdersForm()
        {
            InitializeComponent();
            LoadData();
        }

        // Перегруженный конструктор для совместимости, если где-то вызывается OrdersForm(clients, tariffs)
        public OrdersForm(List<Client> clients, List<Tariff> tariffs) : this()
        {
            // Этот конструктор специально оставлен пустым.
            // Мы игнорируем переданные списки и всегда загружаем данные из базы.
        }

        private List<Client> LoadClientsFromDatabase()
        {
            string query = "SELECT id, name, contact_info FROM clients;";
            DataTable dataTable = DatabaseManager.ExecuteQuery(query);

            var freshClients = new List<Client>();
            foreach (DataRow row in dataTable.Rows)
            {
                freshClients.Add(new Client
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
                    ContactInfo = row["contact_info"].ToString()
                });
            }
            return freshClients;
        }

        private List<Tariff> LoadTariffsFromDatabase()
        {
            string query = "SELECT id, name, price_per_unit FROM tariffs;";
            DataTable dataTable = DatabaseManager.ExecuteQuery(query);

            var freshTariffs = new List<Tariff>();
            foreach (DataRow row in dataTable.Rows)
            {
                freshTariffs.Add(new Tariff
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = row["name"].ToString(),
                    PricePerUnit = Convert.ToDouble(row["price_per_unit"])
                });
            }
            return freshTariffs;
        }

        private void LoadData()
        {
            try
            {
                string query = @"
                    SELECT o.id, o.volume, o.total_cost,
                           c.id AS client_id, c.name AS client_name, c.contact_info AS client_contact_info,
                           t.id AS tariff_id, t.name AS tariff_name, t.price_per_unit AS tariff_price
                    FROM orders o
                    JOIN clients c ON o.client_id = c.id
                    JOIN tariffs t ON o.tariff_id = t.id;";

                DataTable dataTable = DatabaseManager.ExecuteQuery(query);

                orders = new List<Order>();
                foreach (DataRow row in dataTable.Rows)
                {
                    var orderClient = new Client
                    {
                        Id = Convert.ToInt32(row["client_id"]),
                        Name = row["client_name"].ToString(),
                        ContactInfo = row["client_contact_info"].ToString()
                    };

                    var orderTariff = new Tariff
                    {
                        Id = Convert.ToInt32(row["tariff_id"]),
                        Name = row["tariff_name"].ToString(),
                        PricePerUnit = Convert.ToDouble(row["tariff_price"])
                    };

                    var order = new Order
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Volume = Convert.ToDouble(row["volume"]),
                        Client = orderClient,
                        Tariff = orderTariff
                        // TotalCost вычисляется через Volume и Tariff.PricePerUnit внутри класса Order
                    };

                    orders.Add(order);
                }

                UpdateDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDataGridView()
        {
            dataGridViewOrders.DataSource = null;
            dataGridViewOrders.DataSource = orders;

            if (dataGridViewOrders.Columns.Count > 0)
            {
                if (dataGridViewOrders.Columns.Contains("Id"))
                    dataGridViewOrders.Columns["Id"].Visible = false;

                dataGridViewOrders.Columns["Client"].HeaderText = "Клиент";
                dataGridViewOrders.Columns["Tariff"].HeaderText = "Тариф";
                dataGridViewOrders.Columns["Volume"].HeaderText = "Объем груза";
                dataGridViewOrders.Columns["TotalCost"].HeaderText = "Общая стоимость";
                dataGridViewOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var freshClients = LoadClientsFromDatabase();
                var freshTariffs = LoadTariffsFromDatabase();

                OrderEditForm editForm = new OrderEditForm(freshClients, freshTariffs);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    double totalCost = editForm.Order.Volume * editForm.Order.Tariff.PricePerUnit;

                    string command = @"
                        INSERT INTO orders (client_id, tariff_id, volume, total_cost)
                        VALUES (@client_id, @tariff_id, @volume, @total_cost);";

                    var parameters = new Dictionary<string, object>
                    {
                        { "@client_id", editForm.Order.Client.Id },
                        { "@tariff_id", editForm.Order.Tariff.Id },
                        { "@volume", editForm.Order.Volume },
                        { "@total_cost", totalCost }
                    };

                    DatabaseManager.ExecuteNonQuery(command, parameters);

                    LoadData();
                    MessageBox.Show("Заказ успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewOrders.SelectedRows.Count > 0)
                {
                    int index = dataGridViewOrders.SelectedRows[0].Index;
                    Order selectedOrder = orders[index];

                    var freshClients = LoadClientsFromDatabase();
                    var freshTariffs = LoadTariffsFromDatabase();

                    OrderEditForm editForm = new OrderEditForm(freshClients, freshTariffs, selectedOrder);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        double totalCost = editForm.Order.Volume * editForm.Order.Tariff.PricePerUnit;

                        string command = @"
                            UPDATE orders
                            SET client_id = @client_id, tariff_id = @tariff_id, volume = @volume, total_cost = @total_cost
                            WHERE id = @id;";

                        var parameters = new Dictionary<string, object>
                        {
                            { "@id", selectedOrder.Id },
                            { "@client_id", editForm.Order.Client.Id },
                            { "@tariff_id", editForm.Order.Tariff.Id },
                            { "@volume", editForm.Order.Volume },
                            { "@total_cost", totalCost }
                        };

                        DatabaseManager.ExecuteNonQuery(command, parameters);

                        LoadData();
                        MessageBox.Show("Заказ успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите заказ для редактирования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewOrders.SelectedRows.Count > 0)
                {
                    int index = dataGridViewOrders.SelectedRows[0].Index;
                    Order selectedOrder = orders[index];

                    var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный заказ?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string command = "DELETE FROM orders WHERE id = @id;";
                        var parameters = new Dictionary<string, object>
                        {
                            { "@id", selectedOrder.Id }
                        };

                        DatabaseManager.ExecuteNonQuery(command, parameters);

                        LoadData();
                        MessageBox.Show("Заказ успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите заказ для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении заказа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
