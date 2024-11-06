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
    private Collider2D collider;
    private ContactFilter2D movementFilter;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool isMoving { get; private set; }
    private bool isWalking, onCloud;

    private MainMapManager mainMapManager;

    List<RaycastHit2D> castColisitions = new List<RaycastHit2D>();

    [SerializeField]
    private GameObject wind_projectile;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        mainMapManager = FindAnyObjectByType<MainMapManager>();
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

            if(count <= 0 || onCloud) {
                isWalking = true;
                rigidbody.MovePosition(rigidbody.position + movement * speed * Time.fixedDeltaTime);
            }
            else {
                isWalking = false;
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

    public bool GetIsWalking() {
        return isWalking;
    }

    public Vector2 GetMovement() {
        return movement;
    }

    public void SetOnCloud(bool onCloud) {
        this.onCloud = onCloud;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log($"Collision: {collision.gameObject.name}");

        if (collision.CompareTag("Lighting")) {
            mainMapManager.DecreasePlayerHp(5);
        }
    }

    public void OnPlayerAttack(InputAction.CallbackContext context) {
        if (!GameManager.Instance.GetCurrentPlayerData().dayCleared) return;

        //night time
        if(context.phase == InputActionPhase.Started) {
            Vector3 initPos = transform.position;
            GameObject wind = Instantiate(wind_projectile, initPos, Quaternion.identity);
            wind.GetComponent<WindProjectile>().ShootWind(new Vector2(animator.GetFloat("XDir"), animator.GetFloat("YDir")));
        }
        
    }
}
