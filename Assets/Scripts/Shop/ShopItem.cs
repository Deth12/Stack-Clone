using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public TileSkin skin;

    [SerializeField] private Text titleText = null;
    [SerializeField] private Text priceText = null;
    [SerializeField] private Image itemIcon = null;
    
    [SerializeField] private GameObject locker = null;
    [SerializeField] private GameObject selectionBorder = null;

    public bool IsUnlocked
    {
        get => PlayerPrefs.GetInt("TileSkin_" + skin.id, 0) == 1;
        set
        {
            PlayerPrefs.SetInt("TileSkin_" + skin.id, value ? 1 : 0);
        }
    }

    public bool IsEquipped
    {
        get => selectionBorder.activeSelf;
        set
        {
            if(value)
                PlayerPrefs.SetInt("EquippedTileSkin", skin.id);
            selectionBorder.SetActive(value);
        }
    }

    public void Initialize(TileSkin s)
    {
        skin = s;
        titleText.text = s.title;
        priceText.text = s.price.ToString();
        itemIcon.sprite = s.icon;
        UpdateUI();
    }

    public void UpdateUI()
    {
        locker.SetActive(!IsUnlocked);
    }

    public void Equip()
    {
        if (IsUnlocked)
        {
            IsEquipped = true;
            ShopManager.Instance.EquipItem(this);
        }
        else
        {
            ShopManager.Instance.BuyItem(this);
            locker.SetActive(!IsUnlocked);
        }
    }
}

