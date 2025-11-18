using FoodAutomationSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodAutomationSystem.Data
{
    public class FoodAutomationSystemContext : IdentityDbContext<User>
    {
        public FoodAutomationSystemContext(DbContextOptions<FoodAutomationSystemContext> options)
            : base(options)
        {
        }

        public DbSet<FoodAutomationSystem.Models.Food> Food { get; set; } = default!;
        public DbSet<FoodAutomationSystem.Models.FoodMenu> FoodMenu { get; set; } = default!;
        public DbSet<FoodAutomationSystem.Models.Menu> Menu { get; set; } = default!;
        public DbSet<FoodAutomationSystem.Models.Reservation> Reservation { get; set; } = default!;
        public DbSet<FoodAutomationSystem.Models.Transaction> Transaction { get; set; } = default!;
    }
}
