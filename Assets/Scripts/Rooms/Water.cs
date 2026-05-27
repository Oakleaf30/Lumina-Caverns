using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private GameEvent onWaterEnter;
    [SerializeField] private GameEvent onWaterExit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onWaterEnter.Raise();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onWaterExit.Raise();
        }
    }
}
