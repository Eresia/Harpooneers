using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
   
    public float healthNeededForRez;
    public float healthLossPerSec;
    public float rezRadius;
    public float healthPerInput;
    public int rezAmountWhenDead = 0;

    [Header("UI")]
    public Slider rezBar;
    public Image deathIcon;
    public GameObject playerPositionIndicator;
    private Text playerPosText;
    private Camera mainCamera;
    
    public AudioClip[] res_sounds;

    public bool IsDead {
        get { return isDead; }
    }

    // TODO Hide after !!
    public bool isDead = false;
    
    private float _rezAmount;
    private float _allyDistance;
    private PlayerManager _allyToRez;
    private PlayerManager[] _alliesList;

    private MovementBehaviour movement;
    private HarpoonLauncher harpoon;

	public AudioClip death_sound;

    private void Awake()
    {
        _alliesList = FindObjectsOfType<PlayerManager>();
        movement = GetComponent<MovementBehaviour>();
        harpoon = GetComponent<HarpoonLauncher>();

        mainCamera = FindObjectOfType<Camera>();
        playerPosText = playerPositionIndicator.GetComponent<Text>();
    }

    void Start()
    {

        StartCoroutine(TimedFeedbackPlayerPos());

        if (isDead)
        {
            Death();
        }
    }
    
    public void Death()
    {
        // Prevents to kill the player again.
        if (isDead)
        {
            return;
        }

        isDead = true;
        
        _rezAmount = rezAmountWhenDead;

        // Freeze the player and cut the harpoon.
        movement.FreezePlayer();
        harpoon.Cut();

        // Display the dead icon
        deathIcon.enabled = true;
        
        GameManager.instance.shipMgr.NotifyDeath();
		GameManager.instance.audioManager.PlaySoundOneTime (death_sound,0.02f);
    }

    // Called when allies are mashing "A" near your shipwreck
    public void AddHealth()
    {
        _rezAmount += healthPerInput;    
        
        if(_rezAmount >= healthNeededForRez)
        {
            Resurrect();
        }
    }

    // Called when you are mashing "A" near an ally shipwreck
    public void ResurrectFriend()
    {
        _allyDistance = rezRadius;
        _allyToRez = null;

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
            // GameManager.instance.audioManager.PlayRandomSoundOneTimeIn(res_sounds, 0.05f);
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

        if(playerPositionIndicator.activeSelf)
        {
            OrientPositionText();
        }
    }

    // Player is back in the game
    public void Resurrect()
    {
        isDead = false;

        _rezAmount = 0;

        GameManager.instance.shipMgr.NotifyAlive();
    }

    public void FeedbackPlayerPos(bool displayed, int playerId)
    {
        playerPositionIndicator.SetActive(displayed);
        switch(playerId)
        {
            case 0:
                playerPosText.text = "P1";
                playerPosText.color = Color.yellow;
                break;

            case 1:
                playerPosText.text = "P2";
                playerPosText.color = Color.red;
                break;

            case 2:
                playerPosText.text = "P3";
                playerPosText.color = Color.magenta;
                break;

            case 3:
                playerPosText.text = "P4";
                playerPosText.color = Color.green;
                break;
        }
    }

    private void OrientPositionText()
    {
        playerPositionIndicator.transform.LookAt(transform.position - mainCamera.transform.position);
    }

    IEnumerator TimedFeedbackPlayerPos()
    {
        int playerID = GetComponent<PlayerInput>().playerId;
        FeedbackPlayerPos(true, playerID);
        yield return new WaitForSeconds(5f);
        FeedbackPlayerPos(false, playerID);
    }
}
