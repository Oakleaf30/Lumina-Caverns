using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class NodeHunterWave : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float scale;
    [SerializeField] private float verticalOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ChangeSize());
    }

    private IEnumerator ChangeSize()
    {
        transform.localScale = Vector3.zero;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * scale, t);
            yield return null;
        }

        transform.localScale = Vector3.one * scale;
        yield return new WaitForFixedUpdate(); // let physics process the final size before we destroy
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out OreNode node))
        {
            NodeGlow.SpawnAt(node.transform.position);
        }
    }
}
