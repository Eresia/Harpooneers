using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    public GameObject[] mouettePrefab;
    public GameObject[] dauphinPrefab;
    public GameObject[] poissonPrefab;
    public GameObject[] rocherPrefab;
    public GameObject[] icebergPrefab;

    public Transform[] spawnPositions;

    public Transform[] icebergSpawnPositions;

    void Start()
    {
        StartCoroutine(LDCoroutine());
    }

    public void Spawn(GameObject prefab, Transform prefabTransform, bool isRandom, Transform[] spawnArray)
    {
        if (isRandom)
        {
            int index = Random.Range(0, spawnPositions.Length);
            prefabTransform = spawnArray[index];
        }

       Instantiate(prefab, prefabTransform.position, prefabTransform.rotation, transform);
    }

    IEnumerator LDCoroutine()
    {
        while(true)
        {
            Debug.Log("ICEBEEERG");
            int prefabIndex = Random.Range(0, icebergPrefab.Length);
            int transformIndex = Random.Range(0, icebergSpawnPositions.Length);
            GameObject inst = Instantiate(icebergPrefab[prefabIndex], icebergSpawnPositions[transformIndex].position, Quaternion.identity, transform);
            inst.transform.LookAt(Vector3.zero);

            float randomScale = Random.Range(0.3f, 0.5f);
            inst.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            float randomSpeed = Random.Range(1f, 1.5f);
            inst.GetComponent<ConstantMovement>().speed = randomSpeed;

            yield return new WaitForSeconds(10f);
        }          
    }
}
