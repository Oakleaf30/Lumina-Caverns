using UnityEngine;

public enum EnchantSlot { Pickaxe, Armour, Sword }

[CreateAssetMenu(fileName = "NewEnchantData", menuName = "Lumina Caverns/Enchant Data")]
public class EnchantData : ScriptableObject
{
    public string enchantId;
    [TextArea] public string description;
    public ItemData requiredGem;
    public ItemData requiredMagicOre;
    public int magicOreAmount;

    public EnchantSlot slot;
}