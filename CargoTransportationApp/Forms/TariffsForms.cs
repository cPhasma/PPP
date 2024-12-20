using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using CargoTransportationApp.Models;
using CargoTransportationApp.Utils;

namespace CargoTransportationApp.Forms
{
    public partial class TariffsForm : Form
    {
        private List<Tariff> tariffs;

        public TariffsForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string query = "SELECT id, name, price_per_unit FROM tariffs;";
                DataTable dataTable = DatabaseManager.ExecuteQuery(query);

                tariffs = new List<Tariff>();
                foreach (DataRow row in dataTable.Rows)
                {
                    tariffs.Add(new Tariff
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Name = row["name"].ToString(),
                        PricePerUnit = Convert.ToDouble(row["price_per_unit"])
                    });
                }

                UpdateDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке тарифов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tariffs = new List<Tariff>();
            }
        }

        private void UpdateDataGridView()
        {
            dataGridViewTariffs.DataSource = null;
            dataGridViewTariffs.DataSource = tariffs;

            if (dataGridViewTariffs.Columns.Count > 0)
            {
                dataGridViewTariffs.Columns["Id"].HeaderText = "ID";
                dataGridViewTariffs.Columns["Name"].HeaderText = "Название";
                dataGridViewTariffs.Columns["PricePerUnit"].HeaderText = "Цена за единицу";
                dataGridViewTariffs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Можно сделать столбец Id только для чтения, если нужно.
                dataGridViewTariffs.Columns["Id"].ReadOnly = true;
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                TariffEditForm editForm = new TariffEditForm();
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    string command = "INSERT INTO tariffs (name, price_per_unit) VALUES (@name, @price);";
                    var parameters = new Dictionary<string, object>
                    {
                        { "@name", editForm.Tariff.Name },
                        { "@price", editForm.Tariff.PricePerUnit }
                    };

                    DatabaseManager.ExecuteNonQuery(command, parameters);

                    LoadData();
                    MessageBox.Show("Тариф успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении тарифа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTariffs.SelectedRows.Count > 0)
                {
                    int index = dataGridViewTariffs.SelectedRows[0].Index;
                    Tariff selectedTariff = tariffs[index];

                    TariffEditForm editForm = new TariffEditForm(selectedTariff);
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        string command = "UPDATE tariffs SET name = @name, price_per_unit = @price WHERE id = @id;";
                        var parameters = new Dictionary<string, object>
                        {
                            { "@id", selectedTariff.Id },
                            { "@name", editForm.Tariff.Name },
                            { "@price", editForm.Tariff.PricePerUnit }
                        };

                        DatabaseManager.ExecuteNonQuery(command, parameters);

                        LoadData();
                        MessageBox.Show("Тариф успешно обновлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите тариф для редактирования.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании тарифа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTariffs.SelectedRows.Count > 0)
                {
                    int index = dataGridViewTariffs.SelectedRows[0].Index;
                    Tariff selectedTariff = tariffs[index];

                    var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный тариф?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string command = "DELETE FROM tariffs WHERE id = @id;";
                        var parameters = new Dictionary<string, object>
                        {
                            { "@id", selectedTariff.Id }
                        };

                        DatabaseManager.ExecuteNonQuery(command, parameters);

                        LoadData();
                        MessageBox.Show("Тариф успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите тариф для удаления.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении тарифа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Если обработчик FormClosing не нужен, удалите его из дизайнера и код здесь.
        private void TariffsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // При работе с базой нет необходимости сохранять что-то при закрытии,
            // т.к. данные сохраняются сразу при изменении.
            // Если вы хотите что-то особенное — добавьте здесь код.
            // Иначе удалите подписку на событие FormClosing в дизайнере.
        }
    }
}
