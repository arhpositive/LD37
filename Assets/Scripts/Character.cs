using System;
using System.Collections;
using System.Collections.Generic;
using ui;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float CharacterSpeed;
    public float ArtifactSpeedCoef;
    public float SwordSpeedCoef;
    public Vector2 NextMoveDir;
    public Vector2 CurrentMoveDir;
    public string HorizontalAxisName;
    public string VerticalAxisName;
    public int TeamNo;
    public string KeyHelpPanelTagName;

    private bool _movementChangeSet;
    private Vector3 _newPosition;
    private bool _artifactPickedUp;
    private Vector2 _artifactPosition;
    private bool _swordPickedUp;
    private Vector2 _swordPosition;
    private Vector2 _spawnPosition;
    private GameLogic _gameLogicScript;
    private HighlightOnPressScript _highlightOnPressScript;

    // Use this for initialization
    void Start()
    {
        _gameLogicScript = Camera.main.GetComponent<GameLogic>();
        _movementChangeSet = false;
        _artifactPickedUp = false;
        _artifactPosition = Vector2.zero;
        _swordPickedUp = false;
        _swordPosition = Vector2.zero;
        _spawnPosition = transform.position;

        _highlightOnPressScript = GameObject.FindGameObjectWithTag(KeyHelpPanelTagName).GetComponent<HighlightOnPressScript>();
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
        //TODO better adjust keyboard controls wasd fcvb uhjk arrows
        
        if ((verticalInput == 0.0f && Mathf.Abs(horizontalInput) == 1.0f) ||
            (horizontalInput == 0.0f && Mathf.Abs(verticalInput) == 1.0f))
        {

            //skip rest of update if the game has not begun yet
            if (!_gameLogicScript.GameStarted)
            {
                CurrentMoveDir = new Vector2(horizontalInput, verticalInput);
                NextMoveDir = CurrentMoveDir;
                _highlightOnPressScript.HighlightButtonByMoveDir(CurrentMoveDir);
                return;
            }

            _movementChangeSet = false;
            NextMoveDir = new Vector2(horizontalInput, verticalInput);
        }

        

        //we've finished a movement and our current move direction changed
        if (!_movementChangeSet && CurrentMoveDir != NextMoveDir)
        {
            //if we can move on to next block
            bool movementPossible = _gameLogicScript.IsMovementPossible(futureX, futureY);
            if (movementPossible)
            {
                if (_gameLogicScript.IsMovementPossible(futureX + Mathf.RoundToInt(NextMoveDir.x),
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
            if (_gameLogicScript.IsMovementPossible(futureX, futureY))
            {
                transform.Translate(Time.deltaTime*CharacterSpeed*CurrentMoveDir);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (TeamNo != other.gameObject.GetComponent<Character>().TeamNo)
            {
                if (_artifactPickedUp)
                {
                    DropArtifact();
                }
                else if (!_swordPickedUp)
                {
                    RespawnCharacter();
                }
            }
        }
        else if (other.gameObject.tag == "artifact")
        {
            if (!_artifactPickedUp)
            {
                Vector2 artifactPosition = other.transform.position;
                Destroy(other.gameObject);
                PickupArtifact(artifactPosition);
            }
        }
        else if (other.gameObject.tag == "sword")
        {
            if (!_swordPickedUp)
            {
                Vector2 swordPosition = other.transform.position;
                Destroy(other.gameObject);
                PickupSword(swordPosition);
            }
        }
        else if (other.gameObject.tag == "Finish")
        {
            if (_artifactPickedUp)
            {
                DropArtifact();
                _gameLogicScript.ArtifactScored(TeamNo);
                //TODO gain points
            }
        }
    }

    private void RespawnCharacter()
    {
        transform.position = _spawnPosition;
        CurrentMoveDir = Vector2.zero;
        NextMoveDir = Vector2.zero;
    }

    private void PickupArtifact(Vector2 artifactPosition)
    {
        if (_swordPickedUp)
        {
            DropSword();
        }

        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        _artifactPickedUp = true;
        _artifactPosition = artifactPosition;
        CharacterSpeed *= ArtifactSpeedCoef;
    }

    private void DropArtifact()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        _artifactPickedUp = false;
        CharacterSpeed /= ArtifactSpeedCoef;
        
        _gameLogicScript.ReplenishArtifact(_artifactPosition);
        _artifactPosition = Vector2.zero;
    }

    private void PickupSword(Vector2 swordPosition)
    {
        if (_artifactPickedUp)
        {
            DropArtifact();
        }

        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        _swordPickedUp = true;
        _swordPosition = swordPosition;
        CharacterSpeed *= SwordSpeedCoef;
    }

    private void DropSword()
    {
        transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        _swordPickedUp = false;
        CharacterSpeed /= SwordSpeedCoef;

        _gameLogicScript.ReplenishSword(_swordPosition);
        _swordPosition = Vector2.zero;
    }
}
