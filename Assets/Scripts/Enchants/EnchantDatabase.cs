using System.Collections.Generic;
using UnityEngine;

public static class EnchantDatabase
{
    private static Dictionary<(EnchantSlot, ItemData), EnchantData> _bySlotAndGem;
    private static Dictionary<string, EnchantData> _byId;

    private static void EnsureLoaded()
    {
        if (_bySlotAndGem != null) return;

        _bySlotAndGem = new Dictionary<(EnchantSlot, ItemData), EnchantData>();
        _byId = new Dictionary<string, EnchantData>();

        foreach (var enchant in Resources.LoadAll<EnchantData>("Enchants"))
        {
            _bySlotAndGem[(enchant.slot, enchant.requiredGem)] = enchant;
            _byId[enchant.enchantId] = enchant;
        }
    }

    public static EnchantData GetEnchant(EnchantSlot slot, ItemData gem)
    {
        EnsureLoaded();
        return _bySlotAndGem.TryGetValue((slot, gem), out var data) ? data : null;
    }

    public static EnchantData GetById(string id)
    {
        EnsureLoaded();
        if (string.IsNullOrEmpty(id)) return null;
        return _byId.TryGetValue(id, out var data) ? data : null;
    }
}