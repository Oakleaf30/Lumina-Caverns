using UnityEngine;

public class Slime : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * data.movementSpeed * Time.deltaTime;
    }
}
