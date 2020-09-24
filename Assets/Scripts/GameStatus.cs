using UnityEngine;

public class GameStatus : MonoBehaviour
{
    public static Tile CurrentTilePrefab;

    private static int _tilesAmount;

    public static int TilesAmount
    {
        get => _tilesAmount;
        set => _tilesAmount = value;
    }

    private static int _combo;
    public static int Combo
    {
        get => _combo;
        set
        {
            _combo = value;
            OnComboChange?.Invoke(_combo);
        }
    }

    private static int _score;
    public static int Score
    {
        get => _score;
        set
        {
            _score = value;
            if (Highscore < _score)
                Highscore = _score;
            OnScoreChange?.Invoke(_score);
        } 
    }

    public static int Highscore
    {
        get => PlayerPrefs.GetInt("Highscore", 0);
        set => PlayerPrefs.SetInt("Highscore", value);
    }

    public static int Money
    {
        get => PlayerPrefs.GetInt("Money", 0);
        set
        {
            int amount = value - PlayerPrefs.GetInt("Money", 0);
            PlayerPrefs.SetInt("Money", value);
            OnMoneyChange?.Invoke(value, amount);
        }
    }

    public static bool Sound
    {
        get => PlayerPrefs.GetInt("Sound", 1) == 1 ? true : false;
        set
        {
            PlayerPrefs.SetInt("Sound", value == true ? 1 : 0);
            OnSoundToggle(value);
        }
    }
    
    public static System.Action<int> OnScoreChange;
    public static System.Action<int> OnComboChange;
    public static System.Action<int, int> OnMoneyChange;

    public static System.Action<bool> OnSoundToggle;

    public static bool AdsDisabled
    {
        get => PlayerPrefs.GetInt("Ads", 0) == 1 ? true : false;
        set => PlayerPrefs.SetInt("Ads", value == true ? 1 : 0);
    }
}
