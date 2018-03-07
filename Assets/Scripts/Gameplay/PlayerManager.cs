using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
   
    public float healthNeededForRez;
    public float healthLossPerSec;
    public float rezRadius;
    public Slider rezBar;
    public Image deathIcon;

    public int rezAmountWhenDead = 0;

    public bool isDead = false;
    
    private float _rezAmount;
    private float _allyDistance;
    private PlayerManager _allyToRez;
    private PlayerManager[] _alliesList;

    private MovementBehaviour movement;

    private void Awake()
    {
        _alliesList = FindObjectsOfType<PlayerManager>();
        movement = GetComponent<MovementBehaviour>();
    }

    void Start()
    {
        if (isDead)
            Death();
    }

    public void Death()
    {
        Debug.Log("I'm dead");
        isDead = true;

        _rezAmount = rezAmountWhenDead;

        // Freeze the player.
        movement.FreezePlayer();

        // Display the dead icon
        deathIcon.enabled = true;
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

        foreach(PlayerManager ally in _alliesList)
        {
            //Ignore self
            if (ally != this)
            {
                float tempAllydistance = Vector3.Distance(transform.position, ally.gameObject.transform.position);

                // Check if ally is within radius of resurrection
                if (tempAllydistance < rezRadius)
                {
                    // Check if ally is dead.
                    if(ally.isDead)
                    {
                        // Store ally if he's near than the last one.
                        if (tempAllydistance < _allyDistance)
                        {
                            _allyDistance = tempAllydistance;
                            _allyToRez = ally;
                        }
                    }
                }
            }
        }

        // Try to rez to the dead ally.
        if(_allyToRez != null)
        {
            _allyToRez.AddHealth();
        }     
    }

    // Losing rez bar progression while downed
    void Update()
    {
        if(isDead)
        {
            if (_rezAmount > 0)
            {
                _rezAmount -= Time.deltaTime * healthLossPerSec;
                deathIcon.enabled = false;
                rezBar.gameObject.SetActive(true);
                rezBar.value = _rezAmount;
            }

            else
            {
                _rezAmount = 0;
                deathIcon.enabled = true;
                rezBar.gameObject.SetActive(false);
            }
        }
        else
        {
            deathIcon.enabled = false;
            rezBar.gameObject.SetActive(false);
        }
    }

    // Player is back in the game
    public void Resurrect()
    {
        Debug.Log("I'm back !");
        isDead = false;

        _rezAmount = 0;
    }
}
