using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Item")]
public class AssetItem : ScriptableObject
{
    //настройки каждого предмета в инвентаре
    public string Name; 
    public Sprite UIIcon;
    public GameObject Object; // 3d моделька для выбрасывания из инвентаря
}
