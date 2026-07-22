public class BaseStorage : ItemContainer
{
    public static BaseStorage Instance { get; private set; }

    public ItemData copper;
    public ItemData coal;
    public ItemData cobalt;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Instance.AddItem(copper, 100);
        Instance.AddItem(coal, 20);
        Instance.AddItem(cobalt, 20);
    }
}