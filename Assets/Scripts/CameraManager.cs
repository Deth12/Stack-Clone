using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
    }
    
    [SerializeField]
    private float followSpeed = 5f;

    private float targetSize;
    private Vector3 targetPosition;
    private float moveDelta;
    
    private void Start()
    {
        targetPosition = transform.position;
        moveDelta = SpawnManager.Instance.TilePrefab.transform.localScale.y;
        
        // Subscription
        GameManager.Instance.OnGameOver += ZoomOut;
        Tile.OnTilePlaced += MoveCamera;
        Tile.OnTilePlaced += CheckTempZoomOut;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

    private void MoveCamera()
    {
        targetPosition = new Vector3(targetPosition.x, targetPosition.y + moveDelta, targetPosition.z);
    }

    public void CheckTempZoomOut()
    {
        Vector3 ls = Tile.PreviousTile.transform.localScale;
        float avgScale = (ls.x + ls.z) / 2.0f;
        float targetSize = (avgScale > 1f ? avgScale + 1f : 1.65f);
        StartCoroutine(SmoothZooming(targetSize, .5f));
    }

    public void ZoomOut()
    {
        float targetSize = GameStatus.TilesAmount * 0.15f + 1.65f;
        StartCoroutine(SmoothZooming(targetSize, 1f));
    }

    IEnumerator SmoothZooming(float targetSize, float time)
    {
        float left = 0;
        float startSize = Camera.main.orthographicSize;
        while (left < time)
        {
            left += Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSize, left / time) ;
            yield return null;
        }
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= ZoomOut;
        Tile.OnTilePlaced -= MoveCamera;
        Tile.OnTilePlaced -= CheckTempZoomOut;
    }
}
