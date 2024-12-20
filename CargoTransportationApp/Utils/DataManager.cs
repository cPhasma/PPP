using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CargoTransportationApp.Models;

namespace CargoTransportationApp.Utils
{
    public static class DataManager
    {
        private static readonly string TariffsFile = "tariffs.dat";
        private static readonly string ClientsFile = "clients.dat";
        private static readonly string OrdersFile = "orders.dat";

        // Метод для сохранения тарифов
        public static void SaveTariffs(List<Tariff> tariffs)
        {
            SaveData(tariffs, TariffsFile, "тарифов");
        }

        // Метод для загрузки тарифов
        public static List<Tariff> LoadTariffs()
        {
            return LoadData<Tariff>(TariffsFile, "тарифов");
        }

        // Метод для сохранения клиентов
        public static void SaveClients(List<Client> clients)
        {
            SaveData(clients, ClientsFile, "клиентов");
        }

        // Метод для загрузки клиентов
        public static List<Client> LoadClients()
        {
            return LoadData<Client>(ClientsFile, "клиентов");
        }

        // Метод для сохранения заказов
        public static void SaveOrders(List<Order> orders)
        {
            SaveData(orders, OrdersFile, "заказов");
        }

        // Метод для загрузки заказов
        public static List<Order> LoadOrders()
        {
            return LoadData<Order>(OrdersFile, "заказов");
        }

        // Универсальный метод для сохранения данных
        private static void SaveData<T>(List<T> data, string filePath, string dataType)
        {
            try
            {
                // Создаем резервную копию файла, если он существует
                if (File.Exists(filePath))
                {
                    File.Copy(filePath, $"{filePath}.bak", true);
                }

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, data);
                }

                Console.WriteLine($"Данные {dataType} успешно сохранены в файл {filePath}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении {dataType}: {ex.Message}");
            }
        }

        // Универсальный метод для загрузки данных
        private static List<T> LoadData<T>(string filePath, string dataType)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return (List<T>)formatter.Deserialize(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке {dataType}: {ex.Message}");
            }

            Console.WriteLine($"Данные {dataType} не найдены, возвращаем пустой список.");
            return new List<T>(); // Возвращаем пустой список при ошибке
        }
    }
}
