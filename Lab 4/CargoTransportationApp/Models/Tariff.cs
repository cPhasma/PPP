using System;

namespace CargoTransportationApp.Models
{
    [Serializable]
    public class Tariff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double PricePerUnit { get; set; }

        // Конструктор без параметров для сериализации
        public Tariff() { }

        // Конструктор с параметрами
        public Tariff(int id, string name, double pricePerUnit)
        {
            Id = id;
            Name = name;
            PricePerUnit = pricePerUnit;
        }

        // Переопределение ToString для корректного отображения названия тарифа
        public override string ToString()
        {
            return Name; // Возвращает название тарифа
        }
    }
}
