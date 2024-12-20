using System;

namespace CargoTransportationApp.Models
{
    [Serializable]
    public class Order
    {
        public int Id { get; set; }
        public Client Client { get; set; }
        public Tariff Tariff { get; set; }
        public double Volume { get; set; }

        // Свойство для расчета общей стоимости на основе тарифа и объема
        public double TotalCost => Tariff != null ? Volume * Tariff.PricePerUnit : 0;

        // Конструктор без параметров (необходим для сериализации)
        public Order() { }

        // Конструктор с параметрами
        public Order(int id, Client client, Tariff tariff, double volume)
        {
            Id = id;
            Client = client;
            Tariff = tariff;
            Volume = volume;
        }

        // Метод для расчета общей стоимости (оставлено для вызовов вручную, если нужно)
        public void CalculateTotalCost()
        {
            // Метод не обязателен, так как есть свойство TotalCost, но можно оставить
        }
    }
}
