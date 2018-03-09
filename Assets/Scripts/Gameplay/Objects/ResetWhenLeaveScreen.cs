using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetWhenLeaveScreen : MonoBehaviour {

    public BoundaryManager boundaryMgr;

    public IResetable resetable;

    [HideInInspector]
    public bool isReseting;
    
    private void Update()
    {
        if(isReseting)
        {
            return;
        }

        if(Time.frameCount%30 == 0)
        {
            if(boundaryMgr.IsInScreen(transform.position))
            {
                resetable.ResetGameObject();
            }
        }
    }
}
