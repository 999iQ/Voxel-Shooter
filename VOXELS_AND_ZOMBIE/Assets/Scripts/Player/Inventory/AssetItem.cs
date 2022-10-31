using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Item")]
public class AssetItem : ScriptableObject
{
    //��������� ������� �������� � ���������
    public string Name; 
    public Sprite UIIcon;
    public GameObject Object; // 3d �������� ��� ������������ �� ���������
}
