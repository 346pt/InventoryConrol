using System;
using System.Data.SQLite;
using System.Linq.Expressions;

namespace AnanasWarehouse
{
    class Program
    {

        const string DatabasePath = "warehouse.db";

        static void Main(string[] args)
        {
            string choice1;
            while (true)
            {
                Console.WriteLine("Выберите: Руководитель или Работник");
                choice1 = Console.ReadLine();
                if (choice1 == "Руководитель" || choice1 == "Работник") ;
                break;
            }
                if (choice1 == "Руководитель")
                {
                    Console.Write("Введите пароль: ");
                    string password = Console.ReadLine();
                    string passwordCheck = "ананас";
                    if (password == passwordCheck)
                    {
                        CreateTable();
                    }
                    else
                    {
                        Console.WriteLine("Неправильный пароль, попробуйте снова");
                        return;
                    }
                }

            if (choice1 == "Работник")
            {
                Console.Write("Введите пароль: ");
                string password1 = Console.ReadLine();
                string passwordCheck1 = "ананас";
                if (password1 == passwordCheck1)
                {
                    CreateTable();
                }
                else
                {
                    Console.WriteLine("Неправильный пароль, попробуйте снова");
                    return;
                }
            }
            CreateTable();

            while (true)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Просмотреть товары");
                Console.WriteLine("3. Удалить товар");
                Console.WriteLine("4. Изменение кол-ва товара");
                Console.WriteLine("5. Поиск товара по имени");
                Console.WriteLine("6. Выход");
                Console.Write("Введите номер операции: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        ViewProducts();
                        break;
                    case 3:
                        DeleteProduct();
                        break;
                    case 4:
                        UpdateProductQuantity();
                        break;
                    case 5:
                        FindProducts();
                        break;
                    case 6:
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Ошибка: введен недопустимый номер операции.");
                        break;
                }
            }
        }

        static void CreateTable()
        {
            using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
            {
                connection.Open();

                string createTableQuery = "CREATE TABLE IF NOT EXISTS Products (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Quantity INTEGER, ZAKUP INTEGER, OTPUSK INTEGER);";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        static void AddProduct()
        {
            try
            {


                Console.Write("Введите название товара: ");
                string name = Console.ReadLine();
                Console.Write("Введите количество товара: ");
                int quantity = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите закупочную цену: ");
                decimal zakup = Convert.ToDecimal(Console.ReadLine());
                Console.Write("Введите отпускную цену: ");
                decimal otpusk = Convert.ToDecimal(Console.ReadLine());

                using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
                {
                    connection.Open();

                    string insertQuery = $"INSERT INTO Products (Name, Quantity, Zakup, Otpusk) VALUES ('{name}', {quantity}, {zakup}, {otpusk});";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Товар успешно добавлен.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при добавлении товара: " + ex.Message);
            }
        }
        static void ViewProducts()
        {
            try {
                using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM Products;";
                    using (var command = new SQLiteCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("Список товаров пуст.");
                                return;
                            }

                            Console.WriteLine("Список товаров:");
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["Id"]);
                                string name = reader["Name"].ToString();
                                int quantity = Convert.ToInt32(reader["Quantity"]);
                                decimal zakup = Convert.ToDecimal(reader["Zakup"]);
                                decimal otpusk = Convert.ToDecimal(reader["Otpusk"]);

                                Console.WriteLine($"Id: {id}, Название: {name}, Количество: {quantity}, Закупочная цена: {zakup}, Отпускная цена: {otpusk}");
                            }
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при просмотре товара: " + ex.Message);
            }
        }
        static void FindProducts()
        {
            try { 
            Console.Write("Введите название товара: ");
            string nameGood = Console.ReadLine();

                using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
                {
                    connection.Open();


                    string nameGood1 = $"SELECT* FROM Products WHERE NAME = '{nameGood}';";
                    using (var command = new SQLiteCommand(nameGood1, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("Товар с данным именем отсутсвует");
                                return;
                            }

                            Console.WriteLine("Товар");
                            while (reader.Read())
                            {
                                int id = Convert.ToInt32(reader["Id"]);
                                string name = reader["Name"].ToString();
                                int quantity = Convert.ToInt32(reader["Quantity"]);
                                decimal zakup = Convert.ToDecimal(reader["Zakup"]);
                                decimal otpusk = Convert.ToDecimal(reader["Otpusk"]);

                                Console.WriteLine($"Id: {id}, Название: {name}, Количество: {quantity}, Закупочная цена: {zakup}, Отпускная цена: {otpusk}");
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при поиске товара по названию: " + ex.Message);
            }
        }
        static void DeleteProduct()
        {
        try 
        { 
            Console.Write("Введите ID товара для удаления: ");
            int productId = Convert.ToInt32(Console.ReadLine());

            using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
            {
                connection.Open();

                string deleteQuery = $"DELETE FROM Products WHERE Id = {productId};";
                using (var command = new SQLiteCommand(deleteQuery, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Товар успешно удален.");
                    }
                    else
                    {
                        Console.WriteLine("Товар с указанным ID не найден.");
                    }
                }
            }
        }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при удалении товара " + ex.Message);
            }
        }
        static void UpdateProductQuantity()
        {
            try
            {
                Console.Write("Введите Id товара для изменения количества: ");
                int productId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Введите новое количество товара: ");
                int newQuantity = Convert.ToInt32(Console.ReadLine());

                using (var connection = new SQLiteConnection($"Data Source={DatabasePath};Version=3;"))
                {
                    connection.Open();

                    string updateQuery = $"UPDATE Products SET Quantity = {newQuantity} WHERE Id = {productId};";
                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        int rowsUpdated = command.ExecuteNonQuery();
                        if (rowsUpdated > 0)
                        {
                            Console.WriteLine("Количество товара успешно изменено.");
                        }
                        else
                        {
                            Console.WriteLine("Товар с указанным Id не найден.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла ошибка при обновлении количества товара: " + ex.Message);
            }
        }
    }
}
