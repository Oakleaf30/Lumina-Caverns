using System.Collections.Generic;
using UnityEngine;

public class EnchantRegistry : MonoBehaviour
{
    [SerializeField] private List<EnchantData> allEnchants;

    public EnchantData GetEnchant(EnchantSlot slot, ItemData gem)
    {
        return allEnchants.Find(e => e.slot == slot && e.requiredGem == gem);
    }
}