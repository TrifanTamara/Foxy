﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.DTOs
{
    public class UpdateNoteDto
    {
        public Guid ElementId { get; set; }
        public string NewContent { get; set; }
    }
}
