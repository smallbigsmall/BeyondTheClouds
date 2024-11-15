using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    private RoomCleaner roomCleaner;
    private SpriteRenderer spriteRenderer;

    private bool binEntered;
    // Start is called before the first frame update
    void Start()
    {
        roomCleaner = FindAnyObjectByType<RoomCleaner>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipVacuum(bool flipX) {
        spriteRenderer.flipX = flipX;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Trash")) {
            roomCleaner.RemoveTrash();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Bin")) {
            binEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Bin")) {
            binEntered = false;
        }
    }

    public bool IsFinishingCleaningReady() {
        return roomCleaner.GetAllCleaned() && binEntered;
    }
}
