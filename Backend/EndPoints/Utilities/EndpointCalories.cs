namespace Backend.EndPoints.Utilities
{
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
                Backend.HelperFunctions.Calories.UserActivityLevel activitylevel,
                int target,
                int alreadyKnowCalories = 0) =>
            {
                var results = Backend.HelperFunctions.Calories.calculateBMR(
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
}
