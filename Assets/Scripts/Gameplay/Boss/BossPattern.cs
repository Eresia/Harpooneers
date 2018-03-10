using UnityEngine;

using System;

public abstract class BossPattern : MonoBehaviour {

    Action OnPaternFinished;
    
    public void ExecutePatern(Action action)
    {
        OnPaternFinished = action;

        ExecutePatern();

        // Call OnPaternFinished when the patern is finished.
    }

    public abstract void ExecutePatern();
}
