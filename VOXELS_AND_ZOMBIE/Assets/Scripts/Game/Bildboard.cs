using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bildboard : MonoBehaviour
{
    // скрипт поворачивает объект к игроку (локально) (следит за тобой)
    private Camera _cam;

    private void LateUpdate()
    {
        if (_cam == null)
            _cam = FindObjectOfType<Camera>();

        if (_cam == null)
            return;

        transform.LookAt(_cam.transform);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y+180f, 0f);

    }
}
