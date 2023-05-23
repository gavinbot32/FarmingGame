using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 moveInput;
    public bool interactInput;
    public Vector2 facingDir;
    public LayerMask mask;
    public Rigidbody2D rig;
    public SpriteRenderer renderer;

    public int[] seedInventory = new int[3];


    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        renderer = GetComponentInChildren<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {
        rig.velocity = moveInput.normalized * moveSpeed;

    }
    // Update is called once per frame
    void Update()
    {
        if (moveInput.magnitude != 0.0f)
        {
            facingDir = moveInput.normalized;
            renderer.flipX = (moveInput.x == 0) ? renderer.flipX : moveInput.x > 0;
        }
        if (interactInput)
        {
            tryInteractTile();
            interactInput = false;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void tryInteractTile()
    {
        RaycastHit2D hit = Physics2D.Raycast(((Vector2)transform.position + facingDir) - new Vector2(0, .25f), new Vector3(1, 0, 0) * facingDir, 0.5f, mask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Farmtile"))
            {
                FieldTile tile = hit.collider.GetComponent<FieldTile>();
                tile.interact();

            }
        }

    }

    public void onMoveInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();



    }

    public void onInteractInput(InputAction.CallbackContext context)
    {
    
        if(context.phase == InputActionPhase.Performed)
        {
            interactInput = true;
        }
    }
}
