namespace Backend.EndPoints.Calories;

public static class EndpointCalories
{
    public static void CaloriesUtilities(this IEndpointRouteBuilder r)
    {
        var app = r.MapGroup("/calories");

        app.MapGet("/", (
            double weightInKg,
            double heightInCm,
            bool isMale,
            int age,
            Calories.UserActivityLevel activitylevel,
            int target,
            int alreadyKnowCalories = 0) =>
        {
            var results = Calories.calculateBMR(
                weightInKg,
                heightInCm,
                isMale,
                age,
                activitylevel,
                target,
                alreadyKnowCalories);

            return Results.Ok(Math.Round(results));
        });
    }
}
