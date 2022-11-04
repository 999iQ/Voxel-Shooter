using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public AssetItem item;
    [SerializeField] private TMPro.TMP_Text _textName;
    public int Count = 1;

    public void Init(int count)
    {
        Count = count;
    }
    private void Start()
    {
        //����������� 3d ������ ��� ������ � ����� ��� ���������� � �����
        if(Count > 1)
            _textName.text = ($"{item.name} ({Count})");
        else
            _textName.text = ($"{item.name}");
    }
}
