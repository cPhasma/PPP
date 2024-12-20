using System;
using System.Windows.Forms;
using CargoTransportationApp.Models;

namespace CargoTransportationApp.Forms
{
    public partial class ClientEditForm : Form
    {
        public Client Client { get; private set; }

        public ClientEditForm()
        {
            InitializeComponent();
        }

        public ClientEditForm(Client client) : this()
        {
            textBoxName.Text = client.Name;
            textBoxContactInfo.Text = client.ContactInfo;
            Client = client;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("Введите имя клиента.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxContactInfo.Text))
            {
                MessageBox.Show("Введите контактную информацию.");
                return;
            }

            Client = new Client
            {
                Id = 0, // ID будет назначен позже
                Name = textBoxName.Text,
                ContactInfo = textBoxContactInfo.Text
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
