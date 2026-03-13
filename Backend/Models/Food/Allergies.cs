namespace Backend.Models.Food
{
    [Flags]
    public enum Allergies
    {
        None = 0,
        Peanut = 1 << 0, 
        TreeNuts = 1 << 1,
        Milk = 1 << 2,
        Egg = 1 << 3,
        Soy = 1 << 4,
        Wheat = 1 << 5,
        Fish = 1 << 6,
        ShellFish = 1 << 7,
        Sesame =  1 << 8
    }
}
