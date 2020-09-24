using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private Direction moveDirection;
    
    private Tile tilePrefab;
    private Transform tileParent;
    
    public void SetupTileSpawner(Tile prefab, Transform parent)
    {
        tilePrefab = prefab;
        tileParent = parent;
    }

    public void SpawnTile()
    {
        Tile tile = Instantiate(tilePrefab, tileParent);
        Painter.PaintObject(tile.gameObject, true);
        float x = moveDirection == Direction.X ? transform.position.x : Tile.PreviousTile.transform.position.x;
        float z = moveDirection == Direction.Z ? transform.position.z : Tile.PreviousTile.transform.position.z;
        tile.transform.position = 
            new Vector3(x, Tile.PreviousTile.transform.position.y + tilePrefab.transform.localScale.y, z); 
        tile.ApplyMovement(moveDirection);
    }
}
