using System;
using System.Collections.Generic;
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

        // Загрузка данных из DataManager
        private void LoadData()
        {
            try
            {
                clients = DataManager.LoadClients();
                UpdateDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clients = new List<Client>();
            }
        }

        // Обновление DataGridView
        private void UpdateDataGridView()
        {
            dataGridViewClients.DataSource = null;
            dataGridViewClients.DataSource = clients;

            // Настройка столбцов DataGridView
            if (dataGridViewClients.Columns.Count > 0)
            {
                dataGridViewClients.Columns["Id"].HeaderText = "ID";
                dataGridViewClients.Columns["Name"].HeaderText = "Имя";
                dataGridViewClients.Columns["ContactInfo"].HeaderText = "Контактная информация";
                dataGridViewClients.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
                    // Генерация уникального ID
                    int newId = clients.Count > 0 ? clients[clients.Count - 1].Id + 1 : 1;
                    editForm.Client.Id = newId;

                    clients.Add(editForm.Client);
                    DataManager.SaveClients(clients); // Сохранение данных
                    UpdateDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Редактирование выбранного клиента
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
                        clients[index] = editForm.Client;
                        DataManager.SaveClients(clients); // Сохранение данных
                        UpdateDataGridView();
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

        // Удаление выбранного клиента
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewClients.SelectedRows.Count > 0)
                {
                    int index = dataGridViewClients.SelectedRows[0].Index;

                    var result = MessageBox.Show("Вы уверены, что хотите удалить выбранного клиента?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        clients.RemoveAt(index);
                        DataManager.SaveClients(clients); // Сохранение данных
                        UpdateDataGridView();
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

        // Сохранение данных при закрытии формы
        private void ClientsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DataManager.SaveClients(clients);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
