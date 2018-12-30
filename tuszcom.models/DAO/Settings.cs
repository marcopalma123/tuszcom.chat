using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tuszcom.models.DAO
{
    public class Settings
    {
        [Key]
        public int IdSetting { get; set; }
        public string Group { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool AllowDelete { get; set; }
        public string Description { get; set; }
    }
}
