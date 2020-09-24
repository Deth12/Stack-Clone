using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);
        Instance = this;
        InititalizeShop();
    }

    [SerializeField] private TilesSkins skins = null;
    [SerializeField] private Transform shopItemsContainer = null;
    [SerializeField] private ShopItem itemPrefab = null;

    private ShopItem current;
    private List<ShopItem> shopItems = new List<ShopItem>();

    public void InititalizeShop()
    {
        foreach (var item in skins.tileSkins)
        {
            ShopItem i = Instantiate(itemPrefab, shopItemsContainer);
            i.Initialize(item);
            shopItems.Add(i);
        }
        int selectedIndex = PlayerPrefs.GetInt("EquippedTileSkin", 1);
        current = shopItems.Find(x => x.skin.id == selectedIndex);
        current.IsEquipped = true;
        GameStatus.CurrentTilePrefab = current.skin.tilePrefab;
    }

    /*
    private void UpdateCurrentEquippedTileSkin()
    {
        int selectedIndex = PlayerPrefs.GetInt("EquippedTileSkin", 1);
        ShopItem current = shopItems.Find(x => x.skin.id == selectedIndex);
        current.IsEquipped = true;
        GameStatus.CurrentTilePrefab = current.skin.tilePrefab;
    }*/
    
    public void EquipItem(ShopItem item)
    {
        current.IsEquipped = false;
        current = item;
        current.IsEquipped = true;
        GameStatus.CurrentTilePrefab = item.skin.tilePrefab;
        SceneManager.LoadSceneAsync(0);
    }

    public void BuyItem(ShopItem item)
    {
        if (GameStatus.Money >= item.skin.price)
        {
            GameStatus.Money -= item.skin.price;
            item.IsUnlocked = true;
        }
    }
  
}
