using UnityEngine;
using UnityEngine.SceneManagement;

public class LadderTile : MonoBehaviour
{
    [SerializeField] private GameEvent OnLadderUsed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (OnLadderUsed != null)
        {
            OnLadderUsed.Subscribe(LoadMines);
        }
    }

    private void OnDisable()
    {
        if (OnLadderUsed != null)
        {
            OnLadderUsed.Unsubscribe(LoadMines);
        }
    }

    private void LoadMines()
    {
        SceneManager.LoadScene("Mine");
    }
}
