using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    public int NumberOfPlayers = 2;
    public int CurrentPplayerID = 0;
    public int DiceTotal;
    public bool IsDoneRolling = false;
    public bool IsDoneClicking = false;
    public bool IsDoneAnimating = false;
    // Update is called once per frame

    public void NewTurn()
    {
        // This is the start of a player's turn.
        // We don't have a roll for them yet.

        IsDoneRolling   = false;//a dejrlo todo simetrico xDD
        IsDoneClicking  = false;
        IsDoneAnimating = false;
        CurrentPplayerID=(CurrentPplayerID + 1) % NumberOfPlayers;
    }
    void Update()
    {
        if (IsDoneRolling && IsDoneClicking && IsDoneAnimating)
        {
            Debug.Log("Turn is done!");
            NewTurn();
        }

    }
}
