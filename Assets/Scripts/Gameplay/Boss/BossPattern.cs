using UnityEngine;

using System;

public abstract class BossPattern {

    protected Action OnPatternFinished;

	protected PhaseAI boss;

	public virtual void SetBoss(PhaseAI boss)
    {
		this.boss = boss;
	}
    
    public void ExecutePattern(Action action)
    {
        OnPatternFinished = action;

        ExecutePattern();

        // Call OnPaternFinished when the patern is finished.
    }

    protected abstract void ExecutePattern();

    protected abstract void OnStopPattern();

	public void StopPattern() {

        OnStopPattern();
	}
}
