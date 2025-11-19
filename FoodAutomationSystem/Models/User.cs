using FoodAutomationSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodAutomationSystem.Models
{
    public enum UserStatus
    {
        Active,

    }

    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public UserStatus Status { get; set; }
        public List<Reservation>? Reservations { get; set; }
        public decimal Balance(FoodAutomationSystemContext context)
        {
            return context.Transaction.Where(x => x.UserId == Id).Sum(x => (x.Type == TransactionType.Reservation ? -x.Amount : x.Amount));
        }
    }
}
