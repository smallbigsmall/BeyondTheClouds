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
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        targetPosition.x = newPosX;
        targetPosition.y = newPosY;

        isArrive = false;

    }

    private void FixedUpdate() {
        if(transform.position.x > -14) {
            spriteRenderer.flipX = true;
        }
        else {
            spriteRenderer.flipX = false;
        }

        playerDistance = Vector2.Distance(transform.position, targetPlayer.position);

        if(playerDistance <= 8f && wandering) {
            Debug.Log($"{transform.name}: Stop wandering");
            wandering = false;
            currentPos = transform.position;
        }
        else if(playerDistance > 8f && !wandering) {
            ReturnToOutside();
            wandering = true;
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

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("NightMap")) {
            outside = true;
        }
    }

    private void ReturnToOutside() {
        Vector2 currentPos = transform.position;
        if(currentPos.x > -14 && currentPos.y > -81) {
            currentRegion = 0;
        }else if(currentPos.x<-14 && currentPos.y > -81) {
            currentRegion = 1;
        }else if(currentPos.x < -14 && currentPos.y < -81) {
            currentRegion = 2;
        }else if(currentPos.x>-14 && currentPos.y < -81) {
            currentRegion = 3;
        }
        Vector2 minBound = regionList[currentRegion]["minBound"];
        Vector2 maxBound = regionList[currentRegion]["maxBound"];

        float xPos = Random.Range(minBound.x, maxBound.x);
        float yPos = Random.Range(minBound.y, maxBound.y);
        targetPosition = new Vector2(xPos, yPos);

        isArrive = false;
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
