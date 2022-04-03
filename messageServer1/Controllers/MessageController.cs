using messageServer1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace messageServer1.Controllers
{
    /// <summary>
    /// Контроллер для работы со списком сообщений.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MessageController : Controller
    {
        private const string messagePath = "messages.json";
        
        /// <summary>
        /// Проверка что пользователь с такой почтой зарегистрирован.
        /// </summary>
        /// <param name="email">Почта пользователя.</param>
        /// <param name="users">Список пользователей.</param>
        /// <returns>true -  если зарегистрирован, иначе false.</returns>
        private bool ExistUserWithEmail(string email,List<User> users)
        {
            int count = 0;
            foreach (var user in users)
            {
                if (user.Email == email)
                {
                    count++;
                }
            }
            if (count == 0) 
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Отправление нового сообщения.
        /// </summary>
        /// <param name="subject">Тема сообщения.</param>
        /// <param name="message">Текст сообщения.</param>
        /// <param name="senderId">Почта отправителя.</param>
        /// <param name="receiverId">Почта получателя.</param>
        /// <returns>Код ответа.</returns>
        [HttpPost("SendNewMessage/{subject},{message},{senderId},{receiverId}")]
        public IActionResult SendNewMessage(string subject,string message,string senderId,string receiverId)
        {
            var userJsonResult = WorkWithJson.ReadFromJson<User>("users.json");
            if(userJsonResult.Item1 == false)
            {
                return BadRequest(userJsonResult.Item2);
            }
            var users = userJsonResult.Item3;
            
            if ((!ExistUserWithEmail(senderId,users)) || (!ExistUserWithEmail(receiverId,users)))
            {
                return BadRequest("Отправителя или получателя с такой почтой не существует.");
            }

            var newMessage = new MessageClass{
                Subject = subject,
                Message = message,
                SenderId = senderId,
                ReceiverId = receiverId
            };
            var messagesJsonResult = WorkWithJson.ReadFromJson<MessageClass>(messagePath);
            if(messagesJsonResult.Item1 == false)
            {
                return BadRequest(messagesJsonResult.Item2);
            }
            var messages = messagesJsonResult.Item3;
            messages.Add(newMessage);
            
            try
            {
                WorkWithJson.WriteToJson(messages, messagePath);
            }
            catch
            {
                return BadRequest("Ошибка при записи в файл.Возможно файл открыт сторонней программой.\n" +
                    "Закройте файл и попробуйте снова.");
            }
            return Ok();
        }

        /// <summary>
        /// Возврат сообщений по почте отправителя и получателя.
        /// </summary>
        /// <param name="senderId">Почта отправителя.</param>
        /// <param name="receiverId">Почта получателя.</param>
        /// <returns>Код ответа и список сообщений.</returns>
        [HttpGet("GetMessagesBySenderIdAndReceiverId/{senderId},{receiverId}")]
        public IActionResult GetMessagesBySenderIdAndReceiverId(string senderId, string receiverId)
        {
            var messagesJsonResult = WorkWithJson.ReadFromJson<MessageClass>(messagePath);
            if (messagesJsonResult.Item1 == false)
            {
                return BadRequest(messagesJsonResult.Item2);
            }
            var messages = messagesJsonResult.Item3;

            var userJsonResult = WorkWithJson.ReadFromJson<User>("users.json");
            if (userJsonResult.Item1 == false)
            {
                return BadRequest(userJsonResult.Item2);
            }
            var users = userJsonResult.Item3;

            if(!ExistUserWithEmail(senderId,users))
            {
                return BadRequest($"Отправитель с почтой {senderId} не зарегистрирован.");
            }
            if (!ExistUserWithEmail(receiverId, users))
            {
                return BadRequest($"Получатель с почтой {receiverId} не зарегистрирован.");
            }

            List<MessageClass> newMessages = new();
            for (int i = 0; i < messages.Count; i++)
            {
                if ((messages[i].SenderId == senderId) && (messages[i].ReceiverId == receiverId))
                {
                    newMessages.Add(messages[i]);
                }
            }
            return Ok(newMessages);
        }

        /// <summary>
        /// Возврат сообщений по почте отправителя.
        /// </summary>
        /// <param name="senderId">Почта отправителя.</param>
        /// <returns>Код ответа и список сообщений.</returns>
        [HttpGet("GetMessagesBySenderId/{senderId}")]
        public IActionResult GetMessagesBySenderId(string senderId)
        {
            var messagesJsonResult = WorkWithJson.ReadFromJson<MessageClass>(messagePath);
            if (messagesJsonResult.Item1 == false)
            {
                return BadRequest(messagesJsonResult.Item2);
            }
            var messages = messagesJsonResult.Item3;

            var userJsonResult = WorkWithJson.ReadFromJson<User>("users.json");
            if (userJsonResult.Item1 == false)
            {
                return BadRequest(userJsonResult.Item2);
            }
            var users = userJsonResult.Item3;

            if (!ExistUserWithEmail(senderId, users))
            {
                return BadRequest($"Отправитель с почтой {senderId} не зарегистрирован.");
            }
            List<MessageClass> newMessages = new();
            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i].SenderId == senderId)
                {
                    newMessages.Add(messages[i]);
                }
            }
            return Ok(newMessages);
        }

        /// <summary>
        /// Возврат сообщений по почте получателя.
        /// </summary>
        /// <param name="receiverId">Почта получателя.</param>
        /// <returns>Код ответа и список сообщений.</returns>
        [HttpGet("GetMessagesByReceiverId/{receiverId}")]
        public IActionResult GetMessagesByReceiverId(string receiverId)
        {
            var messagesJsonResult = WorkWithJson.ReadFromJson<MessageClass>(messagePath);
            if (messagesJsonResult.Item1 == false)
            {
                return BadRequest(messagesJsonResult.Item2);
            }
            var messages = messagesJsonResult.Item3;

            var userJsonResult = WorkWithJson.ReadFromJson<User>("users.json");
            if (userJsonResult.Item1 == false)
            {
                return BadRequest(userJsonResult.Item2);
            }
            var users = userJsonResult.Item3;
            
            if (!ExistUserWithEmail(receiverId, users))
            {
                return BadRequest($"Получатель с почтой {receiverId} не зарегистрирован.");
            }

            List<MessageClass> newMessages = new();
            for (int i = 0; i < messages.Count; i++)
            {
                if (messages[i].ReceiverId == receiverId)
                {
                    newMessages.Add(messages[i]);
                }
            }
            return Ok(newMessages);
        }
    }
}
