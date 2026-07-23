using UnityEngine;

public abstract class PickaxeAbility : ScriptableObject
{
    [TextArea]
    public string description;

    public abstract void OnMine(PlayerMining player, Vector3 nodePosition);
}

//[CreateAssetMenu(menuName = "Pickaxe Abilities/Radius Knockback")]
public class RadiusKnockbackAbility : PickaxeAbility
{
    public override void OnMine(PlayerMining player, Vector3 nodePosition)
    {
        // iron: 1 tile knockback on swing
    }
}

//[CreateAssetMenu(menuName = "Pickaxe Abilities/Damage Nearby Nodes")]
public class DamageNearbyNodesAbility : PickaxeAbility
{
    public override void OnMine(PlayerMining player, Vector3 nodePosition)
    {
        // gold: damage nodes near the mined node
    }
}