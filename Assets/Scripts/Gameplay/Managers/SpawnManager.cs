using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject mouettePrefab;
    public GameObject dauphinPrefab;
    public GameObject poissonPrefab;
    public GameObject rocherPrefab;
    public GameObject icebergPrefab;

    public Transform[] spawnPositions;
 
    void Start()
    {
        StartCoroutine(SpawnMouettes());
    }

    public void Spawn(GameObject prefab, Transform prefabTransform, bool isRandom)
    {
        if (isRandom)
        {
            int index = Random.Range(0, spawnPositions.Length);
            prefabTransform = spawnPositions[index];
        }

       GameObject inst =  Instantiate(prefab, prefabTransform.position, prefabTransform.rotation, transform);
    }

    IEnumerator SpawnMouettes()
    {
        for(int i = 0; i < 4; i++)
        {
            Spawn(mouettePrefab, transform, true);
            yield return new WaitForSeconds(5f);
        }          
    }
}
