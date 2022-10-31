using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//интерфейсы для драг энд дропа в инвентаре
public class InventoryCell : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public event Action Ejecting;

    [SerializeField] private TMPro.TMP_Text _nameField;
    [SerializeField] private Image _iconField;

    private Transform _draggingParent;
    private Transform _originalParent;

    public void Init(Transform draggingParent)
    {
        _draggingParent = draggingParent;
        _originalParent = transform.parent;
    }

    public void Render(AssetItem item)
    {
        _nameField.text = item.Name;
        _iconField.sprite = item.UIIcon;
    }

    private bool In(RectTransform originalParent)
    {
        //метод проверяющий, находится ли объект в зоне инвентаря или нет, для выкидывания его
        return RectTransformUtility.RectangleContainsScreenPoint(originalParent, transform.position);

    }
    private void Eject()
    {
        Ejecting?.Invoke();
        //Debug.Log("Выбросили");
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        //взяли предмет
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
        else // если предмет за границей инвентаря - выбрасываем
            Eject();
    }

    private void InsertInGrid()
    {
        //отпустили предмет
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

        transform.parent = _originalParent; // присвоили родителя (контент)
        transform.SetSiblingIndex(clossestIndex); // ресортировка в иерархии (установили предмет в клетке)
    }


}
