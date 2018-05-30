using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.DTOs
{
    public class RemoveSynVocDto
    {
        public Guid VocabularId { get; set; }
        public int Index { get; set; }
    }
}
