using InterviewPrep.Shared.Domain;

namespace InterviewPrep.Shared.Services;

public static class DiscountEngine
{
    public static (decimal discountAmount, string reason) CalculateDiscount(Order order)
    {
        if (order.Total >= 1000m)
        {
            return (order.Total * 0.10m, "10% for orders >= 1000");
        }

        return order.Items.Count switch
        {
            >= 5 => (order.Total * 0.05m, "5% for 5+ items"),
            _ => (0m, "No discount")
        };
    }
}
