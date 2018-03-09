using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PhaseTransition
{
    public Vector3 camPosition;
    public float transitionTime;
}

public class PhaseTransitionManager : MonoBehaviour {

    public Transform cameraTransform;
    public int currentPhase = -1;
    [Space(20)]
    public PhaseTransition[] phasesCameras;
    public bool gameFinished;
    
    
    // For Debug / Manual
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && !gameFinished)
        {           
            NextPhase();
        }
    }

    public void NextPhase()
    {
        currentPhase++;

        if(currentPhase < phasesCameras.Length)
        {
            StartCoroutine(TransitionToNextPhase(cameraTransform.position, phasesCameras[currentPhase].camPosition, phasesCameras[currentPhase].transitionTime));
        }

        else
        {
            gameFinished = true;

            Debug.Log("You won !");
        }

    }

    IEnumerator TransitionToNextPhase(Vector3 startPos, Vector3 endPos, float time)
    {
        Debug.Log("yolo");
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            cameraTransform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}


