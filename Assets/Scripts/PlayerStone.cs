﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStone : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
      theStateManager = GameObject.FindObjectOfType<stateManager>();

        targetPosition = this.transform.position;
    }

    public Tile StartingTile;
    Tile currentTile;
    public stoneStorage myStoneStorage;
    bool scoreMe = false;

    stateManager theStateManager;

    Tile[] moveQueue;
    int moveQueueIndex;
    bool isAnimating = false; 

    Vector3 targetPosition;
    Vector3 velocity;
    float smoothTime = 0.25f;
    float smoothTimeVertical = 0.1f;
    float smoothDistance = 0.01f;
    float smoothHeight = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if(isAnimating==false)
        {
            return;
        }

        if (Vector3.Distance(
                new Vector3(this.transform.position.x, targetPosition.y, this.transform.position.z),
                targetPosition) < smoothDistance)
        {
            // We've reached the target. How's our height?
            if (moveQueue != null && moveQueueIndex == (moveQueue.Length) && this.transform.position.y > smoothDistance)
            {
                this.transform.position = Vector3.SmoothDamp(
                    this.transform.position,
                    new Vector3(this.transform.position.x, 0, this.transform.position.z),
                    ref velocity,
                    smoothTimeVertical);
            }
            else
            {
                // Right position, right height -- let's advance the queue
                AdvanceMoveQueue();
            }
        }
        else if (this.transform.position.y < (smoothHeight - smoothDistance))
        {
            // We want to rise up before we move sideways.
            this.transform.position = Vector3.SmoothDamp(
                this.transform.position,
                new Vector3(this.transform.position.x, smoothHeight, this.transform.position.z),
                ref velocity,
                smoothTimeVertical);
        }
        else
        {
            this.transform.position = Vector3.SmoothDamp(
                this.transform.position,
                new Vector3(targetPosition.x, smoothHeight, targetPosition.z),
                ref velocity,
                smoothTime);
        }

    }

    void AdvanceMoveQueue()
    {
        if (moveQueue != null && moveQueueIndex < moveQueue.Length)
        {
            Tile nextTile = moveQueue[moveQueueIndex];
            if (nextTile == null)
            {
                // We are probably being scored
                // TODO: Move us to the scored pile
                Debug.Log("Scoring tile!");
                SetNewTargetPosition(this.transform.position + Vector3.right * 10f);
            }
            else
            {

                SetNewTargetPosition(nextTile.transform.position);
                moveQueueIndex++;
            }
        }
        else
        {
            Debug.Log("Done Animating!");
            this.isAnimating = false;
            theStateManager.IsDoneAnimating =true;
        }
    }

    void SetNewTargetPosition(Vector3 pos)
    {
        targetPosition = pos;
        velocity = Vector3.zero;
    }

    void OnMouseUp()
    {

        // TODO:  Is the mouse over a UI element? In which case, ignore this click.

        Debug.Log("Click!");

        // Have we rolled the dice?
        if (theStateManager.IsDoneRolling == false)
        {
            // We can't move yet.
            return;
        }
        if (theStateManager.IsDoneClicking == true)
        {
            // We can't move yet.
            return;
        }
        int spacesToMove = theStateManager.DiceTotal;

        if (spacesToMove == 0)
        {
            return;
        }

        // Where should we end up?

        moveQueue = new Tile[spacesToMove];
        Tile finalTile = currentTile;

        for (int i = 0; i < spacesToMove; i++)
        {
            if (finalTile == null && scoreMe == false)
            {
                finalTile = StartingTile;
            }
            else
            {
                if (finalTile.NextTiles == null || finalTile.NextTiles.Length == 0)
                {
                    // TODO: We have reached the end and must score.
                    //Debug.Log("SCORE!");
                    //Destroy(gameObject);
                    //return;
                    scoreMe = true;
                    finalTile = null;
                }
                else if (finalTile.NextTiles.Length > 1)
                {
                    // TODO: Branch based on player id
                    finalTile = finalTile.NextTiles[0];
                }
                else
                {
                    finalTile = finalTile.NextTiles[0];
                }
            }

            moveQueue[i] = finalTile;
        }
        this.transform.SetParent(null);//jajaj become batman 
        moveQueueIndex = 0;
        currentTile = finalTile;
        theStateManager.IsDoneClicking = true;
        this.isAnimating = true;
    }

}
