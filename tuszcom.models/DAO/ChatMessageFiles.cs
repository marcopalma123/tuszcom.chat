using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tuszcom.models.DAO
{
    public class ChatMessageFiles
    {
        [Key]
        public int IdChatMessageFile { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public int ChatMessageId { get; set; }
    }
}
