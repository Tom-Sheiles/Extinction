using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 8.0f;
    public float sprintIncrease = 1.5f;
    public float crouchDecrease = 0.75f;
    public float jumpVelocity = 20.0f;
    public float gravity = 10.0f;
    public float jumpThreshold = 0.11f;

    CharacterController controller;
    Vector3 movementVector = Vector3.zero;
    float y = 0;
    public bool isCrouching = false;


    private bool canJump()
    {
        Vector3 feetPosition = transform.position;
        feetPosition.y -= transform.localScale.y;

        if(Physics.Raycast(feetPosition, Vector3.down, out RaycastHit hit))
        {
            return hit.distance <= jumpThreshold ? true : false;
        }

        return false;
    }


    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    private void Update()
    {
        float speedModifier = 1;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementVector = new Vector3(horizontal, 0, vertical);
        movementVector = transform.TransformDirection(movementVector);
        movementVector = movementVector.normalized;

        if (Input.GetKeyDown(KeyCode.C)) isCrouching = !isCrouching;

        if (isCrouching)
        {
            speedModifier *= crouchDecrease;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift) && vertical > 0)
            {
                speedModifier = sprintIncrease;
            }
        }

        controller.Move(movementVector * (movementSpeed * speedModifier) * Time.deltaTime);

        if (controller.isGrounded) y = 0;

        if(Input.GetButtonDown("Jump") && canJump())
        {
            y = jumpVelocity;
        }

        y -= gravity * Time.deltaTime;
        controller.Move(new Vector3(0, y, 0) * Time.deltaTime);
    }
}
