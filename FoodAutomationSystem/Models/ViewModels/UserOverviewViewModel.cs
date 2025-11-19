namespace FoodAutomationSystem.Models.ViewModels
{
    public class UserOverviewViewModel
    {
        public User User { get; set; }
        public List<Reservation>? UpcomingReservations { get; set; }
        public List<FoodMenu>? TomorrowsFood { get; set; }
        public decimal WalletBalance { get; set; }
    }
}
