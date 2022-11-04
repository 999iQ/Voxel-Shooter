using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInformation // класс на каждый предмет
{
    public int Count; //кол-во
    public AssetItem assetItem; //имя и иконка, макс кол-во
}

public class Inventory : MonoBehaviour
{
    //[SerializeField] private Dictionary <string, string> ItemsName = new(); // словарь предметов по их InstanceID и имени
    //[SerializeField] private Dictionary<string, int> ItemsCount = new(); //словарь предметов по их InstanceID и количеству
    [SerializeField] private Dictionary<string, ItemInformation> Items = new();
    [SerializeField] private InventoryCell _inventoryCellTemplate;
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _draggingParent;

    [Tooltip("Точка спавна предметов перед игроком")]
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

        if (item.item.maxCount > 1) //стакаем предмет если он 1.стакается и 2.ещё не полный стак 
        {
            foreach (var itemInCell in Items)
            {
                if(itemInCell.Value.assetItem.Name == item.item.Name)
                {
                    flag = true;

                    // например в стаке 40\64 , а мы нашли ещё 24, то если подберём  у нас будет фул стак 64
                    if(itemInCell.Value.Count + item.Count <= item.item.maxCount)
                    {
                        itemInCell.Value.Count += item.Count;
                    }
                    else //если в стаке уже МАКС или если сумма больше МАКСа
                    {
                        int free;
                        if (item.item.maxCount == itemInCell.Value.Count) //если ФУЛЛ то разница и так 0 (64-64)
                        {
                            free = item.Count;
                        }
                        else
                        {
                            free = item.item.maxCount - itemInCell.Value.Count; //сколько ещё можно впихнуть (64-64)
                            item.Count -= free; // забрали это кол-во в стак, а остальное в ПОЛОЖИМ В новый стак
                            itemInCell.Value.Count = item.item.maxCount; //фул стак
                        }

                        /*//оставшееся выбрасываем иначе теряем вещи) ***
                        //заспавнили предмет
                        var gameObj = Instantiate(item.gameObject, _targetForEjecting.position, _targetForEjecting.rotation);
                        //отдали количество на хранение в предметный скрипт
                        gameObj.GetComponent<ItemObject>().Init(free);*/

                        ItemInformation ii = new(); // создали НОВЫЙ шаблон класса и заполнили его
                        ii.Count = free;
                        ii.assetItem = item.item;
                        // запихали заполненный класс в словарь :3
                        Items.Add(item.GetInstanceID().ToString(), ii); //добавили подобранный предмет

                        break;
                    }
                    
                }
            }
        }
        
        if(flag == false)
        {
            ItemInformation ii = new(); // создали НОВЫЙ шаблон класса и заполнили его
            ii.Count = item.Count;
            ii.assetItem = item.item;
            // запихали заполненный класс в словарь :3
            Items.Add(item.GetInstanceID().ToString(), ii); //добавили подобранный предмет
        }

        RenderInventory(Items); //отрисовали инвентарь заново
    }

    public void RenderInventory(Dictionary<string, ItemInformation> items)
    {
        //отчистка
        foreach (Transform child in _container)
            Destroy(child.gameObject);


        //заполнение
        //items.ForEach(item =>
        foreach(var item in Items)
        {
            var cell = Instantiate(_inventoryCellTemplate, _container);
            
            cell.Init(_draggingParent, item.Value.Count); //инициализируем клетку с количеством элементов в ней

            cell.Render(item.Value.assetItem);
            
            //выкидывание предмета
            cell.Ejecting += () => 
            {
                
                //заспавнили предмет
                var gameObj = Instantiate(item.Value.assetItem.Object, _targetForEjecting.position, _targetForEjecting.rotation);

                //отдали количество на хранение в предметный скрипт
                gameObj.GetComponent<ItemObject>().Init(item.Value.Count);

                //удаление объекта из списка
                items.Remove(item.Key);

                //если произошел инжектинг, то удаляем объект в клетке
                Destroy(cell.gameObject);
                
            };

        }

    }
}
