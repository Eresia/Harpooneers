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

    private bool isTransitioning;
    
    // For Debug / Manual
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && currentPhase < phasesCameras.Length && !isTransitioning)
        {           
            NextPhase();
        }
    }

    public void NextPhase()
    {
        currentPhase++;

        if(currentPhase < phasesCameras.Length)
        {
            Debug.Log(cameraTransform.position + " " + phasesCameras[currentPhase].camPosition);

            StartCoroutine(TransitionToNextPhase(cameraTransform.position, phasesCameras[currentPhase].camPosition, phasesCameras[currentPhase].transitionTime));
        }

        else
        {
            GameManager.instance.GameFinished();
        }

    }

    IEnumerator TransitionToNextPhase(Vector3 startPos, Vector3 endPos, float time)
    {
        isTransitioning = true;

        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            cameraTransform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        cameraTransform.position = endPos;

        GameManager.instance.boundaryMgr.UpdateBoundaries();
        GameManager.instance.ground.ratio += 0.5f;

        isTransitioning = false;
    }
}


