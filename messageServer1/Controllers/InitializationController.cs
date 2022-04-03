using messageServer1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace messageServer1.Controllers
{
    /// <summary>
    /// Контроллер для инициализации списка пользователей и сообщений. 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InitializationController : Controller
    {
        /// <summary>
        /// Создание списка пользователей.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        private List<User> CreateListUsers()
        {
            List<User> users = new List<User>();
            int countOfUsers = Generation.random.Next(1, 50);
            for (int i = 0; i < countOfUsers; i++)
            {
                var user = new User
                {
                    UserName = Generation.GenerateUserName(),
                    Email = Generation.GenerateEmail()
                };
                if (!users.Contains(user))
                {
                    users.Add(user);
                }
            }
            return users;
        }

        /// <summary>
        /// Создание списка сообщений.
        /// </summary>
        /// <param name="countOfUsers">Количество пользователей.</param>
        /// <param name="users">Лист пользователей.</param>
        /// <returns>Лист сообщений.</returns>
        private List<MessageClass> CreateListMessages(int countOfUsers, List<User> users)
        {
            List<MessageClass> messages = new List<MessageClass>();
            messages = new List<MessageClass>();
            int countOfMessages = Generation.random.Next(1, 50);
            for (int i = 0; i < countOfMessages; i++)
            {
                messages.Add(new MessageClass
                {
                    Subject = Generation.GenerateSubject(),
                    Message = Generation.GenerateMessage(),
                    SenderId = users[Generation.random.Next(countOfUsers)].Email,
                    ReceiverId = users[Generation.random.Next(countOfUsers)].Email
                });
            }
            return messages;
        }

        /// <summary>
        /// Инициализация списка пользователей и сообщений.
        /// </summary>
        /// <returns>Статус ответа.</returns>
        [HttpPost("InitializePost")]
        public IActionResult InitializePost()
        {
            var users = CreateListUsers();
            var messages = CreateListMessages(users.Count, users);
            users.Sort();
            try
            {
                WorkWithJson.WriteToJson(users, "users.json");
                WorkWithJson.WriteToJson(messages, "messages.json");
            }
            catch
            {
                return BadRequest("Ошибка при записи в файл.Возможно файлы открыты сторонней программой.\n" +
                    "Закройте файл и попробуйте снова.");
            }
            return Ok();

        }
    }
}
