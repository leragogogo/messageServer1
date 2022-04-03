using System.Runtime.Serialization;

namespace messageServer1.Models
{
    /// <summary>
    /// Модель описывающая сообщения.
    /// </summary>
    [DataContract,KnownType(typeof(MessageClass))]
    public class MessageClass
    {
        /// <summary>
        /// Тема сообщения.
        /// </summary>
        [DataMember]
        public string Subject { get; set; }
        
        /// <summary>
        /// Текст сообщения.
        /// </summary>
        [DataMember]
        public string Message { get; set; }
        
        /// <summary>
        /// Почта отправителя.
        /// </summary>
        [DataMember]
        public string SenderId { get; set; }
        
        /// <summary>
        /// Почта получателя.
        /// </summary>
        [DataMember]
        public string ReceiverId { get; set; }
    }
}
