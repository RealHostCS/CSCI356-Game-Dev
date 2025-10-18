using UnityEngine;

public class ZeldaMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Animation")]
    public Animator anim;
    private Vector2 lastMoveDirection;

    private CharacterController controller;

    [Header("Attack")]
    public GameObject hitboxPrefab;
    private GameObject activeHitbox;

    public float hitboxLifeTime = 0.2f;
    public float hitboxDistance = 1f;


    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!controller)
            Debug.LogError("ZeldaSpriteMovement requires a CharacterController!");

        if (!anim)
            anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    private void HandleMovement()
    {
        // Raw input (snappy)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 moveInput = new Vector2(horizontal, vertical);
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;

        // Move with CharacterController
        controller.Move(move * Time.deltaTime);

        // 🎬 Animator updates
        anim.SetFloat("MoveX", moveInput.x);
        anim.SetFloat("MoveY", moveInput.y);
        anim.SetFloat("MoveMagnitude", moveInput.sqrMagnitude);

        // Store last facing direction
        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveInput;
            anim.SetFloat("LastMoveX", lastMoveDirection.x);
            anim.SetFloat("LastMoveY", lastMoveDirection.y);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Attack");

             if (hitboxPrefab != null)
            {
                Vector3 spawnPos = transform.position + new Vector3(lastMoveDirection.x, 0, lastMoveDirection.y) * hitboxDistance;
                activeHitbox = Instantiate(hitboxPrefab, spawnPos, Quaternion.identity);
                activeHitbox.transform.forward = new Vector3(lastMoveDirection.x, 0, lastMoveDirection.y);


            }
        }
    }
}
