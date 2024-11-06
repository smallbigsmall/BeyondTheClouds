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

    private void Start() {
        StartCoroutine(FindPlayer());
    }
    private void FixedUpdate() {
        if (!playerLoaded) return;

        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }

    IEnumerator FindPlayer() {
        yield return new WaitForSeconds(0.3f);
        player = FindAnyObjectByType<PlayerMoveController>().transform;
        if (player != null) playerLoaded = true;
    }
}
