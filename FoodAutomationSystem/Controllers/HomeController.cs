using FoodAutomationSystem.Data;
using FoodAutomationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoodAutomationSystem.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly UserManager<User> _userManager;
        private readonly FoodAutomationSystemContext _context;
        public HomeController(UserManager<User> userManager, FoodAutomationSystemContext context)
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
                int today = (int)DateTime.Now.DayOfWeek;
                var model = new UserOverviewViewModel
                {
                    User = user,
                    TodaysFoods = _context.FoodMenu.Where(x => (int)x.DayOfWeek == today).Select(x => x.Food).ToList(),
                    UpcomingReservations = _context.Reservation.Include(x => x.FoodMenu.Menu).Include(x => x.FoodMenu.Food)
                    .Where(x => x.UserId == user.Id && x.FoodMenu.Menu.WeekStartDate.AddDays((int)x.FoodMenu.DayOfWeek).DayOfYear >= DateTime.Now.DayOfYear).ToList(),
                    WalletBalance = _context.Transaction.Where(x => x.Type == TransactionType.WalletTopUp).Sum(x => x.Amount) -
                _context.Transaction.Where(x => x.Type == TransactionType.Reservation).Sum(x => x.Amount)
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
                Transactions = _context.Transaction.Where(x => x.UserId == user.Id).ToList(),
                WalletBalance = _context.Transaction.Where(x => x.Type == TransactionType.WalletTopUp).Sum(x => x.Amount) -
                _context.Transaction.Where(x => x.Type == TransactionType.Reservation).Sum(x => x.Amount)
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
                WalletBalance = _context.Transaction.Where(x => x.Type == TransactionType.WalletTopUp).Sum(x => x.Amount) -
                _context.Transaction.Where(x => x.Type == TransactionType.Reservation).Sum(x => x.Amount)

            };
            return View(vm);
        }

        public async Task<IActionResult> FoodDetails(int id)
        {
            var vm = new FoodDetailsViewModel
            {
                Food = _context.Food.First(X => X.Id == id),
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
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
