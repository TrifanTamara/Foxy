using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DTOs
{
    public class ListQuestionAnsDto
    {
        public List<QuestionAnsDto> ListQ { get; set; }
        public String FormularId { get; set; }
    }
}
