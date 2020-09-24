using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    [SerializeField] private GameObject gamePanel = null;
    [SerializeField] private Text comboCounter = null;
    [SerializeField] private Text scoreCounter = null;
    [SerializeField] private GameObject endgamePanel = null;
    [SerializeField] private Text highscoreCounter = null;

    // GET RID OF
    private float comboLocalY;

    private void Start()
    {
        gamePanel.SetActive(false);
        endgamePanel.SetActive(false);
        comboBar.gameObject.SetActive(false);
        comboLocalY = comboCounter.gameObject.transform.localPosition.y;
        // Subscription
        GameStatus.OnScoreChange += UpdateScoreCounter;
        GameStatus.OnComboChange += UpdateComboCounter;
        GameManager.Instance.OnGameOver += ShowEndgameUI;
    }

    public void ShowGameUI()
    {
        gamePanel.SetActive(true);
    }

    public void ShowEndgameUI()
    {
        comboCounter.text = "";
        StopAllCoroutines();
        comboBar.gameObject.SetActive(false);
        endgamePanel.SetActive(true);
        highscoreCounter.text = GameStatus.Highscore.ToString();
    }

    public void UpdateScoreCounter(int value)
    {
        scoreCounter.text = value.ToString();
    }

    private void UpdateComboCounter(int value)
    {
        if (value > 1)
        {
            comboCounter.text = "COMBO x" + GameStatus.Combo;
            StopAllCoroutines();
            StartCoroutine(ComboTimer(5f));
            LeanTween.moveLocalY(comboCounter.gameObject, comboLocalY, 0.1f).setFrom(comboLocalY - 10f);
        }
        else
        {
            comboBar.gameObject.SetActive(false);
            comboCounter.text = "";
        }
    }

    public Image comboBar;
    // IEnumerator ComboTimer(float time)
    // {
    //     comboBar.gameObject.SetActive(true);
    //     float elapsed = 1f;
    //     while (elapsed < time)
    //     {
    //         elapsed -= Time.deltaTime;
    //         comboBar.fillAmount -= Time.deltaTime * time; // elapsed / time;
    //         yield return null;
    //     }
    //     comboCounter.text = "";
    //     GameStatus.Combo = 0;
    //     comboBar.gameObject.SetActive(false);
    // }
    IEnumerator ComboTimer(float time)
    {
        comboBar.gameObject.SetActive(true);
        comboBar.fillAmount = 1f;
        float left = time;
        while (left > 0)
        {
            left -= Time.deltaTime;
            comboBar.fillAmount = left / time;
            yield return null;
        }
        comboCounter.text = "";
        GameStatus.Combo = 0;
        comboBar.gameObject.SetActive(false);
    }
    
    private void OnDisable()
    {
        GameStatus.OnScoreChange -= UpdateScoreCounter;
        GameStatus.OnComboChange -= UpdateComboCounter;
        GameManager.Instance.OnGameOver -= ShowEndgameUI;
    }
}
