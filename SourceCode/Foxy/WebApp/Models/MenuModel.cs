using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class MenuModel
    {
        public string Username { get; set; }
        public int Level { get; set; }
        public int LessonNumber { get; set; }
        public int ReviewNumber { get; set; }
        public int TotalNrLevels { get; set; }
    }
}
