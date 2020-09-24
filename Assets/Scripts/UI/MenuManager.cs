using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    // Panels
    [SerializeField] private GameObject startPanel = null;
    [SerializeField] private GameObject shopPanel = null;
    [SerializeField] private GameObject endgamePanel = null;

    // Sound
    [SerializeField] private AudioClip buttonClick = null;
    [SerializeField] private GameObject soundSwitch = null;
    [SerializeField] private GameObject adsSwitch = null; // DEBUG ONLY

    // Other
    private CanvasGroup startCanvesGroup;
    
    [SerializeField] private CanvasGroup fader = null;
    [SerializeField] private RectTransform tapToStart = null;

    [SerializeField] private Text moneyCounter = null;
    [SerializeField] private Text addedCounter = null;
    
    private CanvasGroup addedCG;

    private void Start()
    {
        startPanel.SetActive(true);
        shopPanel.SetActive(false);
        endgamePanel.SetActive(false);
        startCanvesGroup = startPanel.GetComponent<CanvasGroup>();
        // REMOVE THIS
        soundSwitch.transform.GetChild(GameStatus.Sound ? 1 : 0).gameObject.SetActive(true);
        soundSwitch.transform.GetChild(GameStatus.Sound ? 0 : 1).gameObject.SetActive(false);
        adsSwitch.transform.GetChild(GameStatus.AdsDisabled ? 1 : 0).gameObject.SetActive(false);
        adsSwitch.transform.GetChild(GameStatus.AdsDisabled ? 0 : 1).gameObject.SetActive(true);
        LeanTween.alphaCanvas(startCanvesGroup, 1f, .5f).setFrom(0f).setOnComplete(() =>
        {
            startCanvesGroup.blocksRaycasts = true;
        });
        LeanTween.textAlpha(tapToStart, 0.2f, 0.5f).setFrom(1f).setLoopPingPong();
        GameManager.Instance.OnGameOver += EndGame;
        GameStatus.OnMoneyChange += UpdateMoneyCounter;
        UpdateMoneyCounter(GameStatus.Money, 0);
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        GameManager.Instance.StartGame();
        UIManager.Instance.ShowGameUI();
    }

    public void RestartGame()
    {
        fader.blocksRaycasts = true;
        LeanTween.alphaCanvas(fader, 1f, 0.3f).setFrom(0f).setOnComplete(() =>
        {
            GameManager.Instance.RestartGame();
        });
    }

    public void EndGame()
    {
    /*
        if(!GameStatus.AdsDisabled)
            AdsManager.Instance.ShowInterstitialAd();
    */
        endgamePanel.SetActive(true);
    }

    public void SwitchAudio()
    {
        GameStatus.Sound = !GameStatus.Sound;
        soundSwitch.transform.GetChild(GameStatus.Sound ? 1 : 0).gameObject.SetActive(true);
        soundSwitch.transform.GetChild(GameStatus.Sound ? 0 : 1).gameObject.SetActive(false);
    }

    public void ButtonSFX()
    {
        SoundManager.Instance.PlaySound(buttonClick, .5f);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= EndGame;
        GameStatus.OnMoneyChange -= UpdateMoneyCounter;
    }

    public void EnableCheatMode()
    {
        GameManager.Instance.targetAccuracy = 0.5f;
    }

    public void UpdateMoneyCounter(int newValue, int amount)
    {
        if(amount != 0)
            AddMoney(amount);
        moneyCounter.text = newValue.ToString();
    }
    
    // DEBUG ONLY
    public void SwitchAds()
    {
        GameStatus.AdsDisabled = !GameStatus.AdsDisabled;
        adsSwitch.transform.GetChild(GameStatus.AdsDisabled ? 1 : 0).gameObject.SetActive(false);
        adsSwitch.transform.GetChild(GameStatus.AdsDisabled ? 0 : 1).gameObject.SetActive(true);
    }
    
    public void AddMoney(int amount)
    {
        LeanTween.cancel(addedCounter.rectTransform);
        LeanTween.cancel(addedCounter.gameObject);
        addedCounter.text = "+ " + amount.ToString();
        LeanTween
            .moveLocalY(addedCounter.gameObject, addedCounter.transform.localPosition.y, .5f)
            .setFrom(addedCounter.transform.localPosition.y - 10f)
            .setOnComplete(() =>
            {
                LeanTween
                    .alphaText(addedCounter.rectTransform, 0f, 1.3f)
                    .setFrom(1f)
                    .setOnComplete(() =>
                    {
                        addedCounter.text = "";
                    });
            });
    }
}