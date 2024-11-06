using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightEnemy : MonoBehaviour
{
    
    private Transform targetPlayer;
    private Vector2 targetPosition = Vector2.zero;
    private Vector2 currentPos;
    private int currentRegion;
    private float playerDistance;
    private bool outside = true;
    private List<Dictionary<string, Vector2>> regionList;


    private bool isArrive;
    private bool wandering = false;
   
    private Coroutine attackPlayerRoutine;

    private int currentlife;
    private int totalLife;
    private SpriteRenderer lightSprite;

    [SerializeField]
    private GameObject lightAttackObj;

    

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = GameObject.FindWithTag("Player").transform;
        lightSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
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
                }
                break;
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
            }
        }
    }

    
    IEnumerator AttackPlayer() {
        while (!wandering) {
            yield return new WaitForSeconds(1f);

            Debug.Log("Attack player");
            GameObject attackLight = Instantiate(lightAttackObj, new Vector2(targetPlayer.position.x, targetPlayer.position.y + 2), Quaternion.identity);

            yield return new WaitForSeconds(0.8f);
            Destroy(attackLight);
        }      
        
    }

    public void Hit() {
        MainMapManager mainMapManager;
        currentlife--;
        var lightSpriteColor = lightSprite.color;
        lightSpriteColor.a = (float)currentlife / totalLife;

        lightSprite.color = lightSpriteColor;
        if (currentlife == 0) {
            mainMapManager = FindAnyObjectByType<MainMapManager>();
            mainMapManager.DecreaseNightEnemyCount();
            Destroy(gameObject);
        }
    }

    public void StartWandering() {
        FindNextRegion();
        wandering = true;
    }

    public void SetCurrentPos(Vector2 pos) {
        transform.position = pos;
    }

    public void SetTotalLife(int life) {
        totalLife = life;
        currentlife = totalLife;
    }

    public void SetRegionList(List<Dictionary<string, Vector2>> regionList) {
        this.regionList = regionList;
    }

    public void SetCurrentRegion(int region) {
        currentRegion = region;
    }
}
