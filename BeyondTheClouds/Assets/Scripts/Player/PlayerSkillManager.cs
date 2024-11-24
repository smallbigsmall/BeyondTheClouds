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
    private Transform player;
    [SerializeField]
    private GameObject cloudPrefab;

    [SerializeField]
    private Tilemap cloudMap, mainMap;

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
                        Debug.Log("Initial Pos: " + pos);
                        CheckTile(hit, pos);
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
                        if(hit.collider.name == "Shadow") {
                            Debug.Log("Shadow");
                            return;
                        }
                        Debug.Log(hit.collider.name);
                        if (hitObj.CompareTag("Cloud")) {
                            if (hitObj.GetComponent<Cloud>().forMoving) return;
                            Cloud cloud = hitObj.transform.GetComponent<Cloud>();
                            StartCoroutine(RemovingCloud(hitObj));
                        }
                    }
                    break;
                case PlayerSkill.Moving: {
                        if (hit.collider != null && hit.collider.CompareTag("CloudMap")) {
                            Debug.Log("Can not create moving cloud");
                            return;
                        }
                        MakingMovingCloud(pos);
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
                                cloudMap.transform.GetComponent<CompositeCollider2D>().isTrigger = true;

                            }
                        }
                    }
                    
                    break;
            }
        }
    }

    private void MakingMovingCloud(Vector2 pos) {
        Vector3Int clickedPos = cloudMap.WorldToCell(pos);
        TileBase clickedTile = cloudMap.GetTile(clickedPos);

        if(clickedTile == null) {
            GameObject cloudObj = Instantiate(cloudPrefab, pos, Quaternion.identity);
            cloudObj.transform.GetComponent<Cloud>().forMoving = true;
        }
    }

    IEnumerator RemovingCloud(GameObject cloud) {
        float totalLife = 2f;
        float life = totalLife;
        while(life > 0) {          
            Color cloudColor = cloud.transform.GetComponent<SpriteRenderer>().color;
            cloud.transform.GetComponent<SpriteRenderer>().color = new Color(
                cloudColor.r, cloudColor.g, cloudColor.b, life / totalLife);
            life -= Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }
        Destroy(cloud);
    }

    private void CheckTile(RaycastHit2D hit, Vector2 pos) {
        if (hit.collider == null) {
            Vector3Int clickedPos = mainMap.WorldToCell(pos);
            TileBase clickedTile = mainMap.GetTile(clickedPos);

            if (clickedTile != null) {
                Vector3 cloudPos = mainMap.CellToWorld(clickedPos);
                GameObject cloudObj = Instantiate(cloudPrefab, cloudPos, Quaternion.identity);
                cloudObj.transform.GetComponent<Cloud>().SetPos(cloudPos);
                return;
            }
            else {
                return;
            }
        }

        Tilemap clickedTilemap;
        CropSetting crop;
        FlowerSetting flower;
        Fire_rainCout fire;

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
                case "WaterfallSoil":
                    cloudPos.y += 3f;
                    GameObject cloudObj4 = Instantiate(cloudPrefab, cloudPos, Quaternion.identity);
                    Debug.Log($"WaterfallSoil: {tilePos}");
                    break;
                default:
                    break;
            }          
        }else if(hit.transform.TryGetComponent<CropSetting>(out crop)) {
            Vector3 cloudPos = new Vector2(pos.x, pos.y + 3f);
            GameObject cloudObj = Instantiate(cloudPrefab, cloudPos, Quaternion.identity);
        }else if(hit.transform.TryGetComponent<FlowerSetting>(out flower)) {
            Vector3 cloudPos = new Vector2(pos.x, pos.y + 3f);
            GameObject cloudObj = Instantiate(cloudPrefab, cloudPos, Quaternion.identity);
        }
        else if (hit.transform.TryGetComponent<Fire_rainCout>(out fire)) {
            Vector3 cloudPos = new Vector2(pos.x, pos.y + 3f);
            GameObject cloudObj = Instantiate(cloudPrefab, cloudPos, Quaternion.identity);
        }       
    }

    public void SetPlayerSkill(int skillNum) {
        if(player == null) {
            player = FindAnyObjectByType<PlayerMoveController>().transform;
        }
        playerSkill = (PlayerSkill)skillNum;
        Debug.Log($"Current skill: {playerSkill}");
    }

    
}
