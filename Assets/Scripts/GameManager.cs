using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
    }
    
    public Action OnGameOver;

    public void StartGame()
    {
        GameStatus.Score = 0;
        SpawnManager.Instance.SpawnNextTile();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (Tile.CurrentTile)
                Tile.CurrentTile.Stop();
        }
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if(Tile.CurrentTile)
                Tile.CurrentTile.Stop();
        }
        #endif
    }

    public void EndGame()
    {
        Tile.PreviousTile = null;
        Tile.CurrentTile = null;
        OnGameOver?.Invoke();
    }

    public void RestartGame()
    {
        GameStatus.Combo = 0;
        GameStatus.TilesAmount = 0;
        SceneManager.LoadScene(0);
    }
    
    // DEBUG.ONLY
    public float targetAccuracy;
}
