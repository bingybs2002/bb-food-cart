namespace Backend.Models;
//Enum bitmasking, allows one enum to
//express multiple meanings combined
[Flags]
public enum Allergies
{
    None = 0,//0
    Peanut = 1 << 0,//1
    TreeNuts = 1 << 1,//2
    Milk = 1 << 2,//4
    Egg = 1 << 3,//8
    Soy = 1 << 4,//16
    Wheat = 1 << 5,//32
    Fish = 1 << 6,//64
    ShellFish = 1 << 7,//128
    Sesame = 1 << 8//256
}
