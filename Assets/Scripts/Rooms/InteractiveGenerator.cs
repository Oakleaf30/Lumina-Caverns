using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractiveGenerator : MonoBehaviour
{
    [Header("Spawning Setup")]
    [SerializeField] private GameObject NodePrefab;
    [SerializeField] private GameObject BarrelPrefab;
    
    [SerializeField] private int minNodesPerAnchor;
    [SerializeField] private int maxNodesPerAnchor;
    private int activeAnchors;
    private List<SpawnPool> SpawnPool;

    [Header("Overlap Safety")]
    [SerializeField] private LayerMask resourcesLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int maxSpawnAttempts;

    [SerializeField] private LadderTile ladder;
    private int totalNodes = 0;

    [SerializeField] private GameObject ladderPrefab;
    private bool ladderFlag = true;
    private bool hasSetupLadderFlag = false;

    [SerializeField] BarrelData[] barrels;

    private void Setup(BiomeData biome)
    {
        totalNodes = 0;
        activeAnchors = biome.activeAnchors;
        SpawnPool = biome.spawnPools;

        if (!hasSetupLadderFlag)
        {
            ladderFlag = biome.biomeName != "Coal";
            hasSetupLadderFlag = true;
        }
    }

    public void GenerateInteractivity(Room room, BiomeData biome)
    {
        if (biome.activeAnchors == 0)
            return;

        Setup(biome);

        Transform anchorsContainer = room.transform.Find("Markers/Interaction Marker Container");

        if (anchorsContainer == null)
        {
            return;
        }

        // 1. Gather all possible spawn anchors placed in this room prefab
        SpawnAnchor[] allAnchors = anchorsContainer.GetComponentsInChildren<SpawnAnchor>();

        // 2. Create a list to shuffle so we don't pick the same anchor twice
        List<SpawnAnchor> anchorPool = new List<SpawnAnchor>(allAnchors);
        ShuffleList(anchorPool);

        // 3. Determine how many anchors we will actually activate for this specific room execution
        int anchorsToActivate = Mathf.Min(activeAnchors, anchorPool.Count);

        // 4. Loop ONLY through the chosen subset of anchors
        for (int a = 0; a < anchorsToActivate; a++)
        {
            SpawnAnchor chosenAnchor = anchorPool[a];

            if (CheckLadder(chosenAnchor))
                continue;

            SpawnPool clumpType = SelectWeightedPool();
            int numberOfNodes = Random.Range(minNodesPerAnchor, maxNodesPerAnchor + 1);
            bool guaranteedSpawned = false;

            // Extra setup for barrels
            bool isBarrel = clumpType.resourceName == "Barrel";
            BarrelData barrelData = null;
            OreData oreData = null;
            float spaceRequired;

            // 5. Spawn the exact maximum number of nodes dedicated to this specific anchor radius
            for (int n = 0; n < numberOfNodes; n++)
            {
                Vector2 validSpawnPosition = Vector2.zero;
                bool foundSpot = false;
                

                if (isBarrel)
                {
                    barrelData = barrels[Random.Range(0, barrels.Length)];
                    spaceRequired = barrelData.spaceRequired;
                } else
                {
                    oreData = ResolveVariant(clumpType, ref guaranteedSpawned);
                    spaceRequired = oreData.spaceRequired;
                }

                for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
                {
                    // Get a random 2D coordinate inside this specific anchor's circle
                    Vector2 randomOffset = Random.insideUnitCircle * chosenAnchor.spawnRadius;
                    Vector2 potentialSpot = (Vector2)chosenAnchor.transform.position + randomOffset;

                    LayerMask layer = isBarrel ? enemyLayer : resourcesLayer;
                    // Overlap Check: Avoid clipping walls or previously spawned nodes
                    Collider2D hit = Physics2D.OverlapCircle(potentialSpot, spaceRequired, layer);

                    if (hit == null)
                    {
                        validSpawnPosition = potentialSpot;
                        foundSpot = true;
                        break; // Clear spot found! Exit attempt loop
                    }
                }

                if (foundSpot)
                {
                    if (isBarrel)
                    {
                        GameObject interactable = Instantiate(BarrelPrefab, validSpawnPosition, Quaternion.identity, room.transform);

                        Barrel barrelScript = interactable.GetComponent<Barrel>();

                        barrelScript.InitialiseImmediate(barrelData);
                    } else
                    {
                        totalNodes++;

                        GameObject interactable = Instantiate(NodePrefab, validSpawnPosition, Quaternion.identity, room.transform);

                        // Grab the OreNode component
                        OreNode nodeScript = interactable.GetComponent<OreNode>();

                        // FIX 1: Tell the node to update its physical collider/scale RIGHT NOW
                        // (Ensure you implement this Initialize method in your OreNode script!)
                        nodeScript.InitialiseImmediate(oreData);
                    }

                    // FIX 2: Force Unity to register this new collider into the physics world space mid-frame
                    Physics2D.SyncTransforms();
                }
            }
        }

        ladder.AddNodes(totalNodes);
    }

    private SpawnPool SelectWeightedPool()
    {
        float totalWeight = 0f;
        foreach (var pool in SpawnPool)
            totalWeight += pool.selectionWeight;

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var pool in SpawnPool)
        {
            cumulative += pool.selectionWeight;
            if (roll <= cumulative)
                return pool;
        }

        // Fallback in case of floating point rounding at the boundary
        return SpawnPool[SpawnPool.Count - 1];
    }

    private OreData ResolveVariant(SpawnPool pool, ref bool guaranteedSpawned)
    {
        if (guaranteedSpawned == false && pool.guaranteedVariant != null)
        {
            guaranteedSpawned = true;
            return pool.guaranteedVariant;
        } else
        {
            bool spawnLarge = Random.value < pool.largeSizeChance;
            return spawnLarge ? pool.largeVariant : pool.smallVariant;
        }
    }

    // Fisher-Yates shuffle algorithm to randomize our list of anchors completely.
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private bool CheckLadder(SpawnAnchor anchor)
    {
        if (!ladderFlag)
        {
            Instantiate(ladderPrefab, anchor.transform.position, Quaternion.identity);
            ladderFlag = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}