namespace Backend.Models.Foods;

public class Nutrition
{
    public int Id { get; set; }
    public required int Calories { get; set; }
    public required int Protein { get; set; }
    public required int Carbs { get; set; }
}