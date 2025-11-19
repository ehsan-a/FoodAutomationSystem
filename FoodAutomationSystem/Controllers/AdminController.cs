using FoodAutomationSystem.Data;
using FoodAutomationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using FoodAutomationSystem.Models.ViewModels;

namespace FoodAutomationSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly FoodAutomationSystemContext _context;
        public AdminController(UserManager<User> userManager, FoodAutomationSystemContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var vm = new AdminDashboardViewModel
            {
                TodayReservations = 1,
                TotalRevenue = 2,
                ActiveStudents = 3,
                WeeklyReservations = 4,
                SuccessRate = 5,
                FailedPayments = 6,
                TodayReservationsList = _context.Reservation.Include(x => x.User).Include(x => x.FoodMenu).Include(x => x.FoodMenu.Food).Where(x => x.Date.DayOfYear == DateTime.Now.DayOfYear).ToList(),
                WeeklyData = [
                new WeeklyData{Name="Sat" , Value=14},
                new WeeklyData{Name="Sun" , Value=16},
                new WeeklyData{Name="Mon" , Value=10},
                new WeeklyData{Name="Tue" , Value=18},
                new WeeklyData{Name="Wed" , Value=2},

                ],
                FoodTypeData = [
                    new FoodTypeData{Name="Breakfast",Value=15,Color="#3b82f6"},
                    new FoodTypeData{Name="Luanch",Value=75,Color="#ef4444"}
                    ],
            };
            return View(vm);
        }

        public IActionResult MenuManagement(string SelectedWeeklyMenuId = "1")
        {
            var menus = _context.Menu.ToList();
            var menu = _context.Menu.Include(x => x.FoodMenus).ThenInclude(x => x.Food).FirstOrDefault(x => x.Id == int.Parse(SelectedWeeklyMenuId));
            var fm = menu.FoodMenus;
            var dates = Enumerable.Range(0, 7)
                .Select(i => menu.WeekStartDate.AddDays(i).ToString("yyyy-MM-dd"))
                .ToList();
            var days = Enum.GetValues(typeof(DayOfWeek))
               .Cast<DayOfWeek>()
               .Select(d => d.ToString())
               .ToList();

            var vm = new MenuManagementViewModel
            {
                Dates = dates,
                Days = days,
                SearchQuery = "",
                SelectedWeeklyMenu = menu,
                WeeklyMenuOptions = new SelectList(menus, "Id", "WeekStartDate"),
                SelectedWeeklyMenuId = int.Parse(SelectedWeeklyMenuId),
                AllFoods = _context.Food.ToList(),
            };
            return View(vm);
        }

        public IActionResult UserManagement()
        {
            IEnumerable<UserManagementViewModel> vm = _userManager.Users.Include(x => x.Reservations).Select(x => new UserManagementViewModel { User = x, WalletBalance = x.Balance(_context) });

            return View(vm);

        }
        public IActionResult FoodManagement()
        {
            IEnumerable<Food> vm = _context.Food;
            return View(vm);
        }
        public IActionResult ReservationsManagement()
        {
            IEnumerable<Reservation> vm = _context.Reservation.Include(x => x.User).Include(x => x.FoodMenu.Food);
            return View(vm);
        }

        public IActionResult PaymentManagement()
        {
            IEnumerable<Transaction> vm = _context.Transaction.Include(x => x.User);
            return View(vm);
        }
    }
}
