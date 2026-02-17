namespace Gacha.Models
{
    public enum ItemRarity
    {
        None = 0,           //default to 0
        Common,             //0
        Rare,               //1
        Epic,               //2
        Mystic,             //3 
        Legendary           //4
    }
    public class ItemType
    {
        public int Id { get; set; }//primary key
        public required string ItemName { get; set; }
        public string? Description { get; set; }
        public ItemRarity ItemRarity { get; set; }
    }
}
