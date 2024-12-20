using System;
using System.Windows.Forms;
using CargoTransportationApp.Models;

namespace CargoTransportationApp.Forms
{
    public partial class TariffEditForm : Form
    {
        public Tariff Tariff { get; private set; }

        // Максимальное значение для цены за единицу
        private const double MaxPricePerUnit = 1000;

        // Конструктор для добавления нового тарифа
        public TariffEditForm()
        {
            InitializeComponent();
        }

        // Конструктор для редактирования существующего тарифа
        public TariffEditForm(Tariff tariff) : this()
        {
            Tariff = tariff;
            textBoxName.Text = Tariff.Name;
            textBoxPricePerUnit.Text = Tariff.PricePerUnit.ToString();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Проверка названия тарифа
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Введите название тарифа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка корректности ввода цены
            if (!double.TryParse(textBoxPricePerUnit.Text, out double pricePerUnit))
            {
                MessageBox.Show("Введите корректную цену за единицу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка, что цена положительная
            if (pricePerUnit <= 0)
            {
                MessageBox.Show("Цена за единицу должна быть положительным числом.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка, что цена не превышает максимального значения
            if (pricePerUnit > MaxPricePerUnit)
            {
                MessageBox.Show($"Цена за единицу не должна превышать {MaxPricePerUnit}.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Tariff == null)
            {
                // Создание нового тарифа
                Tariff = new Tariff
                {
                    Id = 0, // ID будет присвоен позже
                    Name = textBoxName.Text,
                    PricePerUnit = pricePerUnit
                };
            }
            else
            {
                // Обновление существующего тарифа
                Tariff.Name = textBoxName.Text;
                Tariff.PricePerUnit = pricePerUnit;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
