using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float health;

    private Rigidbody rb;
    [Header("���-�� ��������")]
    public float speed;
    public float smoothTime;
    private Vector3 smoothMoveVelocity;
    private Vector3 moveAmount;
    public float jumpForce;
    [SerializeField] private IsGroundCheck _isGround;

    [Header("���������������� ���� � ������")]
    public float sensivityMouse;
    private float _mouseY;
    public GameObject cameraHolder;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Cursor.lockState = CursorLockMode.Locked; 
    }

    private void Update()
    {
        GetInput();
        Look();
    }

    private void FixedUpdate()
    {
        // ����������� ��������
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    private void GetInput()
    {
        moveAmount = new Vector3(0, 0, 0);
        smoothMoveVelocity = new Vector3(0, 0, 0);

        // movement
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * speed, ref smoothMoveVelocity, smoothTime);

        // jump
        if (Input.GetKeyDown(KeyCode.Space) && _isGround.IsGround)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    private void Look()
    {
        // �������������� ������� ������ �� �
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * sensivityMouse);

        // ������������ ������� ������ � ������������ ����� � ������
        _mouseY += Input.GetAxisRaw("Mouse Y") * sensivityMouse;
        _mouseY = Mathf.Clamp(_mouseY, -90f, 90f);
        cameraHolder.transform.localEulerAngles = Vector3.left * _mouseY;
    }
}
