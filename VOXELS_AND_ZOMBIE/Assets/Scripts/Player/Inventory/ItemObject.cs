using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public AssetItem item;
    [SerializeField] private TMPro.TMP_Text _textName;

    private void Start()
    {
        //подписываем 3d объект его именем
        _textName.text = item.name;
    }
}
