using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tuszcom.models.DAO
{
    public class ViewMessages
    {
        [Key]
        public int IdChatMessage { get; set; }
        public string SenderUserId { get; set; }
        public string CustomerUserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Date { get; set; }
        public string ConnectionId { get; set; }
        public string senderUsername { get; set; }
        public string senderEmail { get; set; }
        public string senderFirstname { get; set; }
        public string senderSurname { get; set; }
        public string customerUsername { get; set; }
        public string customerEmail { get; set; }
        public string customerirstname { get; set; }
        public string customerSurname { get; set; }
        public DateTime customerRegistrationDate { get; set; }
        public DateTime senderRegistrationDate { get; set; }
    }
}
