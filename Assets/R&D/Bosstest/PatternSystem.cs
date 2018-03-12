using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PatternSystem : MonoBehaviour
{
    [Header("Preview")]
    public float TimeBeforePattern = 0f;
    [Range(1,5)]
    public int TurnMin = 1; 
    [Range(5,10)]
    public int TurnMax = 5;
    [Range(0,50)]
    public int DistMin;
    [Range(0,50)]
    public int DistMax;
    public float WaitBeforeDash = 0f;

    [Header("Effect")]
    public float WhaleSpeed = 0f;

    [Header("Links")]
    public GameObject WhalePrefab;
    public Transform north;
    public Transform east;
    public Transform south;
    public Transform west;

    private void Start()
    {
        StartCoroutine(Pattern());
    }

    IEnumerator Pattern()
    {
        GameObject whale;

        int side = Random.Range(0, 4);
        
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
        bool _left;
        int turn = Random.Range(TurnMin, TurnMax+1);
        _left = false;

        for(int i = 0; i < turn; i++)
        {
            int distance = Random.Range(DistMin,DistMax);
            if (_left)
                distance *= -1;
            Vector3 destination = whale.position + whale.transform.right * distance;
            float duration = Mathf.Abs(distance/WhaleSpeed);
            whale.DOMove(destination, duration);
            yield return new WaitForSeconds(Mathf.Abs(duration));

            _left = !_left;
        }

        yield return new WaitForSeconds(WaitBeforeDash);
        whale.DOMove(whale.transform.forward * 50f, 1f);
    }
}
