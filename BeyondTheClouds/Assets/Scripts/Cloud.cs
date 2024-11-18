using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Vector2Int pos;
    public bool forMoving;
    public bool isRaining;

    private float speed=3f;

    private PlayerMoveController ownerController;
    private new Rigidbody2D rigidbody;
    private MainMapManager mainMapManager;

    [SerializeField]
    private ParticleSystem rainSystem;
   
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        mainMapManager = FindAnyObjectByType<MainMapManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if (ownerController == null || rigidbody == null) return;

        if (ownerController.GetIsWalking()) {
            rigidbody.MovePosition(rigidbody.position + ownerController.GetMovement() * speed * Time.fixedDeltaTime);
        }
    }

    public void SetTilePos(Vector2Int pos) {
        this.pos = pos;
    }

    public Vector2Int GetTilePos() {
        return pos;
    }

    public void SetOwnerController(PlayerMoveController controller) {
        ownerController = controller;
        ownerController.SetOnCloud(true);
        ownerController.SetMovingCloud(gameObject);
        //rigidbody = transform.gameObject.AddComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    public void MakeRain() {
        if (forMoving) return;
        var cloudColor = GetComponent<SpriteRenderer>().color;
        if (cloudColor.a < 1) return;
        rainSystem.gameObject.SetActive(true);
/*        ParticleSystem particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        particle.Play();*/
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Border")) {
            mainMapManager.SetSkillPanel(true, false);
        }
    }

    private void OnParticleCollision(GameObject other) {
        Debug.Log("Collision in cloud: "+other.name);
    }

}
