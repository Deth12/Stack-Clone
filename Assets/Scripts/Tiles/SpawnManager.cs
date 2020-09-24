using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
    }
    
    public Tile TilePrefab;
    public ColorsList colorsList;

    [SerializeField] private TileSpawner[] TileSpawners = null;

    
    [SerializeField] private Tile startTile = null;
    [SerializeField] private GameObject startBlock = null;
    [SerializeField] private Transform startPos = null;
    [SerializeField] private Transform buildingParent = null;
    [SerializeField] private Image background = null;
        
    private int nextSpawnerIndex = 0;

    private void Start()
    {
        Painter.InitializePainter(colorsList.colors);
        TilePrefab = GameStatus.CurrentTilePrefab;
        Tile startTile = 
            Instantiate(TilePrefab, startPos.position, Quaternion.identity, buildingParent);
        foreach (var spawner in TileSpawners)
        {
            spawner.SetupTileSpawner(TilePrefab, buildingParent);
        }
        
        Tile.PreviousTile = startTile;
        
        Painter.PaintObject(startBlock.gameObject, false);
        Painter.PaintObject(startTile.gameObject, false);
        Painter.PaintBackground(background);
        LeanTween.moveLocalY(buildingParent.gameObject, -2.2f, .5f)
            .setFrom(-3.2f)
            .setEaseOutSine();

        // Subscription
        Tile.OnTilePlaced += SpawnNextTile;
    }

    public void SpawnNextTile()
    {
        GameStatus.TilesAmount++;
        TileSpawners[nextSpawnerIndex].SpawnTile();
        nextSpawnerIndex = nextSpawnerIndex == 0 ? 1 : 0;
        Painter.PaintBackground(background);
    }
    
    private void OnDisable()
    {
        Tile.OnTilePlaced -= SpawnNextTile;
    }

}
