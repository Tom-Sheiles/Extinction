using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{
    public float mouseSensitivity = 1.0f;
    public float cameraSmoothFactor = 0.1f;
    [SerializeField] GameObject playerCamera = null;
    [SerializeField] Transform thirdPersonPosition = null;
    [SerializeField] Transform firstPersonPosition = null;
    public float cameraSwitchSpeed = 0.5f;
    float targetX = 0, targetY = 0;
    float cameraX = 0, cameraY = 0;

    private bool isThirdPerson = false;


    private void Start()
    {
        firstPersonPosition.position = playerCamera.transform.position;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) isThirdPerson = !isThirdPerson;

        if(isThirdPerson)
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, thirdPersonPosition.position, cameraSwitchSpeed);
            playerCamera.transform.LookAt(transform);
        }
        else
        {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, firstPersonPosition.position, cameraSwitchSpeed);
        }

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
