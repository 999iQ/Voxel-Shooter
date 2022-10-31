using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersCheck : MonoBehaviour
{
    //������ ������ ��� �������� ������������
    [SerializeField] private Inventory inventory;
    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ItemObject>())
        {
            // �������� ��� � ������ ���� � ����������
            inventory.AddItem((other.GetComponent<ItemObject>().item));
            Destroy(other.gameObject);
            Debug.Log("�������");
        }
    }

}
