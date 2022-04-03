using System;
using System.Runtime.Serialization;

namespace messageServer1.Models
{
    /// <summary>
    /// Модель, описывающая пользователя.
    /// </summary>
    [DataContract,KnownType(typeof(User))]
    public class User:IComparable
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        
        /// <summary>
        /// Почта.
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// Переопределенный метод для сравнения двух пользователей.
        /// </summary>
        /// <param name="obj">Второй пользователь.</param>
        /// <returns>1 - если первый больше,-1 - второй больше. При равенстве - 0.</returns>
        public int CompareTo(object obj)
        {
            var otherUser = obj as User;
            return Email.CompareTo(otherUser.Email);
        }

        /// <summary>
        /// Проверка на равенство двух пользователей.
        /// </summary>
        /// <param name="obj">Второй пользователь.</param>
        /// <returns>True если равны, иначе False.</returns>
        public override bool Equals(object obj)
        {
            var otherUser = obj as User;
            return otherUser.Email == Email;  
        }

        /// <summary>
        /// Предоставляет хеш-код пользователя.
        /// </summary>
        /// <returns>Хеш-код.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
