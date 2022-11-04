using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//интерфейсы дл€ драг энд дропа в инвентаре
public class InventoryCell : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public event Action Ejecting;

    [SerializeField] public TMPro.TMP_Text _nameField;
    [SerializeField] private TMPro.TMP_Text _countField;
    [SerializeField] private Image _iconField;

    private Transform _draggingParent;
    private Transform _originalParent;

    private int Count = 1; //количество предметов в €чейке

    public void Init(Transform draggingParent, int count)
    {
        _draggingParent = draggingParent;
        _originalParent = transform.parent;
        Count = count;
    }

    public void Render(AssetItem item)
    {
        _nameField.text = item.Name;
        _iconField.sprite = item.UIIcon;

        if (Count > 1) //пишем количество предметов в —“ј ≈, только если его больше чем 1
            _countField.text = Count.ToString();
        else
            _countField.text = "";
    }

    private bool In(RectTransform originalParent)
    {
        //метод провер€ющий, находитс€ ли объект в зоне инвентар€ или нет, дл€ выкидывани€ его
        return RectTransformUtility.RectangleContainsScreenPoint(originalParent, transform.position);

    }
    private void Eject()
    {
        Ejecting?.Invoke();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        //вз€ли предмет
        transform.parent = _draggingParent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //перетаскиваем предмет 
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (In((RectTransform)_originalParent))
            InsertInGrid();
        else // если предмет за границей инвентар€ - выбрасываем
            Eject();
    }

    private void InsertInGrid()
    {
        //выбросили предмет за границы инвентар€
        int clossestIndex = 0;

        for (int i = 0; i < _originalParent.transform.childCount; ++i)
        {
            //ищем ближайший предмет к нашему предмету
            if (Vector3.Distance(transform.position, _originalParent.GetChild(i).position) <
               Vector3.Distance(transform.position, _originalParent.GetChild(clossestIndex).position))
            {
                clossestIndex = i;

            }
        }

        transform.parent = _originalParent; // присвоили родител€ (контент)
        transform.SetSiblingIndex(clossestIndex); // ресортировка в иерархии (установили предмет в клетке)
    }


}
