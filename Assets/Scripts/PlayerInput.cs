// MyCharacter.cs - A simple example showing how to get input from Rewired.Player

using UnityEngine;
using System.Collections;
using Rewired;

[RequireComponent(typeof(MovementBehaviour))]
public class PlayerInput : MonoBehaviour
{
    public int playerId = 0; // The Rewired player id of this character
    
    private Player player; // The Rewired Player

    private MovementBehaviour movement;
    private Vector2 inputDir;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerId);

        movement = GetComponent<MovementBehaviour>();
    }

    private void FixedUpdate()
    {
        Vector2 dir = 
    }

    private void ProcessInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            cc.Move(moveVector * moveSpeed * Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        // Free delegates.
    }
}