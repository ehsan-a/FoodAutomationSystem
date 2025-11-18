using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodAutomationSystem.Data;
using FoodAutomationSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace MvcFoodAutomationSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _context;

        public UsersController(UserManager<User> context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }
    }
}
