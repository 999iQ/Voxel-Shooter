using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInformation // ����� �� ������ �������
{
    public int Count; //���-��
    public AssetItem assetItem; //��� � ������, ���� ���-��
}

public class Inventory : MonoBehaviour
{
    //[SerializeField] private Dictionary <string, string> ItemsName = new(); // ������� ��������� �� �� InstanceID � �����
    //[SerializeField] private Dictionary<string, int> ItemsCount = new(); //������� ��������� �� �� InstanceID � ����������
    [SerializeField] private Dictionary<string, ItemInformation> Items = new();
    [SerializeField] private InventoryCell _inventoryCellTemplate;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _draggingParent;

    [Tooltip("����� ������ ��������� ����� �������")]
    private Transform _targetForEjecting;

    private void Start()
    {
        _targetForEjecting = GameObject.Find("Target for ejecting").transform;
    }
    public void OnEnable()
    {
        RenderInventory(Items);
    }

    public void AddItem(ItemObject item)
    {
        bool flag = false;

        if (item.item.maxCount > 1) //������� ������� ���� �� 1.��������� � 2.��� �� ������ ���� 
        {
            foreach (var itemInCell in Items)
            {
                if(itemInCell.Value.assetItem.Name == item.item.Name)
                {
                    flag = true;

                    // �������� � ����� 40\64 , � �� ����� ��� 24, �� ���� �������  � ��� ����� ��� ���� 64
                    if(itemInCell.Value.Count + item.Count <= item.item.maxCount)
                    {
                        itemInCell.Value.Count += item.Count;
                    }
                    else //���� � ����� ��� ���� ��� ���� ����� ������ �����
                    {
                        int free;
                        if (item.item.maxCount == itemInCell.Value.Count) //���� ���� �� ������� � ��� 0 (64-64)
                        {
                            free = item.Count;
                        }
                        else
                        {
                            free = item.item.maxCount - itemInCell.Value.Count; //������� ��� ����� �������� (64-64)
                            item.Count -= free; // ������� ��� ���-�� � ����, � ��������� � ������� � ����� ����
                            itemInCell.Value.Count = item.item.maxCount; //��� ����
                        }

                        /*//���������� ����������� ����� ������ ����) ***
                        //���������� �������
                        var gameObj = Instantiate(item.gameObject, _targetForEjecting.position, _targetForEjecting.rotation);
                        //������ ���������� �� �������� � ���������� ������
                        gameObj.GetComponent<ItemObject>().Init(free);*/

                        ItemInformation ii = new(); // ������� ����� ������ ������ � ��������� ���
                        ii.Count = free;
                        ii.assetItem = item.item;
                        // �������� ����������� ����� � ������� :3
                        Items.Add(item.GetInstanceID().ToString(), ii); //�������� ����������� �������

                        break;
                    }
                    
                }
            }
        }
        
        if(flag == false)
        {
            ItemInformation ii = new(); // ������� ����� ������ ������ � ��������� ���
            ii.Count = item.Count;
            ii.assetItem = item.item;
            // �������� ����������� ����� � ������� :3
            Items.Add(item.GetInstanceID().ToString(), ii); //�������� ����������� �������
        }

        RenderInventory(Items); //���������� ��������� ������
    }

    public void RenderInventory(Dictionary<string, ItemInformation> items)
    {
        //��������
        foreach (Transform child in _container)
            Destroy(child.gameObject);


        //����������
        //items.ForEach(item =>
        foreach(var item in Items)
        {
            var cell = Instantiate(_inventoryCellTemplate, _container);
            
            cell.Init(_draggingParent, item.Value.Count); //�������������� ������ � ����������� ��������� � ���

            cell.Render(item.Value.assetItem);
            
            //����������� ��������
            cell.Ejecting += () => 
            {
                
                //���������� �������
                var gameObj = Instantiate(item.Value.assetItem.Object, _targetForEjecting.position, _targetForEjecting.rotation);

                //������ ���������� �� �������� � ���������� ������
                gameObj.GetComponent<ItemObject>().Init(item.Value.Count);

                //�������� ������� �� ������
                items.Remove(item.Key);

                //���� ��������� ���������, �� ������� ������ � ������
                Destroy(cell.gameObject);
                
            };

        }

    }
}
