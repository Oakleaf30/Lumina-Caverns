using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Lumina Caverns/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Base Identity")]
    public string enemyName;
    public Sprite defaultSprite;
    public RuntimeAnimatorController animatorController;

    [Header("Combat Stats")]
    public int maxHealth;
    public float movementSpeed;
    public int contactDamage;
    public float attackCooldown;
    public float knockbackResistance;

    [Header("Drop Tables")]
    public ItemData dropData;
    [Range(0f, 1f)] public float dropChance;
}