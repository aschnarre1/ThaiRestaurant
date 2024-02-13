﻿using MySqlConnector;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
using Microsoft.AspNetCore.Http; 

using ThaiRestaurant.Models;

namespace ThaiRestaurant.Data
{
    public class DatabaseContext
    {
        private readonly string _connectionString;
        private readonly ImageUploadService _imageUploadService;

        public DatabaseContext(IConfiguration configuration, ImageUploadService imageUploadService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _imageUploadService = imageUploadService;
        }


        public List<Dish> GetDishes()
        {
            var dishes = new List<Dish>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM dish", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dish = new Dish
                            {
                                DishId = reader.GetInt32("DishId"),
                                Name = reader.GetString("Name"),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                                Price = reader.GetDecimal("Price"),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString("ImageUrl"),
                            };
                            dishes.Add(dish);
                        }
                    }
                }
            }
            return dishes;
        }







        public List<Message> GetMessages()
        {
            var messages = new List<Message>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM message", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var message = new Message
                            {
                                MessageId = reader.GetInt32("MessageId"),
                                Name = reader.GetString("Name"),
                                Email = reader.GetString("Email"),
                                MessageText = reader.GetString("MessageText")

                            };
                            messages.Add(message);
                        }
                    }
                }
            }
            return messages;
        }




        public List<User> GetUsers()
        {
            var users = new List<User>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM user", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                UserId = reader.GetInt32("UserId"),
                                UserName = reader.GetString("UserName"),
                                Email = reader.GetString("Email"),

                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }








        public void AddDish(Dish dish, IFormFile imageFile)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string imageUrl = _imageUploadService.UploadImage(imageFile);

                var query = "INSERT INTO dish (Name, Description, Price, ImageUrl) VALUES (@Name, @Description, @Price, @ImageUrl)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", dish.Name);
                    command.Parameters.AddWithValue("@Description", dish.Description);
                    command.Parameters.AddWithValue("@Price", dish.Price);
                    command.Parameters.AddWithValue("@ImageUrl", imageUrl);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void EditDish(Dish dish)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE dish SET Name = @Name, Description = @Description, Price = @Price, ImageUrl = @ImageUrl WHERE DishId = @DishId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", dish.Name);
                    command.Parameters.AddWithValue("@Description", dish.Description);
                    command.Parameters.AddWithValue("@Price", dish.Price);
                    command.Parameters.AddWithValue("@ImageUrl", dish.ImageUrl);
                    command.Parameters.AddWithValue("@DishId", dish.DishId);

                    command.ExecuteNonQuery();
                }
            }
        }


        public Dish GetDishById(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM dish WHERE DishId = @DishId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DishId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Dish
                            {
                                DishId = reader.GetInt32("DishId"),
                                Name = reader.GetString("Name"),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString("Description"),
                                Price = reader.GetDecimal("Price"),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString("ImageUrl"),
                            };
                        }
                    }
                }
            }
            return null;
        }




        public User GetUserById(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM user WHERE UserId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32("UserId"),
                                UserName = reader.GetString("UserName"),
                                Email = reader.GetString("Email"),
                                Password = reader.GetString("Password"),
                            };
                        }
                    }
                }
            }
            return null;
        }




        public Message GetMessageById(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM message WHERE MessageId = @MessageId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MessageId", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Message
                            {
                                MessageId = reader.GetInt32("MessageId"),
                                Name = reader.GetString("Name"),
                                Email = reader.GetString("Email"),
                                MessageText = reader.GetString("MessageText"),
                            };
                        }
                    }
                }
            }
            return null;
        }



        public void DeleteUser(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM user WHERE UserId = @UserId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", id);
                    command.ExecuteNonQuery();
                }
            }
        }



        public void DeleteDish(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM dish WHERE DishId = @DishId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DishId", id);
                    command.ExecuteNonQuery();
                }
            }
        }




        public void CreateMessage(Message message)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO message (Name, Email, MessageText) VALUES (@Name, @Email, @MessageText)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", message.Name);
                    command.Parameters.AddWithValue("@MessageText", message.MessageText);
                    command.Parameters.AddWithValue("@Email", message.Email);

                    command.ExecuteNonQuery();
                }
            }
        }


        public void DeleteMessage(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "DELETE FROM message WHERE MessageId = @MessageId";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MessageId", id);
                    command.ExecuteNonQuery();
                }
            }
        }


        public void AddUser(User user)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO user (UserName, Email, Password) VALUES (@UserName, @Email, @Password)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", user.UserName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    command.Parameters.AddWithValue("@Password", hashedPassword);

                    command.ExecuteNonQuery();
                }
            }
        }

        public User LoginUser(string username, string password)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM user WHERE UserName = @UserName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", username);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var storedPassword = reader.GetString("Password");
                            if (BCrypt.Net.BCrypt.Verify(password, storedPassword))
                            {
                                return new User
                                {
                                    UserId = reader.GetInt32("UserId"),
                                    UserName = reader.GetString("UserName"),
                                    Email = reader.GetString("Email"),
                                };
                            }
                        }
                    }
                    
                }
            }
            return null; 
        }


    }
}
