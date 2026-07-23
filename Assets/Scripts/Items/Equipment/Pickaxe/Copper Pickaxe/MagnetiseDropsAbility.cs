using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Pickaxe Abilities/Magnetise Drops")]
public class MagnetiseDropsAbility : PickaxeAbility
{
    public float radius = 3f;
    public float pullSpeed = 8f;

    public override void OnMine(PlayerMining player, Vector3 nodePosition)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(nodePosition, radius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<ItemDrop>(out var drop))
            {
                player.StartCoroutine(PullTowardsPlayer(drop, player.transform));
            }
        }
    }

    private IEnumerator PullTowardsPlayer(ItemDrop drop, Transform playerTransform)
    {
        while (Vector3.Distance(drop.transform.position, playerTransform.position) > 0.2f)
        {
            drop.transform.position = Vector3.MoveTowards(
                drop.transform.position,
                playerTransform.position,
                pullSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}