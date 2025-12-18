using UnityEngine;

public class CrumbleEffect : MonoBehaviour
{
    public void OnAnimationFinish()
    {
        Destroy(gameObject);
    }
}