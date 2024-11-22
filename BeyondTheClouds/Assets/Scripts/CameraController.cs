using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private float smoothing = 0.2f;
    public Vector2 minCameraBoundary;
    public Vector2 maxCameraBoundary;
    private bool playerLoaded;
    private float defaultSize;
    private Camera ownCamera;

    private void Start() {
        ownCamera = transform.GetComponent<Camera>();
        defaultSize = ownCamera.orthographicSize;
    }
    private void FixedUpdate() {
        if (!playerLoaded) return;

        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }

    public void FindPlayer(Transform player) {
        this.player = player;       
    }

    public void FollowPlayer() {
        playerLoaded = true;
        if (GameManager.Instance.GetCurrentPlayerData().dayCleared) {
            ownCamera.orthographicSize = 8f;
        }
        else {
            ownCamera.orthographicSize = defaultSize;
        }
    }

    public void ShowMap() {
        transform.position = new Vector3(0, 1, -10);
        ownCamera.orthographicSize = 18;
    }

}
