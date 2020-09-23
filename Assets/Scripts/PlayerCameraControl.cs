using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    public float mouseSensitivity = 1.0f;
    public float cameraSmoothFactor = 0.1f;
    public float crouchHeight = 0.35f;
    public float crouchSpeed = 0.4f;
    [SerializeField] GameObject playerCamera = null;
    [SerializeField] Transform thirdPersonPosition = null;
    [SerializeField] Transform firstPersonPosition = null;
    private PlayerMovement playerMovement;
    public float cameraSwitchSpeed = 0.5f;
    float targetX = 0, targetY = 0;
    float cameraX = 0, cameraY = 0;

    private bool isThirdPerson = false;


    private void getStateInput()
    {
        if (Input.GetKeyDown(KeyCode.F)) isThirdPerson = !isThirdPerson;
        if(Input.GetButtonDown("Escape")) { }
    }


    private void checkCameraMode()
    {
        if (isThirdPerson)
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, thirdPersonPosition.position, cameraSwitchSpeed);
            playerCamera.transform.LookAt(transform);
        }
        else
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, firstPersonPosition.position, cameraSwitchSpeed);
        }
    }


    private void handleCrouch()
    {
        if (!playerMovement.isCrouching)
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, firstPersonPosition.position, crouchSpeed);
        }
        else
        {
            Vector3 crouchPosition = firstPersonPosition.position;
            crouchPosition.y -= crouchHeight;
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, crouchPosition, crouchSpeed);
        }
    }


    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        firstPersonPosition.position = playerCamera.transform.position;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        checkCameraMode();
        handleCrouch();

        targetX += -Input.GetAxis("Mouse Y");
        targetY += Input.GetAxis("Mouse X");

        targetX = Mathf.Clamp(targetX, -45, 45);

        cameraX = Mathf.Lerp(cameraX, targetX, cameraSmoothFactor);
        cameraY = Mathf.Lerp(cameraY, targetY, cameraSmoothFactor);

        Vector3 cameraMovement = new Vector3(cameraX, cameraY, 0) * mouseSensitivity;

        playerCamera.transform.eulerAngles = cameraMovement;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, playerCamera.transform.eulerAngles.y, 0);
    }
}
