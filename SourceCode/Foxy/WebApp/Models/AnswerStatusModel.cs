using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class AnswerStatusModel
    {
        public bool Final { get; set; }
        public bool Meaning{ get; set; }
        public bool Reading { get; set; }
        
        public string LevelName { get; set; }
    }
}
