namespace FoodAutomationSystem.Models
{
    public class ReservationFlowViewModel
    {

        public User User { get; set; }


        public FoodMenu FoodMenu { get; set; }

        public DateTime Date { get; set; }

        public decimal WalletBalance { get; set; }
        public ReservationStatus Step { get; set; } = ReservationStatus.Confirmed;


        //public bool HasEnoughBalance => User.WalletBalance >= Food.Price;
    }
}
