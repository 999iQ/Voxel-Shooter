using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<AssetItem> Items;
    [SerializeField] private InventoryCell _inventoryCellTemplate;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _draggingParent; 
    [SerializeField] private Transform _playerTransform;

    [Tooltip("Смещение точки выбрасывания предмета")]
    public Vector3 offset;

    private void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
    }
    public void OnEnable()
    {
        RenderInventory(Items);
    }

    public void AddItem(AssetItem item)
    {
        Items.Add(item); //подобрали предмет
        RenderInventory(Items); //отрисовали инвентарь заново
    }

    public void RenderInventory(List<AssetItem> items)
    {
        foreach (Transform child in _container)
            Destroy(child.gameObject);

        items.ForEach(item =>
        {
            var cell = Instantiate(_inventoryCellTemplate, _container);
            //Debug.Log("Подняли");
            cell.Init(_draggingParent);
            cell.Render(item);

            //выкидывание предмета
            cell.Ejecting += () => Instantiate(item.Object, _playerTransform.position + offset, transform.rotation);
            //удаление объекта из списка
            cell.Ejecting += () => items.Remove(item);
            //если произошел инжектинг, то удаляем объект в клетке
            cell.Ejecting += () => Destroy(cell.gameObject);

        });

    }
}
