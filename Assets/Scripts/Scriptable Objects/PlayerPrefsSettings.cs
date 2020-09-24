using UnityEngine;

[CreateAssetMenu(menuName = "Settings/PlayerPrefsSettings")]
public class PlayerPrefsSettings : ScriptableObject
{
    public int GetHighscore()
    {
        return PlayerPrefs.GetInt("Highscore", 0);
    }

    public int GetMoney()
    {
        return PlayerPrefs.GetInt("Money", 0);
    }
    
    public void AddMoney(int amount)
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + amount);
    }
 
    public void ResetAchievements()
    {
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("Highscore", 0);
    }
    
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("TileSkin_1", 1);
    }
}
