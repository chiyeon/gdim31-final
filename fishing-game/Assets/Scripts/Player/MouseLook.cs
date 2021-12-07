using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 30.0f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Global.instance.SetFPSMouse(true);      // disable mouse cursor
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseLook = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseLook.y;
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        yRotation += mouseLook.x;

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
