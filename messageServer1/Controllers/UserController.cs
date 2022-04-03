using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using messageServer1.Models;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Json;

namespace messageServer1.Controllers
{
    /// <summary>
    /// Контроллер для работы со списком пользователей.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private const string userPath = "users.json";


        /// <summary>
        /// Проверка строки, что все ее символы это цифры 
        /// или строчные,заглавные буквы латинского алфавита.
        /// </summary>
        /// <param name="str">Строка</param>
        /// <returns>true - если строка корректна, иначе false.</returns>
        private bool OnlySymbolsFromAlfabet(string str)
        {
            foreach (var s in str)
            {
                if (!Generation.alfabet.Contains(s))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Проверка на то, что почта заканчивается на @gogogo.com
        /// </summary>
        /// <param name="email">Почта.</param>
        /// <returns>true - если почта корректна, иначе false.</returns>
        private bool CheckEmail(string email)
        {
            var regex = new Regex(@"\w+@gogogo\.com$");
            if (!regex.IsMatch(email))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Регистрация нового пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="email">Почта.</param>
        /// <returns>Код ответа.</returns>
        [HttpPost("RegistrationNewUser/{userName},{email}")]
        public IActionResult RegistrationPost(string userName, string email)
        {
            if(!OnlySymbolsFromAlfabet(userName) || (email.Length <= 11) || !OnlySymbolsFromAlfabet(email.Substring(0,email.Length-11)))
            {
                return BadRequest("Имя пользователя и почта должны содержать только цифры,\n" +
                    "строчные и заглавные буквы латинского алфавита.\n" +
                    "Кроме того, почта должна заканчиваться на @gogogo.com");
            }

            if (!CheckEmail(email))
            {
                return BadRequest("Некорректная почта.Она должна заканчиваться на @gogogo.com");
            }
            
            var newUser = new User
            {
                UserName = userName,
                Email = email
            };

            var jsonResult = WorkWithJson.ReadFromJson<User>(userPath);
            if (jsonResult.Item1 == false)
            {
                return BadRequest(jsonResult.Item2);
            }
            var users = jsonResult.Item3;

            // Проверка, что есть ли уже пользователь с такой почтой.
            if (users.Contains(newUser))
            {
                return BadRequest("Пользователь с такой почтой уже зарегистрирован.");
            }

            users.Add(newUser);
            users.Sort();

            try
            {
                WorkWithJson.WriteToJson(users, userPath);
            }
            catch
            {
                return BadRequest("Ошибка при записи в файл.Возможно файл открыт сторонней программой.\n" +
                    "Закройте файл и попробуйте снова.");
            }
            return Ok();
        }

        /// <summary>
        /// Возврат всех пользователей.
        /// </summary>
        /// <returns>Код ответа и список пользователей.</returns>
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var jsonResult = WorkWithJson.ReadFromJson<User>(userPath);
            if (jsonResult.Item1 == false)
            {
                return BadRequest(jsonResult.Item2);
            }
            var users = jsonResult.Item3;
            if (users == null)
            {
                return NotFound("Список пользователей пуст.");
            }
            return Ok(users);
        }


        /// <summary>
        /// Возврат пользователя по его почте.
        /// </summary>
        /// <param name="email">Почта.</param>
        /// <returns>Код ответа и пользователь.</returns>
        [HttpGet("GetUserByEmail/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var jsonResult = WorkWithJson.ReadFromJson<User>(userPath);
            if (jsonResult.Item1 == false)
            {
                return BadRequest(jsonResult.Item2);
            }
            var users = jsonResult.Item3;

            User answer = null;
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Email == email)
                {
                    answer = users[i];
                }
            }
            if (answer == null)
            {
                return NotFound($"Пользователь с почтой {email} не зарегистрирован.");
            }
            return Ok(answer);
        }
    }
}

