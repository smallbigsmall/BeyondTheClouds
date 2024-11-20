using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCollider : MonoBehaviour
{
    [SerializeField] GameObject rainCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable() {
        StartCoroutine(SetRainActive());
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator SetRainActive() {
        while (true) {
            yield return new WaitForSeconds(0.2f);
            rainCollider.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            rainCollider.SetActive(false);
        }
    }
}
