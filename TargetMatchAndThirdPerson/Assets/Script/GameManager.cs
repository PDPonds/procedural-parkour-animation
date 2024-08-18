using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Auto_Singleton<GameManager>
{
    public Transform playerTransform;
    public Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
