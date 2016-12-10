﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float CharacterSpeed;
    public Vector2 NextMoveDir;
    public Vector2 CurrentMoveDir;
    public string HorizontalAxisName;
    public string VerticalAxisName;

    private bool _movementChangeSet;
    private Vector3 _newPosition;
    private bool _artifactPickedUp;
    private MapGen _mapGenScript;

    // Use this for initialization
    void Start()
    {
        _mapGenScript = Camera.main.GetComponent<MapGen>();
        _movementChangeSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        //fill current move direction
        int moveDirX = Mathf.RoundToInt(CurrentMoveDir.x);
        int moveDirY = Mathf.RoundToInt(CurrentMoveDir.y);
        
        //fill current position in terms of grid structure
        int currentX = Mathf.FloorToInt(transform.position.x);
        int currentY = Mathf.FloorToInt(transform.position.y);

        if (moveDirX == -1 || moveDirY == -1)
        {
            currentX = Mathf.CeilToInt(transform.position.x);
            currentY = Mathf.CeilToInt(transform.position.y);
        }

        //find out the next block we're gonna land upon
        int futureX = currentX + moveDirX;
        int futureY = currentY + moveDirY;

        //take input and find out next move direction
        float horizontalInput = Input.GetAxisRaw(HorizontalAxisName);
        float verticalInput = Input.GetAxisRaw(VerticalAxisName);
        
        if ((verticalInput == 0.0f && Mathf.Abs(horizontalInput) == 1.0f) ||
            (horizontalInput == 0.0f && Mathf.Abs(verticalInput) == 1.0f))
        {
            _movementChangeSet = false;
            NextMoveDir = new Vector2(horizontalInput, verticalInput);
        }

        //we've finished a movement and our current move direction changed
        if (!_movementChangeSet && CurrentMoveDir != NextMoveDir)
        {
            //if we can move on to next block
            bool movementPossible = _mapGenScript.IsMovementPossible(futureX, futureY);
            if (movementPossible)
            {
                if (_mapGenScript.IsMovementPossible(futureX + Mathf.RoundToInt(NextMoveDir.x),
                    futureY + Mathf.RoundToInt(NextMoveDir.y)))
                {
                    _movementChangeSet = true;
                    _newPosition = new Vector3(futureX, futureY);
                }
            }
            else
            {
                _movementChangeSet = true;
                _newPosition = new Vector3(currentX, currentY);
            }
        }

        if (_movementChangeSet)
        {
            float step = CharacterSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _newPosition, step);

            if (transform.position == _newPosition)
            {
                //change direction
                CurrentMoveDir = NextMoveDir;
                _movementChangeSet = false;
            }
        }
        else
        {
            //constantly move with the direction applied to it
            if (_mapGenScript.IsMovementPossible(futureX, futureY))
            {
                transform.Translate(Time.deltaTime*CharacterSpeed*CurrentMoveDir);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //TODO do stuff, drop artifact, etc.
            if (_artifactPickedUp)
            {
                DropArtifact();
            }
        }
        else if (other.gameObject.tag == "artifact")
        {
            //TODO artifact stuff
            //attach artifact to player

            //destroy game object
            //enable artifact object on player character, switch a boolean on
            //eventually we'll still need to spawn a new artifact after this is gone
            if (!_artifactPickedUp)
            {
                Destroy(other.gameObject);
                PickupArtifact();
            }
        }
    }

    private void PickupArtifact()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        _artifactPickedUp = true;
    }

    private void DropArtifact()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        _artifactPickedUp = false;

        //TODO either drop the artifact on the ground
        //OR reset the artifact by notifying map gen script
    }
}
