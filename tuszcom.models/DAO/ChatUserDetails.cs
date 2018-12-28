using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tuszcom.models.DAO
{
    public class ChatUserDetails
    {
        [Key]
        public int IdChatUserDetail { get; set; }
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
    }
}
