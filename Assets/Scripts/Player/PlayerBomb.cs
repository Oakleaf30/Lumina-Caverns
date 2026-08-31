using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    private BaseStorage Storage => BaseStorage.Current;

    [Header("Settings")]
    [SerializeField] private float placeOffset = 0.5f;
    [SerializeField] private float verticalCenterOffset = 0.5f;
    

    [Header("References")]
    [SerializeField] private ItemData bomb;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameEvent onBombCountChanged;

    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && Storage.GetQuantity(bomb) > 0)
        {
            PlaceBomb();
        }
    }

    private void PlaceBomb()
    {
        Vector3 centerOrigin = transform.position + new Vector3(0, verticalCenterOffset, 0);

        // 2. Get player look direction and project the strike position forward smoothly
        Vector2 lookDir = playerInteraction != null ? playerInteraction.GetLastDirection() : Vector2.down;
        Vector3 spawnPosition = centerOrigin + (Vector3)(lookDir * placeOffset);

        Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
        Storage.RemoveItem(bomb, 1);
        onBombCountChanged.Raise();
    }
}
