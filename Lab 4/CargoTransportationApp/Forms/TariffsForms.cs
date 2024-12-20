using System;
using System.Collections.Generic;
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

        // Загрузка данных из DataManager
        private void LoadData()
        {
            try
            {
                tariffs = DataManager.LoadTariffs();
                UpdateDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке тарифов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tariffs = new List<Tariff>();
            }
        }

        // Обновление DataGridView
        private void UpdateDataGridView()
        {
            dataGridViewTariffs.DataSource = null;
            dataGridViewTariffs.DataSource = tariffs;

            // Настройка заголовков столбцов
            if (dataGridViewTariffs.Columns.Count > 0)
            {
                dataGridViewTariffs.Columns["Id"].HeaderText = "ID";
                dataGridViewTariffs.Columns["Name"].HeaderText = "Название";
                dataGridViewTariffs.Columns["PricePerUnit"].HeaderText = "Цена за единицу";
                dataGridViewTariffs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }

        // Добавление нового тарифа
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                TariffEditForm editForm = new TariffEditForm();
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    // Генерация уникального ID
                    int newId = tariffs.Count > 0 ? tariffs[tariffs.Count - 1].Id + 1 : 1;
                    editForm.Tariff.Id = newId;

                    tariffs.Add(editForm.Tariff);
                    DataManager.SaveTariffs(tariffs); // Сохранение данных
                    UpdateDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении тарифа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Редактирование выбранного тарифа
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
                        tariffs[index] = editForm.Tariff;
                        DataManager.SaveTariffs(tariffs); // Сохранение данных
                        UpdateDataGridView();
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

        // Удаление выбранного тарифа
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTariffs.SelectedRows.Count > 0)
                {
                    int index = dataGridViewTariffs.SelectedRows[0].Index;

                    var result = MessageBox.Show("Вы уверены, что хотите удалить выбранный тариф?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        tariffs.RemoveAt(index);
                        DataManager.SaveTariffs(tariffs); // Сохранение данных
                        UpdateDataGridView();
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

        // Сохранение данных при закрытии формы
        private void TariffsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DataManager.SaveTariffs(tariffs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении тарифов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
