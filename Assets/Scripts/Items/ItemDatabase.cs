using System.Collections.Generic;
using UnityEngine;

public static class ItemDatabase
{
    private static Dictionary<string, ItemData> _byId;

    public static ItemData GetById(string id)
    {
        if (_byId == null)
        {
            _byId = new Dictionary<string, ItemData>();
            foreach (var item in Resources.LoadAll<ItemData>("Items"))
                _byId[item.itemId] = item;
        }
        return _byId.TryGetValue(id, out var data) ? data : null;
    }
}