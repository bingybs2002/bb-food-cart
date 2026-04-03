namespace Backend.Models.Gacha;

public class GachaModel
{
    public enum ItemRarity
    {
        //droprate
        Common,             //50%
        Rare,               //30%
        Epic,               //12%
        Mystic,             //7%
        Legendary           //1%
    }
    public class GachaItemType
    {
        public int Id { get; set; }//primary key
        public required string ItemName { get; set; }
        public string? Description { get; set; }
        public ItemRarity ItemRarity { get; set; }
    }
}
