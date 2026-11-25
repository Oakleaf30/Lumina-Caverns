using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeManager : MonoBehaviour
{
    public static SpikeManager Instance { get; private set; }

    [SerializeField] private Transform playerFeet;   // point at the feet, not the sprite centre

    [Header("Tiles")]
    [SerializeField] private TileBase[] frames;      // 6 frames: flat, ..., flat

    [Header("Timing")]
    [SerializeField] private float downTime = 2f;    // how long it rests on frame 0
    [SerializeField] private float frameTime = 0.25f;

    [Header("Danger frames")]
    [SerializeField] private int dangerStart = 2;
    [SerializeField] private int dangerEnd = 3;      // inclusive

    [Header("Damage")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float hitCooldown = 0.5f;

    private Tilemap spikeMap;
    private readonly List<Vector3Int> cells = new();
    private int currentFrame;
    private bool isDangerous;
    private float nextHitTime;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Only keep this if the player also persists between scenes
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerHealth = playerFeet.GetComponentInParent<PlayerHealth>();
        StartCoroutine(Cycle());
    }

    public void SetMap(Tilemap map)
    {
        spikeMap = map;
        cells.Clear();

        if (map == null) return;

        map.CompressBounds();
        foreach (var pos in map.cellBounds.allPositionsWithin)
            if (map.GetTile(pos) == frames[0])
                cells.Add(pos);

        // Join the current frame so a new room is in sync mid-cycle
        foreach (var c in cells) map.SetTile(c, frames[currentFrame]);
    }

    private IEnumerator Cycle()
    {
        while (true)
        {
            SetAll(0);
            isDangerous = false;
            yield return new WaitForSeconds(downTime);

            for (int i = 1; i < frames.Length; i++)
            {
                SetAll(i);
                isDangerous = i >= dangerStart && i <= dangerEnd;
                yield return new WaitForSeconds(frameTime);
            }
        }
    }

    private void SetAll(int frame)
    {
        currentFrame = frame;
        if (spikeMap == null) return;
        foreach (var c in cells) spikeMap.SetTile(c, frames[frame]);
    }

    private void Update()
    {
        if (spikeMap == null || !isDangerous || Time.time < nextHitTime) return;

        Vector3Int playerCell = spikeMap.WorldToCell(playerFeet.position);
        if (cells.Contains(playerCell))
        {
            playerHealth.TakeDamage(damage);   // or raise your OnPlayerDamaged GameEvent
            nextHitTime = Time.time + hitCooldown;
        }
    }
}