using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpawner : MonoBehaviour
{
    [SerializeField] float spawnDelay;
    [SerializeField] GameObject wind;

    void Start() {
        StartCoroutine(SpawnWindLoop());
    }


    IEnumerator SpawnWindLoop() {
        while (true) {
            yield return new WaitForSeconds(spawnDelay);
            Instantiate(wind, gameObject.transform.position, Quaternion.Euler(0, 0, 90f), gameObject.transform);
        }
    }
}
