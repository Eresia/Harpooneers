using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternSystem : MonoBehaviour
{
    [Header("Preview")]
    public float TimeBeforePattern = 0f;
    [Range(1,5)]
    public int TurnMin = 1; 
    [Range(5,10)]
    public int TurnMax = 5;
    [Header("Effect")]
    public float WhaleSpeed = 1f;
    [Header("Links")]
    public GameObject WhalePrefab;
    public Transform north;
    public Transform east;
    public Transform south;
    public Transform west;
    private bool _sideCheck;

    private void Start()
    {
        StartCoroutine(Pattern());
    }

    private void LatDash(Transform whale)
    {
        Random.Range(0,10);
        _sideCheck = !_sideCheck;
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
                whale = Instantiate(WhalePrefab, north.position, Quaternion.Euler(0f, 180f, 0f)) as GameObject;
                StartCoroutine(HMove(whale.transform));
                break;
            case 1:
                whale = Instantiate(WhalePrefab, east.position, Quaternion.Euler(0f, -90f, 0f)) as GameObject;
                StartCoroutine(HMove(whale.transform));
                break;
            case 2:
                whale = Instantiate(WhalePrefab, south.position, Quaternion.Euler(0f, 0f, 0f)) as GameObject;
                StartCoroutine(HMove(whale.transform));
                break;
            case 3:
                whale = Instantiate(WhalePrefab, west.position, Quaternion.Euler(0f, 90f, 0f)) as GameObject;
                StartCoroutine(HMove(whale.transform));
                break;
        }
    }

    IEnumerator HMove(Transform whale)
    {
        int turn = Random.Range(TurnMin, TurnMax+1);
        Debug.Log("Turns = " + turn);

        _sideCheck = false;
        for(int i = 0; i < turn; i++)
        {
            LatDash(whale);
        }

        yield return null;
    }
}
