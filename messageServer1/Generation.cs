using System;

namespace messageServer1
{
    /// <summary>
    /// Рандомная генерация.
    /// </summary>
    public static class Generation
    {
        /// <summary>
        /// Рандом-генератор.
        /// </summary>
        public static Random random = new();
        
        /// <summary>
        /// Алфавит.
        /// </summary>
        public static readonly string alfabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMN" +
            "OPQRSTUVWXYZ0123456789";
        
        /// <summary>
        /// Генерация текста сообщения.
        /// </summary>
        /// <returns>Текст сообщения.</returns>
        public static string GenerateMessage()
        {
            string message = "";
            for (int i = 0; i < random.Next(1, 51); i++)
            {
                message += alfabet[random.Next(alfabet.Length)];
            }
            return message;
        }

        /// <summary>
        /// Генерация темы сообщения.
        /// </summary>
        /// <returns>Тема сообщения.</returns>
        public static string GenerateSubject()
        {
            string subject = "";
            for (int i = 0; i < random.Next(4, 15); i++)
            {
                subject += alfabet[random.Next(alfabet.Length - 10)];
            }
            return subject;
        }

        /// <summary>
        /// Генерация имени пользователя.
        /// </summary>
        /// <returns>Имя пользователя.</returns>
        public static string GenerateUserName()
        {
            string userName = "";
            for (int i = 0; i < 8; i++)
            {
                userName += alfabet[random.Next(alfabet.Length)];
            }
            return userName;
        }

        /// <summary>
        /// Генерация почты пользователя.
        /// </summary>
        /// <returns>Почта.</returns>
        public static string GenerateEmail()
        {
            string email = "";
            for (int i = 0; i < random.Next(8, 16); i++)
            {
                email += alfabet[random.Next(alfabet.Length)];
            }
            return email + "@gogogo.com";
        }
    }

}

