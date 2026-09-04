using UnityEngine;

public class BaseStorage : ItemContainer
{
    [SerializeField] private GameEvent onStorageReady;
    private static BaseStorage _current;

    public static BaseStorage Current
    {
        get
        {
            if (_current == null)
                _current = FindFirstObjectByType<BaseStorage>();
            return _current;
        }
    }

    public ItemData copper;
    public ItemData coal;
    public ItemData cobalt;

    void Start()
    {
        items = GameSession.Instance.runState.storage;

        //AddItem(copper, 100);
        //AddItem(coal, 9);
        //AddItem(cobalt, 20);

        onStorageReady.Raise();
    }
}