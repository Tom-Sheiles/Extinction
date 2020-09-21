using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayeMovement : MonoBehaviour
{
    public float movementSpeed = 8.0f;
    public float jumpVelocity = 20.0f;
    public float gravity = 10.0f;
    public float jumpThreshold = 0.11f;

    CharacterController controller;
    Vector3 movementVector = Vector3.zero;
    float y = 0;


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
        movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        movementVector = transform.TransformDirection(movementVector);
        movementVector = movementVector.normalized;
        controller.Move(movementVector * movementSpeed * Time.deltaTime);

        if (controller.isGrounded) y = 0;

        if(Input.GetButtonDown("Jump") && canJump())
        {
            y = jumpVelocity;
        }

        y -= gravity * Time.deltaTime;
        controller.Move(new Vector3(0, y, 0) * Time.deltaTime);
    }
}
