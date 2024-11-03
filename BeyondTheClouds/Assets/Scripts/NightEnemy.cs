using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEnemy : MonoBehaviour
{
    private List<Dictionary<string, Vector2>> regionList;
    private Transform targetPlayer;
    private Vector2 targetPosition = Vector2.zero;
    private Vector2 currentPos;
    private float playerDistance;
    private bool outside = true;
    private int currentRegion = -1;
    private bool isArrive;
    private bool wandering = true;
    private float boundOffset = 6f;
    private Coroutine attackPlayerRoutine;

    [SerializeField]
    private GameObject light;

    [SerializeField]
    private Transform cloudRegion;

    // Start is called before the first frame update
    void Start()
    {
        InitializeRegions();
        SpawnEnemy();
        targetPlayer = GameObject.FindWithTag("Player").transform;
    }

    private void InitializeRegions() {
        regionList = new List<Dictionary<string, Vector2>>();

        for(int i = 0; i < 4; i++) {
            Dictionary<string, Vector2> region = new Dictionary<string, Vector2>();
            region["minBound"] = new Vector2(0, 0);
            region["maxBound"] = new Vector2(0, 0);
            regionList.Add(region);
        }

        Vector2 cloudRegion_pos = cloudRegion.position;
        Vector2 cloudRegion_scale = cloudRegion.localScale;
        regionList[0]["minBound"] = new Vector2(cloudRegion_pos.x + cloudRegion_scale.x / 2,
            cloudRegion_pos.y - cloudRegion_scale.y / 2);
        regionList[0]["maxBound"] = new Vector2(cloudRegion_pos.x + cloudRegion_scale.x / 2 + boundOffset,
            cloudRegion_pos.y + cloudRegion_scale.y / 2);

        regionList[1]["minBound"] = new Vector2(cloudRegion_pos.x - cloudRegion_scale.x / 2,
            cloudRegion_pos.y + cloudRegion_scale.y / 2);
        regionList[1]["maxBound"] = new Vector2(cloudRegion_pos.x + cloudRegion_scale.x / 2,
            cloudRegion_pos.y + cloudRegion_scale.y / 2 + boundOffset);

        regionList[2]["minBound"] = new Vector2(cloudRegion_pos.x - cloudRegion_scale.x / 2 - boundOffset,
            cloudRegion_pos.y - cloudRegion_scale.y / 2);
        regionList[2]["maxBound"] = new Vector2(cloudRegion_pos.x - cloudRegion_scale.x / 2,
            cloudRegion_pos.y + cloudRegion_scale.y / 2);

        regionList[3]["minBound"] = new Vector2(cloudRegion_pos.x - cloudRegion_scale.x / 2,
            cloudRegion_pos.y - cloudRegion_scale.y / 2 - boundOffset);
        regionList[3]["maxBound"] = new Vector2(cloudRegion_pos.x + cloudRegion_scale.x / 2,
            cloudRegion_pos.y - cloudRegion_scale.y / 2);
    }


    private void SpawnEnemy() {
        currentRegion = Random.Range(0, 4);

        Vector2 minBound = regionList[currentRegion]["minBound"];
        Vector2 maxBound = regionList[currentRegion]["maxBound"];

        float xPos = Random.Range(minBound.x, maxBound.x);
        float yPos = Random.Range(minBound.y, maxBound.y);

        transform.position = new Vector2(xPos, yPos);

        currentPos = transform.position;

        FindNextRegion();
    }

    private void FindNextRegion() {
        float newPosX = 0;
        float newPosY = 0;

        int newRegion = Random.Range(0, 2);
        switch (currentRegion) {
            case 0: {
                    currentRegion = 2 * newRegion + 1;
                    newPosX = regionList[currentRegion]["maxBound"].x;
                    newPosY = Random.Range(regionList[currentRegion]["minBound"].y, regionList[currentRegion]["maxBound"].y);
                }break;
            case 1: {
                    currentRegion = 2 * newRegion;
                    newPosX = Random.Range(regionList[currentRegion]["minBound"].x, regionList[currentRegion]["maxBound"].x);
                    newPosY = regionList[currentRegion]["maxBound"].y;
                }
                break;
            case 2: {
                    currentRegion = 2 * newRegion + 1;
                    newPosX = regionList[currentRegion]["minBound"].x;
                    newPosY = Random.Range(regionList[currentRegion]["minBound"].y, regionList[currentRegion]["maxBound"].y);
                }
                break;
            case 3: {
                    currentRegion = 2 * newRegion;
                    newPosX = Random.Range(regionList[currentRegion]["minBound"].x, regionList[currentRegion]["maxBound"].x);
                    newPosY = regionList[currentRegion]["minBound"].y;
                }
                break;

        }

        Debug.Log(currentRegion);

        targetPosition.x = newPosX;
        targetPosition.y = newPosY;

        isArrive = false;

    }

    private void FixedUpdate() {

        playerDistance = Vector2.Distance(transform.position, targetPlayer.position);

        if(playerDistance <= 8f && wandering) {
            wandering = false;
            currentPos = transform.position;
        }
        else if(playerDistance > 8f) {
            wandering = true;
            outside = true;
        }

        if (wandering) {
            if (Vector2.Distance(targetPosition, transform.position) <= 0.5f) {
                isArrive = true;
                FindNextRegion();
            }

            if (!isArrive) {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, 1.5f * Time.deltaTime);
            }
        }
        else { // look at player
            if (outside) {
                transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, 2f * Time.deltaTime);
            }
        }
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!wandering && collision.CompareTag("NightMap")) {
            Debug.Log((targetPlayer.position - transform.position).normalized);
            outside = false;
            attackPlayerRoutine = StartCoroutine(AttackPlayer());
        }else if (wandering) {
            if(attackPlayerRoutine != null) {
                StopCoroutine(AttackPlayer());
                attackPlayerRoutine = null;

                SpawnEnemy();
            }
        }
    }

    
    IEnumerator AttackPlayer() {
        while (!wandering) {
            yield return new WaitForSeconds(1f);

            Debug.Log("Attack player");
            GameObject attackLight = Instantiate(light, new Vector2(targetPlayer.position.x, targetPlayer.position.y + 2), Quaternion.identity);

            yield return new WaitForSeconds(0.8f);
            Destroy(attackLight);
        }      
        
    }
}
