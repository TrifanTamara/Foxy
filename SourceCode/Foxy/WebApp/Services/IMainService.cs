﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IMainService
    {
        MenuModel GetMenuModel(string email);
    }
}