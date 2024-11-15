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
    private new Collider2D collider;
    private ContactFilter2D movementFilter;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool isMoving { get; private set; }
    private bool isWalking, onCloud;
    private bool isNight;
    private Vacuum vacuum = null;
    private Transform playerWand;

    private MainMapManager mainMapManager;

    List<RaycastHit2D> castColisitions = new List<RaycastHit2D>();

    [SerializeField]
    private GameObject wind_projectile, wand;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        mainMapManager = FindAnyObjectByType<MainMapManager>();

        isNight = GameManager.Instance.GetCurrentPlayerData().dayCleared;
        if (isNight) {
            playerWand = Instantiate(wand, transform).transform;
            playerWand.GetComponent<SpriteRenderer>().flipX = true;
            playerWand.localPosition = new Vector2(-0.57f, -0.39f);
        }
        else if(!isNight && transform.childCount > 0) {
            vacuum = transform.GetChild(0).GetComponent<Vacuum>();
        }
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

            if(!isNight && vacuum != null) {
                vacuum.FlipVacuum(movement.x < 0);
            }

            if(isNight && playerWand != null) {
                if(movement.x > 0) { // ->
                    playerWand.GetComponent<SpriteRenderer>().flipX = false;
                    playerWand.localPosition = new Vector2(0.22f, -0.5f);
                }else if(movement.x < 0) {
                    playerWand.GetComponent<SpriteRenderer>().flipX = true;
                    playerWand.localPosition = new Vector2(-0.3f, -0.42f);
                }
                else {
                    playerWand.GetComponent<SpriteRenderer>().flipX = true;
                    playerWand.localPosition = new Vector2(-0.57f, -0.39f);
                }
            }
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
        else if (collision.CompareTag("HouseDoor")) {
            transform.position = new Vector2(2.7f, 0.4f);
        }
        else if (collision.CompareTag("GardenDoor")) {
            transform.position = new Vector2(52.5f, -78.3f);
        }
        else if (collision.CompareTag("Portal")) {
            transform.position = new Vector2(-3.5f,-20f);
        }
    }

    public void OnPlayerAttack(InputAction.CallbackContext context) {
       
        if (context.phase == InputActionPhase.Started) {
            if (isNight) {
                Vector3 initPos =playerWand!=null? playerWand.GetChild(0).position:
                    transform.position;
                GameObject wind = Instantiate(wind_projectile, initPos, Quaternion.identity);
                wind.GetComponent<WindProjectile>().ShootWind(new Vector2(animator.GetFloat("XDir"), animator.GetFloat("YDir")));
            }
            else {
                if (vacuum == null) return;

                if (vacuum.IsFinishingCleaningReady()) { //mission clear
                    mainMapManager.CleaningMissionFinished();
                }
            }
            
        }
        
    }
}
