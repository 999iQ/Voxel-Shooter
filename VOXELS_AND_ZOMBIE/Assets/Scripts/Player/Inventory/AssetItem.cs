using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class AssetItem : ScriptableObject
{
    //��������� ������� �������� � ���������
    public string Name; 
    public Sprite UIIcon;
    public GameObject Object; // 3d �������� ��� ������������ �� ���������
    public int maxCount;
    
}
