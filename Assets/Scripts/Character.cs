using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float CharacterSpeed;
    public Vector2 FutureMoveDir;
    public Vector2 CurrentMoveDir;

    private bool _movementChangeSet;

	// Use this for initialization
	void Start ()
    {
        _movementChangeSet = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if ((verticalInput == 0.0f && Mathf.Abs(horizontalInput) == 1.0f) ||
            (horizontalInput == 0.0f && Mathf.Abs(verticalInput) == 1.0f))
        {
            FutureMoveDir = new Vector2(horizontalInput, verticalInput);
        }

        Vector3 translation = Time.deltaTime * CharacterSpeed * CurrentMoveDir;

        Vector3 futurePosition = transform.position + translation;
        int currentX = Mathf.RoundToInt(transform.position.x);
        int currentY = Mathf.RoundToInt(transform.position.y);
        int futureX = Mathf.RoundToInt(futurePosition.x);
        int futureY = Mathf.RoundToInt(futurePosition.y);

        if ((Mathf.Abs(currentX - futureX) >= 1 || Mathf.Abs(currentY - futureY) >= 1) && 
            CurrentMoveDir != FutureMoveDir && !_movementChangeSet)
        {
            _movementChangeSet = true;
        }
        
        if (_movementChangeSet)
        {
            Vector3 newPosition = new Vector3(futureX, futureY);
            float step = CharacterSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);

            if (transform.position == newPosition)
            {
                //change direction
                CurrentMoveDir = FutureMoveDir;
                _movementChangeSet = false;
            }
        }
        else
        {
            //constantly move with the direction applied to it
            transform.Translate(Time.deltaTime * CharacterSpeed * CurrentMoveDir);
        }
    }
}
