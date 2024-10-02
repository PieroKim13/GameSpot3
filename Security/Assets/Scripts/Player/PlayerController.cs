using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    enum PlayerState
    {
        Idle = 0,
        Move,
    }
    PlayerState state = PlayerState.Idle;
    PlayerState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                if(state == PlayerState.Idle)
                {

                }
                else if (state == PlayerState.Move)
                {

                }
            }
        }
    }

    PlayerInputAction InputAction;
    public PlayerInputAction playerInputAction => InputAction;
    CharacterController controller;
    public CharacterController Controller => controller;
    CinemachineVirtualCamera cinemachine;
    public CinemachineVirtualCamera Cinemachine => cinemachine;

    /// <summary>
    /// ī�޶� ��ġ
    /// </summary>
    public Transform cameraRoot;

    /// <summary> 
    /// ��ġ���� - (0, 0, 0)��ǥ
    /// </summary>
    Vector3 moveDir = Vector3.zero;

    /// <summary>
    /// ���� �̵��ӵ�
    /// </summary>
    float currentSpeed = 0.0f;

    /// <summary>
    /// �ȱ� �ӵ�
    /// </summary>
    float walkingSpeed = 3.0f;

    /// <summary>
    /// �޸��� �ӵ�
    /// </summary>
    float sprintingSpeed = 4.7f;

    /// <summary>
    /// �޸��� üũ
    /// </summary>
    bool sprintChecking = false;

    /// <summary>
    /// ���� ����
    /// </summary>
    float jumpHeight = 4.0f;

    /// <summary>
    /// ���� üũ
    /// </summary>
    bool jumpChecking = false;

    /// <summary>
    /// ���� ���� Ȯ��
    /// </summary>
    float jumpCheckHeight = 0.0f;

    /// <summary>
    /// �߷� ũ��
    /// </summary>
    float gravity = 9.8f;

    /// <summary>
    /// ���� Ƚ�� - �������� ����
    /// </summary>
    int jumpCount = 0;

    /// <summary>
    /// ��ũ���� �̵��ӵ� ���ҷ�
    /// </summary>
    float crouchDecrease = 1.0f;

    /// <summary>
    /// ��ũ������ Ȯ���ϴ� ����
    /// </summary>
    bool crouchChecking = false;

    /// <summary>
    /// x���� ��ȯ �ΰ���
    /// </summary>
    float rotateSensitiveX = 7.5f;

    /// <summary>
    /// y���� ��ȯ �ΰ���
    /// </summary>
    float rotateSensitiveY = 10.0f;

    /// <summary>
    /// ���� �̵��� y���� ��ȯ
    /// </summary>
    float curRotateY = 0.0f;
    
    /// <summary>
    /// �ٴ��� üũ�ϴ� position�� ���� ���� ũ��
    /// </summary>
    Vector3 boxsize = new Vector3(0.25f, 0.125f, 0.25f);
   
    /// <summary>
    /// �ٴ� ��ġ
    /// </summary>
    Vector3 groundCheckPostion;

    public Action onInteraction;
    public Action onSprinting;
    public Action offSprinting;

    bool ison = false;
    public bool isStamina = false;  // false = �޸��� ��(���׹̳� ȸ��X), true = �⺻ ����(���׹̳� ȸ��O)
    public bool isMove = false; // false = �⺻����, true = �����̴� ��

    private void Start()
    {
        currentSpeed = walkingSpeed;

        StartCoroutine(ShotRaycast());
    }

    private void Awake()
    {
        InputAction = new PlayerInputAction();
        controller = GetComponent<CharacterController>();
        cinemachine = GetComponentInChildren<CinemachineVirtualCamera>();

        cameraRoot = transform.GetChild(0);

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        InputAction.Player.Enable();
        InputAction.Player.Move.performed += OnMove;
        InputAction.Player.Move.canceled += OnMove;
        InputAction.Player.Sprint.performed += OnSprint;
        InputAction.Player.Sprint.canceled += OnSprint;
        InputAction.Player.Jump.performed += OnJump;
        InputAction.Player.Crouch.performed += OnCrouch;
        InputAction.Player.Crouch.canceled += OnCrouch;

        InputAction.Mouse.Enable();
        InputAction.Mouse.MouseVector2.performed += OnMouseDelta;
        InputAction.Mouse.MouseLeftClick.performed += OnMouseLeftClick;
        InputAction.Mouse.MouseLeftClick.canceled += OnMouseLeftClick;
    }

    private void OnDisable()
    {
        InputAction.Mouse.MouseLeftClick.canceled -= OnMouseLeftClick;
        InputAction.Mouse.MouseLeftClick.performed -= OnMouseLeftClick;
        InputAction.Mouse.MouseVector2.performed -= OnMouseDelta;
        InputAction.Mouse.Disable();

        InputAction.Player.Crouch.canceled -= OnCrouch;
        InputAction.Player.Crouch.performed -= OnCrouch;
        InputAction.Player.Jump.performed -= OnJump;
        InputAction.Player.Sprint.canceled -= OnSprint;
        InputAction.Player.Sprint.performed -= OnSprint;
        InputAction.Player.Move.canceled -= OnMove;
        InputAction.Player.Move.performed -= OnMove;
        InputAction.Player.Disable();
    }

    private void Update()
    {
        if (!IsGrounded())
            moveDir.y -= gravity * Time.deltaTime;
        
        if(isMove&&sprintChecking)
        {
            isStamina = false;
            onSprinting?.Invoke();
        }

        // �÷��̾� x, z��ǥ �̵�
        controller.Move(Time.deltaTime * (currentSpeed * crouchDecrease) * transform.TransformDirection(new Vector3(moveDir.x, 0.0f, moveDir.z)));
        // �÷��̾� y��ǥ �̵�
        controller.Move(Time.deltaTime * new Vector3(0.0f, moveDir.y, 0.0f));
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        moveDir.x = dir.x; moveDir.z = dir.y;

        if (context.performed)
        {
            isMove = true;
            State = PlayerState.Move;
        }
        else
        {
            isMove = false;
            State = PlayerState.Idle;
        }
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        // ��ũ���� ���°� �ƴ� ��
        if (!crouchChecking)
        {
            if (context.performed)
            {
                sprintChecking = true;
                currentSpeed = sprintingSpeed;
            }
            else
            {
                // sprintChecking�� true�� ��� = ���׹̳� �� 0���� ���ų� SiftŰ�� ������ �ִ� ���, ���� �ϳ��� false�� ���� ����
                if (sprintChecking) 
                {
                    OffSprinting();
                }
            }
        }
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        // ��ũ���� ���°� �ƴ� ��
        if (!crouchChecking && IsGrounded())
        {
            if (jumpCount < 1)
            {
                // y�̵� ���� ���� ���̷� �Ҵ�
                moveDir.y = jumpHeight;

                if (jumpCount == 0)
                {
                    // ��ǥ ������ ���� ����
                    jumpCheckHeight = transform.position.y + controller.radius * 0.3f;
                }
                // ���� ���� = true
                jumpChecking = true;
                jumpCount++;
            }
        }
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (!sprintChecking && IsGrounded())
        {
            if (context.performed)
            {
                // ���ҷ� 0.5��
                crouchDecrease = 0.5f;
                crouchChecking = true;

                // ���� ���߱� #��ũ���� O
                cameraRoot.transform.position += new Vector3(0f, -0.5f, 0f);
            }
            else
            {
                // ���ҷ� X
                crouchDecrease = 1.0f;
                crouchChecking = false;

                // ���� �ø��� #��ũ���� X
                cameraRoot.transform.position += new Vector3(0f, 0.5f, 0f);
            }
        }
    }

    private void OnMouseDelta(InputAction.CallbackContext context)
    {
        // �Է¹��� ���콺 ��ǥ�� ����
        Vector2 temp = context.ReadValue<Vector2>();
        // �Է¹��� ��ǥ�� x���� ��ȯ �ΰ��� ��ŭ õõ�� �̵�
        float rotateX = temp.x * rotateSensitiveX * Time.fixedDeltaTime;
        // transform�� ����
        transform.Rotate(Vector3.up, rotateX);

        // �Է¹��� ��ǥ�� y���� ��ȯ �ΰ��� ��ŭ õõ�� �̵�
        float rotateY = temp.y * rotateSensitiveY * Time.fixedDeltaTime;
        // ���� y���� ��ȯ �̵����� ���� �̵��� y���� ��ȯ�� ����
        curRotateY -= rotateY;
        // y���� ��ȯ�� �ּ� �� �ִ뷮�� ����
        curRotateY = Mathf.Clamp(curRotateY, -60.0f, 60.0f);
        // �̵��� ���⸸ŭ ī�޶� �̵�
        cameraRoot.rotation = Quaternion.Euler(curRotateY, cameraRoot.eulerAngles.y, cameraRoot.eulerAngles.z);
    }

    private void OnMouseLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ison = true;
        }
        else
        {
            ison = false;
        }
    }

    private bool IsGrounded()
    {
        // ���� ���°� �ƴϰ�, ���� y���̰� ��ǥ y���� ���� ��
        if (jumpChecking && transform.position.y > jumpCheckHeight)
        {
            jumpChecking = false;
        }

        // ĳ���� ������ �ٴ��� üũ�ϴ� ���簢���� ����
        groundCheckPostion = new Vector3(transform.position.x, transform.position.y + controller.radius * -3.0f, transform.position.z);

        // ���簢���� ���̾� "Ground"�� ���� ���
        if (Physics.CheckBox(groundCheckPostion, boxsize, Quaternion.identity, LayerMask.GetMask("Ground")))
        {
            if (!jumpChecking)
            {
                //���� ���̰� ���� �ִ� ���̺��� ���� ��
                if (moveDir.y < jumpHeight)
                {
                    //���� ���̸� -0.01f ����
                    moveDir.y = -0.01f;
                }
                jumpChecking = false;
                jumpCount = 0;
                return true;
            }
        }
        return false;
    }

    public void OffSprinting()
    {
        sprintChecking = false;
        currentSpeed = walkingSpeed;

        offSprinting?.Invoke();
    }

    /// <summary>
    /// ���콺 ��Ŭ�� �ϸ� �ߵ��ϴ� �̺�Ʈ �Լ�
    /// </summary>
    private void OnDetectTarget()
    {
        Ray ray = new(cameraRoot.position, cameraRoot.forward); // ī�޶� ��ġ����, ī�޶� ���� ��������

        // ���̸� ���� �� �Ÿ��� 2.0f �Ÿ� �� �浹�� ���� �� ���(�Ÿ��� 2.0f)
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 2.0f, LayerMask.GetMask("Interaction")))
        {
            
        }
        //Image cross = tra.GetComponent<Image>();
        //Ray ray = new(cameraRoot.position, cameraRoot.forward);
        //if (Physics.Raycast(ray, out RaycastHit hitInfo, 2.0f))
        //{
        //        Debug.Log(hitInfo.transform.name);
        //    if (hitInfo.transform.gameObject.layer == 7)
        //    {
        //        cross.sprite = GameManager.Inst.crossHair2;
        //        if(ison)
        //        {
        //            SingleDoor dor = hitInfo.transform.parent.GetComponent<SingleDoor>();
        //            dor.Interact_Door();
        //            ison = false;
        //        }
        //    }
        //    else
        //        cross.sprite = GameManager.Inst.crossHair1;
        //}
        //else
        //{
        //    cross.sprite = GameManager.Inst.crossHair1;

        //}

    }

    private IEnumerator ShotRaycast()
    {
        int framCount = 0;
        while (true)
        {
            if(framCount >= 15)
            {
                OnDetectTarget();
                framCount = 0;
            }

            framCount++;
            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(groundCheckPostion, boxsize);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(groundCheckPostion, boxsize);
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(cameraRoot.transform.position, 0.25f);

        Gizmos.color = Color.red;
        Vector3 from = cameraRoot.position;
        Vector3 to = cameraRoot.transform.position + cameraRoot.forward * 2.0f;
        Gizmos.DrawLine(from, to);
    }

#endif


}
