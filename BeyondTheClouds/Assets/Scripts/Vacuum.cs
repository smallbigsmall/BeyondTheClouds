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

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space) && binEntered && roomCleaner.GetAllCleaned()) {
            Debug.Log("First Day Mission Clear");
        }*/
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
}
