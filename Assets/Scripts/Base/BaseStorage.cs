public class BaseStorage : ItemContainer
{
    public static BaseStorage Instance { get; private set; }

    public ItemData copper;
    public ItemData coal;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Instance.AddItem(copper, 10);
        Instance.AddItem(coal, 2);
    }
}