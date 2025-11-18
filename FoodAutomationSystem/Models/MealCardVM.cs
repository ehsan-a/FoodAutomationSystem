namespace FoodAutomationSystem.Models
{
    public class MealCardVM
    {
        public FoodMenu FoodMenu { get; set; }
        public string Date { get; set; }
        public bool IsPast { get; set; }
    }
}
