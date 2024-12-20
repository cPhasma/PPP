using System;

namespace CargoTransportationApp.Models
{
    [Serializable]
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }

        // Конструктор без параметров для сериализации
        public Client() { }

        // Конструктор с параметрами
        public Client(int id, string name, string contactInfo)
        {
            Id = id;
            Name = name;
            ContactInfo = contactInfo;
        }

        // Переопределение ToString для корректного отображения имени клиента
        public override string ToString()
        {
            return Name; // Возвращает имя клиента
        }
    }
}
