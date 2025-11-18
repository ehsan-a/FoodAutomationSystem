namespace FoodAutomationSystem.Models
{
    public class UserOverviewViewModel
    {

        public User User { get; set; }
        public List<Reservation>? UpcomingReservations { get; set; }
        public List<Food>? TodaysFoods { get; set; }
        public decimal WalletBalance { get; set; }


    }
}
