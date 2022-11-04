using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCheck : MonoBehaviour
{
    //������ ��� �������� ����
    [SerializeField] private Inventory inventory;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject Button_E;
    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        cam = FindObjectOfType<Camera>();
        Button_E = GameObject.Find("Press E");
        Button_E.SetActive(false);
    }

    private void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            if (hit.collider.gameObject.GetComponent<ItemObject>())
            {
                Button_E.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    //�� �������� ������ � �������� ������� ����� �������� � ���� ��� ���������� � �����
                    //inventory.AddItem((hit.collider.gameObject.GetComponent<ItemObject>().item), 
                    //hit.collider.gameObject.GetComponent<ItemObject>().Count);
                    inventory.AddItem(hit.collider.gameObject.GetComponent<ItemObject>());

                    Destroy(hit.collider.gameObject);
                }
            }
            else
            {
                Button_E.SetActive(false);
            }
        }

    }
}
