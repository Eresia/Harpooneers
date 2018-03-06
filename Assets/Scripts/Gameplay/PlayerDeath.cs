using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeath : MonoBehaviour {
   
    public float healthNeededForRez;
    public float healthLossPerSec;
    public float rezRadius;
   
    public bool isDead = false;
    private float _rezAmount;
    private float _allyDistance;
    private PlayerDeath _allyToRez;
    private PlayerDeath[] _alliesList;

    void Start()
    {
        if (isDead)
            Death();

        _alliesList = FindObjectsOfType<PlayerDeath>();
    }

    public void Death()
    {
        Debug.Log("I'm dead");
        isDead = true;

        // Temporary indicator of death
        transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
    }

    // Called when allies are mashing "A" near your shipwreck
    public void AddHealth()
    {
        _rezAmount += 1;    
        
        if(_rezAmount >= healthNeededForRez)
        {
            Resurrect();
        }
    }

    // Called when you are mashing "A" near an ally shipwreck
    public void ResurrectFriend()
    {
        _allyDistance = rezRadius;

        foreach(PlayerDeath ally in _alliesList)
        {
            //Ignore self
            if (transform.position != ally.gameObject.transform.position)
            {
                float tempAllydistance = Vector3.Distance(transform.position, ally.gameObject.transform.position);
                // Check if ally is within radius of resurrection
                if (tempAllydistance < rezRadius)
                {
                    if(tempAllydistance < _allyDistance)
                    {
                        _allyDistance = tempAllydistance;
                        _allyToRez = ally;
                    }
                }
            }
        }
        if(_allyToRez != null)
        {
            if (_allyToRez.isDead)
            {
                _allyToRez.AddHealth();
            }
        }     
    }

    // Losing rez bar progression while downed
    void Update()
    {
        if(_rezAmount > 0)
        {
            _rezAmount -= Time.deltaTime * healthLossPerSec;
        }
        else
        {
            _rezAmount = 0;
        }  
    }

    // Player is back in the game
    public void Resurrect()
    {
        Debug.Log("I'm back !");
        isDead = false;

        _rezAmount = 0f;

        // Temporary indicator of death
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

	
}
