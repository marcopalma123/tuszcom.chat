using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tuszcom.chat.Data
{
    public class ClaimsData
    {
        public static List<string> Claims { get; set; } = new List<string>
        {
            "Panel role",
            "Panel administratora",
            "Zarządzanie użytkownikami",
            "Chat - Wysyłanie plików",
            "Chat - Wysyłanie wiadomości"
        };
    }
}
