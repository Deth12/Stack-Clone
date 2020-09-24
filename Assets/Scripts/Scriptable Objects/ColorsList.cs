using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stack/ColorsList")]
public class ColorsList : ScriptableObject
{
    public List<Color> colors = new List<Color>();
}
