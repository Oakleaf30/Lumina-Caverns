using UnityEngine;

[CreateAssetMenu(menuName = "Pickaxe Abilities/Magnetise Drops")]
public class MagnetiseDropsAbility : PickaxeAbility
{
    public override void OnMine(PlayerMining player, Vector3 nodePosition)
    {
        // copper: pull nearby drops toward player
    }
}