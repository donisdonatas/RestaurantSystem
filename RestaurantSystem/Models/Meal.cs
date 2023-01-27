using RestaurantSystem.Enumbers;

namespace RestaurantSystem.Models;

public class Meal
{
    public MealTypes MealType;
    public string MealName { get; set; }
    public decimal MealPrice { get; set; }

    public Meal(MealTypes mealType, string mealName, decimal mealPrice)
    {
        MealType = mealType;
        MealName = mealName;
        MealPrice = mealPrice;
    }
}
