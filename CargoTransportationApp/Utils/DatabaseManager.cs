using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace CargoTransportationApp.Utils
{
    public static class DatabaseManager
    {
        private static readonly string ConnectionString = "Host=localhost;Port=5432;Database=myappdb;Username=postgres;Password=1234";

        // Метод для проверки подключения
        public static bool TestConnection()
        {
            try
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    Console.WriteLine("Подключение успешно установлено.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
                return false;
            }
        }

        // Метод для выполнения SELECT-запросов
        public static DataTable ExecuteQuery(string query)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        // Метод для выполнения SQL-команд с параметрами (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string commandText, Dictionary<string, object> parameters = null)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
