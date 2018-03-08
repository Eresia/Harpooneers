using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSystem : MonoBehaviour
{
    public float WhaleSpeed = 1f;
    public float MovezoneX = 1f;

    public float TimeBeforePattern = 0f;

    public GameObject WhalePrefab;

    Vector3 north;
    Vector3 west;
    Vector3 south;
    Vector3 east;

    private void Start()
    {
        north =  Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f));
        west = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f));
        south = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0f));
        east = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f));

        StartCoroutine(Pattern());
    }

    private void Move(Transform whale)
    {
        
    }

    private void Rush(Transform whale)
    {
        
    }

    IEnumerator Pattern()
    {
        GameObject whale;

        int side = Random.Range(0, 4);

        Debug.Log(side);
        yield return new WaitForSeconds(TimeBeforePattern);
    
        switch (side)
        {
            case 0:
                whale = Instantiate(WhalePrefab, north + new Vector3 (0, -30, 0), new Quaternion(0f, 180f, 0f, 0f)) as GameObject;
                Move(whale.transform);
                break;
            case 1:
                whale = Instantiate(WhalePrefab, west + new Vector3(0, -30, 0), new Quaternion(0f, 90f, 0f, 0f)) as GameObject;
                Move(whale.transform);
                break;
            case 2:
                whale = Instantiate(WhalePrefab, south + new Vector3(0, -30, 0), new Quaternion(0f, 0f, 0f, 0f)) as GameObject;
                Move(whale.transform);
                break;
            case 3:
                whale = Instantiate(WhalePrefab, east + new Vector3(0, -30, 0), new Quaternion(0f, -90f, 0f, 0f)) as GameObject;
                Move(whale.transform);
                break;
        }
    }
}
