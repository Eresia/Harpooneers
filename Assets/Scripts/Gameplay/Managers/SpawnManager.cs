using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LDCategory{

    public GameObject[] prefabs;
}


public class SpawnManager : MonoBehaviour {

    public GameObject[] animalsPrefab;
    public Transform[] animalsTransformPos;
    public float spawnDelay;


    [Header("Icebergs")]
    public GameObject[] icebergPrefab;
    public Transform[] icebergSpawnPositions;
    public float minIcebergSpawnTime;
    public float maxIcebergSpawnTime;


    [Header("LD Chunks")]
    public LDCategory[] prefabCategory;
    public Transform[] spawnPositionsLD;
    public float minLDSpawnTime;
    public float maxLDSpawnTime;
    private int lastPrefabIndex = -1;
    private int lastCategoryIndex = -1;
    private int lastTransformIndex = -1;




    void Start()
    {
        //  StartCoroutine(IcebergCoroutine());
        //  StartCoroutine(LDCoroutine());
        StartCoroutine(AnimalsCoroutine());
    }


    /*
    public void Spawn(GameObject prefab, Transform prefabTransform, bool isRandom, Transform[] spawnArray)
    {
        if (isRandom)
        {
            int index = Random.Range(0, spawnPositions.Length);
            prefabTransform = spawnArray[index];
        }

       Instantiate(prefab, prefabTransform.position, prefabTransform.rotation, transform);
    }

    */

    IEnumerator IcebergCoroutine()
    {
        while(true)
        {
            int prefabIndex = Random.Range(0, icebergPrefab.Length);
            int transformIndex = Random.Range(0, icebergSpawnPositions.Length);
            GameObject inst = Instantiate(icebergPrefab[prefabIndex], icebergSpawnPositions[transformIndex].position, Quaternion.identity, transform);
            inst.transform.LookAt(Vector3.zero);

            float randomScale = Random.Range(0.3f, 0.5f);
            inst.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            float randomSpeed = Random.Range(1f, 1.5f);
            inst.GetComponent<ConstantMovement>().speed = randomSpeed;

            yield return new WaitForSeconds(Random.Range(minIcebergSpawnTime, maxIcebergSpawnTime));
        }          
    }

    IEnumerator LDCoroutine()
    {
        while (true)
        {
            int prefabcategoryIndex = Random.Range(0, prefabCategory.Length);
            int prefabIndex = Random.Range(0, prefabCategory[prefabcategoryIndex].prefabs.Length);
            int transformIndex = Random.Range(0, spawnPositionsLD.Length);


            // Pas possible d'instancier 2 fois de suite le même prefab
            while (prefabIndex == lastPrefabIndex && prefabcategoryIndex == lastCategoryIndex)
            {
                prefabcategoryIndex = Random.Range(0, prefabCategory.Length);
                prefabIndex = Random.Range(0, prefabCategory[prefabcategoryIndex].prefabs.Length);
            }

    
            // Pas possible d'instancier 2 fois de suite au même endroit
            while (transformIndex == lastTransformIndex)
            {
                transformIndex = Random.Range(0, spawnPositionsLD.Length);
            }
           
            // Instantiate
            GameObject inst = Instantiate(prefabCategory[prefabcategoryIndex].prefabs[prefabIndex], spawnPositionsLD[transformIndex].position, Quaternion.identity, transform);

            float randomRot = Random.Range(0f, 360f);
            inst.transform.rotation = Quaternion.Euler(0f, randomRot, 0f);

                       
            inst.GetComponent<ConstantMovement>().speed = 0.5f;

            lastTransformIndex = transformIndex;
            lastPrefabIndex = prefabIndex;
            lastCategoryIndex = prefabcategoryIndex;

            yield return new WaitForSeconds(Random.Range(minLDSpawnTime, maxLDSpawnTime));
        }
    }

    IEnumerator AnimalsCoroutine()
    {
        while (true)
        {
            int prefabIndex = Random.Range(0, animalsPrefab.Length);
            int transformIndex = Random.Range(0, animalsTransformPos.Length);


            /* 
            // Pas possible d'instancier 2 fois de suite le même prefab
            while (prefabIndex == lastPrefabIndex)
            {
                prefabIndex = Random.Range(0, animalsPrefab.Length);
            }
            */
            // Pas possible d'instancier 2 fois de suite au même endroit
            while (transformIndex == lastTransformIndex)
            {
                transformIndex = Random.Range(0, spawnPositionsLD.Length);
            }

            GameObject inst = Instantiate(animalsPrefab[prefabIndex], animalsTransformPos[transformIndex].position, Quaternion.identity, transform);


            inst.transform.rotation = animalsTransformPos[transformIndex].rotation;
          //  inst.GetComponent<Animator>().speed = Random.Range(0.5f, 1f);

            lastTransformIndex = transformIndex;
            lastPrefabIndex = prefabIndex;

            Destroy(inst, 60f);

            yield return new WaitForSeconds(spawnDelay);
        }

    }
}
