// Daily calories needs based on Mifflin-St Jeor Equation
// 10% aggresive bulking/losing goals.
namespace Backend.EndPoints.Calories
{

    public class Calories
    {
        //Raised 3 decimal places
        public enum UserActivityLevel
        {
            None,
            Sedentary,                  //little to no exercise

            LightlyActive,              //Light exercise 1-3 Days a week

            ModeratelyActive,           //Light to moderate exercise
                                        //4-5 days a week

            Active,                     //Moderate exercise 6 to 7
                                        //days/week or intense exercise
                                        //3 to 4/week

            VeryActive                 //Physical job or intense training,
                                       //intense exercise 6 to 7 times/week
        }
        public static double calculateBMR(double weightInKg, double heightInCm, 
                                           bool IsMale, int age, 
                                           UserActivityLevel activityLevel, 
                                           int UserTrainingTarget, 
                                           int alreadyKnowCalories = 0)
        {
            double[] ActivityLevelToMultiplier = { 0, 1.2, 1.375, 1.55, 1.725, 1.9 };
            if (alreadyKnowCalories<0)
            {
                throw new ArgumentOutOfRangeException("Please enter a valid range");
            }
            if (alreadyKnowCalories > 0)
            {
                return alreadyKnowCalories;
            }
            if(weightInKg <= 0)
            {
                throw new ArgumentOutOfRangeException("Please enter a valid Weight.");
            }
            if(heightInCm <= 0)
            {
                throw new ArgumentOutOfRangeException("Please enter a valid Height.");
            }
            double bmr = IsMale
                ? (10 * weightInKg + 6.25 * heightInCm - 5 * age + 5)
                : (10 * weightInKg + 6.25 * heightInCm - 5 * age - 161);
            double multiplier = activityLevel == UserActivityLevel.None
                ? 1.0 : ActivityLevelToMultiplier[(int)activityLevel];
            double dailyBMR = bmr * multiplier;
            if(UserTrainingTarget == 0)
            {
                return dailyBMR;
            }else if(UserTrainingTarget == 1)
            {
                return dailyBMR * 1.1;
            }
            else
            {
                return dailyBMR * 0.9;
            }
        }
    }
}
