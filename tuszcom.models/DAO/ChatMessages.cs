using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tuszcom.models.DAO
{
    public class ChatMessages
    {
        [Key]
        public int IdChatMessage { get; set; }
        public string SenderUserId { get; set; }
        public string CustomerUserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Date { get; set; }
        public string ConnectionId { get; set; }
    }
}
