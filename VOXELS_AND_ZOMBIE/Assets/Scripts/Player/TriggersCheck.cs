using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersCheck : MonoBehaviour
{
    //скрипт игрока для проверки столкновений
    [SerializeField] private Inventory inventory;
    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ItemObject>())
        {
            // добавили лут в список лута в интерфейсе
            inventory.AddItem((other.GetComponent<ItemObject>().item));
            Destroy(other.gameObject);
            Debug.Log("Подняли");
        }
    }

}
