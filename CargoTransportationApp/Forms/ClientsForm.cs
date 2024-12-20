using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CargoTransportationApp.Models;
using CargoTransportationApp.Utils;

namespace CargoTransportationApp.Forms
{
    public partial class ClientsForm : Form
    {
        private List<Client> clients;

        public ClientsForm()
        {
            InitializeComponent();
            LoadData();
        }

        // Загрузка данных из базы данных с параметризированным запросом
        private void LoadData()
        {
            try
            {
                string query = "SELECT id, name, contact_info FROM clients;";
                DataTable dataTable = DatabaseManager.ExecuteQuery(query);

                clients = new List<Client>();
                foreach (DataRow row in dataTable.Rows)
                {
                    clients.Add(new Client
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Name = row["name"].ToString(),
                        ContactInfo = row["contact_info"].ToString()
                    });
                }

                UpdateDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных из базы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Обновление DataGridView
        private void UpdateDataGridView()
        {
            dataGridViewClients.DataSource = null;
            dataGridViewClients.DataSource = clients;

            if (dataGridViewClients.Columns.Count > 0)
            {
                dataGridViewClients.Columns["Id"].HeaderText = "ID";
                dataGridViewClients.Columns["Name"].HeaderText = "Имя";
                dataGridViewClients.Columns["ContactInfo"].HeaderText = "Контактная информация";
                dataGridViewClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewClients.Columns["Id"].ReadOnly = true;
            }
        }

        // Добавление нового клиента
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ClientEditForm editForm = new ClientEditForm();
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    string command = "INSERT INTO clients (name, contact_info) VALUES (@name, @contact_info);";
                    var parameters = new Dictionary<string, object>
                    {
                        { "@name", editForm.Client.Name },
                        { "@contact_info", editForm.Client.ContactInfo }
                    };
                    DatabaseManager.ExecuteNonQuery(command, parameters);

                    LoadData();
                    MessageBox.Show("Клиент успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Редактирование клиента
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewClients.SelectedRows.Count > 0)
                {
                    int index = dataGridViewClients.SelectedRows[0].Index;
                    Client selectedClient = clients[index];

                    ClientEditForm editForm = new ClientEditForm(selectedClient);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        string command = "UPDATE clients SET name = @name, contact_info = @contact_info WHERE id = @id;";
                        var parameters = new Dictionary<string, object>
                        {
                            { "@id", selectedClient.Id },
                            { "@name", editForm.Client.Name },
                            { "@contact_info", editForm.Client.ContactInfo }
                        };
                        DatabaseManager.ExecuteNonQuery(command, parameters);

                        LoadData();
                        MessageBox.Show("Клиент успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите клиента для редактирования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании клиента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Удаление клиента
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewClients.SelectedRows.Count > 0)
                {
                    int index = dataGridViewClients.SelectedRows[0].Index;
                    Client selectedClient = clients[index];

                    var result = MessageBox.Show("Вы уверены, что хотите удалить выбранного клиента?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string command = "DELETE FROM clients WHERE id = @id;";
                        var parameters = new Dictionary<string, object> { { "@id", selectedClient.Id } };
                        DatabaseManager.ExecuteNonQuery(command, parameters);

                        LoadData();
                        MessageBox.Show("Клиент успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите клиента для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
