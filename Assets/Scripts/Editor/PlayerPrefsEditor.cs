using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerPrefsSettings))]
public class PlayerPrefsEditor : Editor
{
    private int addMoneyAmount;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerPrefsSettings contr = (PlayerPrefsSettings) target;

        EditorGUILayout.LabelField("Current money: " + contr.GetMoney());
        EditorGUILayout.LabelField("Highscore: " + contr.GetHighscore());

        if (GUILayout.Button("Reset Player Prefs"))
        {
            contr.ResetPlayerPrefs();
        }

        addMoneyAmount = EditorGUILayout.IntField("Money amount", addMoneyAmount);
        if (GUILayout.Button("Add Money"))
        {
            contr.AddMoney(addMoneyAmount);
        }
    }
}
