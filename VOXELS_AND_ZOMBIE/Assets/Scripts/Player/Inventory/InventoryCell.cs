using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//���������� ��� ���� ��� ����� � ���������
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
        //����� �����������, ��������� �� ������ � ���� ��������� ��� ���, ��� ����������� ���
        return RectTransformUtility.RectangleContainsScreenPoint(originalParent, transform.position);

    }
    private void Eject()
    {
        Ejecting?.Invoke();
        //Debug.Log("���������");
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        //����� �������
        transform.parent = _draggingParent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //������������� ������� 
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (In((RectTransform)_originalParent))
            InsertInGrid();
        else // ���� ������� �� �������� ��������� - �����������
            Eject();
    }

    private void InsertInGrid()
    {
        //��������� �������
        int clossestIndex = 0;

        for (int i = 0; i < _originalParent.transform.childCount; ++i)
        {
            //���� ��������� ������� � ������ ��������
            if (Vector3.Distance(transform.position, _originalParent.GetChild(i).position) <
               Vector3.Distance(transform.position, _originalParent.GetChild(clossestIndex).position))
            {
                clossestIndex = i;
            }
        }

        transform.parent = _originalParent; // ��������� �������� (�������)
        transform.SetSiblingIndex(clossestIndex); // ������������ � �������� (���������� ������� � ������)
    }


}
