using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/TileSkins")]
public class TilesSkins : ScriptableObject
{
    public List<TileSkin> tileSkins = new List<TileSkin>();
}

[System.Serializable]
public class TileSkin
{
    public int id;
    public Tile tilePrefab;
    public string title;
    public int price;
    public Sprite icon;
}
