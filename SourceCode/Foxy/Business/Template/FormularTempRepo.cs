﻿using Data.Domain.Entities.TemplateItems;
using Data.Domain.Interfaces;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Template
{
    public class FormularTempRepo :
        GenericRepo<FormularTemplate>, IFormularTempRepo
    {
        private readonly IDatabaseContext _databaseContext;
        private IQuestionTempRepo _questRepo;

        public FormularTempRepo(IDatabaseContext databaseContext, IQuestionTempRepo questRepo ) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _questRepo = questRepo;
        }

        public async Task<List<FormularTemplate>> GetByType(FormularType type, String name)
        {
            return await _databaseContext.FormularTemplates.Where(formular => formular.Type == type).ToListAsync();
        }
    }
}