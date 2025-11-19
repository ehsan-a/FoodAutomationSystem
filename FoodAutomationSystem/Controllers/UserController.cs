using FoodAutomationSystem.Data;
using FoodAutomationSystem.Models;
using FoodAutomationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodAutomationSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly FoodAutomationSystemContext _context;
        public UserController(UserManager<User> userManager, FoodAutomationSystemContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Overview()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                int tomorrow = (int)DateTime.Now.AddDays(1).DayOfWeek;
                var model = new UserOverviewViewModel
                {
                    User = user,
                    TomorrowsFood = _context.FoodMenu.Include(x => x.Food).Where(x => (int)x.DayOfWeek == tomorrow).ToList(),
                    UpcomingReservations = _context.Reservation.Include(x => x.FoodMenu.Menu).Include(x => x.FoodMenu.Food)
                    .Where(x => x.UserId == user.Id && x.FoodMenu.Menu.WeekStartDate.AddDays((int)x.FoodMenu.DayOfWeek).DayOfYear >= DateTime.Now.DayOfYear).ToList(),
                    WalletBalance = user.Balance(_context)
                };
                return View(model);
            }
            return NotFound();
        }

        public IActionResult WeeklyMenu(string date)
        {

            var selectedDate = string.IsNullOrEmpty(date)
                ? DateTime.Now.Date
                : DateTime.Parse(date);

            var menu = _context.Menu.FirstOrDefault(x => x.WeekStartDate.DayOfYear <= selectedDate.DayOfYear);

            var dates = Enumerable.Range(0, 7)
                .Select(i => menu.WeekStartDate.AddDays(i).ToString("yyyy-MM-dd"))
                .ToList();

            FoodMenu breakfast = null;
            var query = _context.FoodMenu.Include(x => x.Food)
                .FirstOrDefault(m => m.MenuId == menu.Id && m.Food.Type == FoodType.Breakfast && m.DayOfWeek == selectedDate.DayOfWeek);
            if (query != null) breakfast = query;

            FoodMenu lunch = null;
            query = _context.FoodMenu.Include(x => x.Food)
               .FirstOrDefault(m => m.MenuId == menu.Id && m.Food.Type == FoodType.Lunch && m.DayOfWeek == selectedDate.DayOfWeek);
            if (query != null) lunch = query;

            bool deadlinePassed = DateTime.Now > selectedDate;

            var vm = new WeeklyMenuViewModel
            {
                SelectedDate = selectedDate.ToString("yyyy-MM-dd"),
                Dates = dates,
                Breakfast = breakfast,
                Lunch = lunch,
                IsDeadlinePassed = deadlinePassed
            };

            return View(vm);
        }
        public async Task<IActionResult> Wallet()
        {

            var user = await _userManager.GetUserAsync(User);
            var vm = new UserWalletViewModel
            {
                User = user,
                Transactions = _context.Transaction.Where(x => x.UserId == user.Id).OrderByDescending(x=>x.Date).ToList(),
                WalletBalance = user.Balance(_context)
            };
            return View(vm);
        }

        public async Task<IActionResult> MyReservations()
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = new UserReservationsViewModel
            {
                User = user,
                Reservations = _context.Reservation.Include(x => x.FoodMenu.Food).Include(x => x.FoodMenu.Menu).Where(x => x.UserId == user.Id).ToList(),

            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ReservationFlow(int foodMenuId, string date)
        {
            var user = await _userManager.GetUserAsync(User);
            var vm = new ReservationFlowViewModel
            {
                User = user,
                FoodMenu = _context.FoodMenu.Include(x => x.Food).First(x => x.Id == foodMenuId),
                Date = DateTime.Parse(date),
                WalletBalance = user.Balance(_context)
            };
            return View(vm);
        }

        public async Task<IActionResult> FoodDetails(int id)
        {
            var foodMenu = _context.FoodMenu.Include(x => x.Menu).Include(x => x.Food).First(X => X.Id == id);
            var date = foodMenu.Menu.WeekStartDate.AddDays((int)foodMenu.DayOfWeek);
            var vm = new FoodMenuViewModel
            {
                FoodMenu = foodMenu,
                Date = date.ToString("yyyy-MM-dd"),
                IsPast = DateTime.Now.DayOfYear >= date.DayOfYear,
            };
            return View(vm);
        }

        public async Task<IActionResult> QRTicket(int id)
        {
            var vm = new QRTicketViewModel
            {
                Reservation = _context.Reservation.Include(x => x.FoodMenu.Food).First(X => X.Id == id),
            };
            return View(vm);
        }


        public async Task<IActionResult> ReservationSuccess(int id)
        {
            var vm = new QRTicketViewModel
            {
                Reservation = _context.Reservation.Include(x => x.FoodMenu.Food).First(X => X.Id == id),
            };
            return View(vm);
        }
    }
}
