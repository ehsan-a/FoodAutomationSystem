using System.ComponentModel.DataAnnotations;

namespace FoodAutomationSystem.Models
{
    public class Menu
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateOnly WeekStartDate { get; set; }
        public List<FoodMenu>? FoodMenus { get; set; }
    }
}
