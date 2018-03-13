using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CameraManager : MonoBehaviour {

    public Camera cam;

    [Header("Default shake config")]
    public float defaultDuration = 1f;
    public float defaultStrength = 3f;
    public int defaultVibrato = 10;

    private Vector3 originCamPos;
    private Quaternion originCamRot;

    void Awake()
    {
        // TODO WARNING Cam moves and rotates along the game SO change these values !!!

        originCamPos = cam.transform.position;
        originCamRot = cam.transform.rotation;
    }

    public void Shake()
    {
        Shake(defaultDuration, defaultStrength, defaultVibrato);
    }

    public void Shake(float duration, float strength, int vibrato)
    {
        cam.DOShakePosition(duration, strength, vibrato).onComplete = OnShakeFinished;
        cam.DOShakeRotation(duration, 3f);
    }

    private void OnShakeFinished()
    {
        cam.transform.position = originCamPos;
        cam.transform.rotation = originCamRot;
    }

    [ContextMenu("SHAKE")]
    public void TestShake()
    {
        Shake();
    }
}
