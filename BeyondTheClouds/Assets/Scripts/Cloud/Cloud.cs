using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public Vector2 pos;
    public bool forMoving;
    public bool isRaining;

    private float speed=3f;

    private PlayerMoveController ownerController;
    private new Rigidbody2D rigidbody;
    private MainMapManager mainMapManager;

    [SerializeField]
    private ParticleSystem rainSystem;
   
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        mainMapManager = FindAnyObjectByType<MainMapManager>();
    }

    private void FixedUpdate() {
        if (ownerController == null || rigidbody == null) return;

        if (ownerController.GetIsWalking()) {
            rigidbody.MovePosition(rigidbody.position + ownerController.GetMovement() * speed * Time.fixedDeltaTime);
        }
    }

    public void SetPos(Vector2 pos) {
        this.pos = pos;
    }

    public Vector2 GetPos() {
        return pos;
    }

    
    public void SetOwnerController(PlayerMoveController controller) {
        ownerController = controller;
        ownerController.SetOnCloud(true);
        ownerController.SetMovingCloud(gameObject);
        mainMapManager.SetSkillPanel(true, false);
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    public void MakeRain() {
        if (forMoving) return;
        var cloudColor = GetComponent<SpriteRenderer>().color;
        if (cloudColor.a < 1) return;
        rainSystem.gameObject.SetActive(true);
    }

    /*private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Border")) {
            mainMapManager.SetSkillPanel(true, false);
        }
    }*/

}
