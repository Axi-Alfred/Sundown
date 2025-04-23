using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Filter", menuName = "Filter/Filter Data")]
public class Filter : ScriptableObject
{
    public string filterName;
    public Sprite filterSprite;
}
