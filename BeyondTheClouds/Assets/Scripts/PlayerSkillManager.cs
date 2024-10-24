using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public enum PlayerSkill { Creating, Raining, Removing, Cutting}
public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject cloudPrefab;

    private PlayerSkill playerSkill;

    // Start is called before the first frame update
    void Start()
    {
        /*foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin) {
            
            if (!tilemap.HasTile(pos)) {
                Debug.Log($"No tile in {pos}");
            }
            else {
                var tile = tilemap.GetTile<TileBase>(pos);
                Debug.Log($"{pos}: {tile.name}");
            }

        }*/

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            switch (playerSkill) {
                case PlayerSkill.Creating: {
                        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        Vector3Int tilePos = tilemap.WorldToCell(pos);
                        CheckTile(tilePos);
                    }
                    break;
                case PlayerSkill.Raining: {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

                        if (hit.collider == null) return;

                        GameObject hitObj = hit.transform.gameObject;
                        if (hitObj.CompareTag("Cloud")) {
                            MakeRain(hitObj);
                        }
                    }
                    break;
                case PlayerSkill.Removing: {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

                        if (hit.collider == null) return;

                        GameObject hitObj = hit.transform.gameObject;
                        if (hitObj.CompareTag("Cloud")) {
                            Destroy(hitObj);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void CheckTile(Vector3Int pos) {
        TileBase clickedTile = tilemap.GetTile(pos);

        if(clickedTile == null) {
            Vector3Int cloudPos = new Vector3Int(pos.x + 1, pos.y + 1);
            Debug.Log($"Current pos: {pos} and Cloud position: {new Vector3Int(pos.x, pos.y + 1)}");
            GameObject cloudObj = Instantiate(cloudPrefab, tilemap.CellToWorld(cloudPos), Quaternion.identity);
            return;
        }
        int xPos = pos.x;
        int yPos = pos.y;
        while(yPos < tilemap.cellBounds.yMax) {
            yPos += 1;
            TileBase upperTile = tilemap.GetTile(new Vector3Int(xPos, yPos));

            if(upperTile == null) {
                Debug.Log($"Current pos: {pos} and Cloud position: {tilemap.CellToWorld(new Vector3Int(xPos, yPos))}");
                Vector3Int cloudPos = new Vector3Int(xPos + 1, yPos);
                GameObject cloudObj = Instantiate(cloudPrefab, tilemap.CellToWorld(cloudPos), Quaternion.identity);
                return;
            }
        }
    }

    private void MakeRain(GameObject cloud) {
        cloud.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetPlayerSkill(int skillNum) {
        playerSkill = (PlayerSkill)skillNum;
        Debug.Log($"Current skill: {playerSkill}");
    }
}
