using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tuszcom.models.DAO
{
    public class ErrorLogs
    {
        [Key]
        public int IdError { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
    }
}
