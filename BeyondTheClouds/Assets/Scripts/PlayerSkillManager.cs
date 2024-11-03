using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

[Serializable]
public enum PlayerSkill { None, Creating, Raining, Removing, Moving}
public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private GameObject cloudPrefab;

    private PlayerSkill playerSkill;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);

            switch (playerSkill) {
                case PlayerSkill.Creating: {
                        CheckTile(hit, pos);
                        //Vector3Int tilePos = tilemap.WorldToCell(pos);
                        //CheckTile(tilePos);
                    }
                    break;
                case PlayerSkill.Raining: {
                        if (hit.transform == null) return;
                        GameObject hitObj = hit.transform.gameObject;
                        if (hitObj.CompareTag("Cloud")) {
                            hitObj.GetComponent<Cloud>().MakeRain();
                        }
                    }
                    break;
                case PlayerSkill.Removing: {
                        GameObject hitObj = hit.transform.gameObject;
                        if (hitObj.CompareTag("Cloud")) {
                            Destroy(hitObj);
                        }
                    }
                    break;
                case PlayerSkill.Moving: { 
                        if(hit.collider == null) {
                            GameObject cloudObj = Instantiate(cloudPrefab, pos, Quaternion.identity);
                            cloudObj.transform.GetComponent<Cloud>().forMoving = true;
                        }
                    }
                    break;
                default: {
                        if (hit.collider == null) return;
                            Cloud cloud;
                        if (hit.transform.TryGetComponent<Cloud>(out cloud)) {
                            if (cloud.forMoving) {
                                Vector3 playerPos = cloud.transform.position;
                                playerPos.y += 1f;
                                player.position = playerPos;
                                cloud.SetOwnerController(player.GetComponent<PlayerMoveController>());
                            }
                        }
                    }
                    
                    break;
            }
        }
    }

    private void CheckTile(RaycastHit2D hit, Vector2 pos) {
        if (hit.collider == null) {
            Debug.Log("No collider, " + hit.transform);
            return;
        }

        Tilemap clickedTilemap;

        if(hit.transform.TryGetComponent<Tilemap>(out clickedTilemap)) {
            Vector3Int tilePos = clickedTilemap.WorldToCell(pos);
            Vector3Int cloudTilePos = new Vector3Int(tilePos.x, tilePos.y+1);
            Vector3 cloudPos = clickedTilemap.CellToWorld(cloudTilePos);
            switch (hit.transform.tag) {
                case "House":                
                    cloudPos.x += 2.5f;
                    GameObject cloudObj = Instantiate(cloudPrefab, cloudPos, Quaternion.identity);
                    cloudObj.transform.localScale = new Vector3(8f, 8f);
                    break;
                case "Tree1":
                    cloudPos = pos;
                    cloudPos.y += 0.5f;
                    GameObject cloudObj1 = Instantiate(cloudPrefab, cloudPos, Quaternion.identity);
                    Debug.Log($"Tree1: {tilePos}");
                    break;
                case "Tree2":
                    cloudPos.x += 0.5f;
                    //cloudPos.y += 1f;
                    GameObject cloudObj2 = Instantiate(cloudPrefab, cloudPos, Quaternion.identity);
                    Debug.Log($"Tree2: {tilePos}");
                    break;
                case "Deco":
                    GameObject cloudObj3 = Instantiate(cloudPrefab, cloudPos, Quaternion.identity);
                    Debug.Log($"Deco: {tilePos}");
                    break;
                default:
                    break;
            }

            /*TileBase clickedTile = clickedTilemap.GetTile(tilePos);

            if (clickedTile == null) {
                Vector3Int cloudPos = new Vector3Int(tilePos.x + 1, tilePos.y + 1);
                Debug.Log($"Current pos: {pos} and Cloud position: {new Vector3Int(tilePos.x, tilePos.y + 1)}");
                GameObject cloudObj = Instantiate(cloudPrefab, clickedTilemap.CellToWorld(cloudPos), Quaternion.identity);
                return;
            }
            int xPos = tilePos.x;
            int yPos = tilePos.y;
            while (yPos < clickedTilemap.cellBounds.yMax) {
                yPos += 1;
                TileBase upperTile = clickedTilemap.GetTile(new Vector3Int(xPos, yPos));

                if (upperTile == null) {
                    Debug.Log($"Current pos: {pos} and Cloud position: {clickedTilemap.CellToWorld(new Vector3Int(xPos, yPos))}");
                    Vector3Int cloudPos = new Vector3Int(xPos + 1, yPos);
                    GameObject cloudObj = Instantiate(cloudPrefab, clickedTilemap.CellToWorld(cloudPos), Quaternion.identity);
                    return;
                }
            }*/
        }
    }

/*    private void CheckTile(Vector3Int pos) {
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
    }*/

    public void SetPlayerSkill(int skillNum) {
        playerSkill = (PlayerSkill)skillNum;
        Debug.Log($"Current skill: {playerSkill}");
    }

    
}
