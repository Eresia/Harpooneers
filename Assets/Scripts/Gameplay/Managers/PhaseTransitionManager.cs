using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[System.Serializable]
public struct PhaseTransition
{
    public Vector3 camPosition;
    public Vector3 camRotation;
    public float transitionTime;
}

public class PhaseTransitionManager : MonoBehaviour {

    public Transform cameraTransform;

    // TO REMOVE WHEN FINISHED.
    public int currentPhase = -1;

    [Space(20)]
    public PhaseTransition[] phasesCameras;

    private bool isTransitioning;
    
    public Action OnTransitionFinished;

    // For Debug / Manual
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && currentPhase < phasesCameras.Length && !isTransitioning)
        {           
            NextPhase(currentPhase++);
        }
    }

    public void NextPhase(int currentPhase)
    {
        if(currentPhase-1 > 0 && currentPhase < phasesCameras.Length)
        {
            StartCoroutine(TransitionToNextPhase(phasesCameras[currentPhase-1], phasesCameras[currentPhase], phasesCameras[currentPhase].transitionTime));
        }
    }

    IEnumerator TransitionToNextPhase(PhaseTransition from, PhaseTransition to, float time)
    {
        isTransitioning = true;

        float elapsedTime = 0;

        Quaternion startRot = Quaternion.Euler(from.camRotation);
        Quaternion endRot = Quaternion.Euler(to.camRotation);

        WaitForEndOfFrame wait = new WaitForEndOfFrame();

        while (elapsedTime < time)
        {
            float t = elapsedTime / time;

            cameraTransform.position = Vector3.Lerp(from.camPosition, to.camPosition, t);
            cameraTransform.rotation = Quaternion.Lerp(startRot, endRot, t);

            elapsedTime += Time.deltaTime;
            yield return wait;
        }

        cameraTransform.position = to.camPosition;

        GameManager.instance.boundaryMgr.UpdateBoundaries();
        GameManager.instance.ground.ratio += 0.5f;

        OnTransitionFinished();
        isTransitioning = false;
    }
}


