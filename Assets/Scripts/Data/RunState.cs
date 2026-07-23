using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RunState", menuName = "Scriptable Objects/RunState")]
public class RunState : ScriptableObject
{
    public Dictionary<string, int> oreCounts = new();
    public Dictionary<string, int> gemCounts = new();
    public List<string> activeEnchantmentIds = new();

    public float currentHealth;
    public float pickaxeDurability;

    public void ResetForNewRun()
    {
        oreCounts.Clear();
        gemCounts.Clear();
        activeEnchantmentIds.Clear();
        currentHealth = 100f;
        pickaxeDurability = 100f;
    }
}