using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    private float speed = 3f;
    private float collisitionOffset = 0.05f;
    private Vector2 movement;
    private new Rigidbody2D rigidbody;
    private ContactFilter2D movementFilter;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool isMoving { get; private set; }

    List<RaycastHit2D> castColisitions = new List<RaycastHit2D>();
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {

        if (isMoving) {
            
            int count = rigidbody.Cast(
                movement, 
                movementFilter, 
                castColisitions, 
                speed * Time.fixedDeltaTime + collisitionOffset 
            );

            if(count <= 0) {
                rigidbody.MovePosition(rigidbody.position + movement * speed * Time.fixedDeltaTime);
            }

            animator.SetBool("IsWalking", true);
            animator.SetFloat("XDir", movement.x);
            animator.SetFloat("YDir", movement.y);

        }
        else {
            animator.SetBool("IsWalking", false);
        }
    }

    public void OnPlayerMove(InputAction.CallbackContext context){
        movement = context.ReadValue<Vector2>();

        isMoving = movement != Vector2.zero;
        
    }
}
