﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Application
{
    public class ZipcodeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
