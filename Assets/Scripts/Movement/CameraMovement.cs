using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private float RotationX;
    private float RotationY;

    public float Sensitivity;

    public float VerticalRotation;

    public Texture2D cursorTexture;
    Vector2 hotSpot = Vector2.zero;

    private bool LockCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void Update()
    {
        if (LockCamera == false)
        {
            RotationX = Input.GetAxis("Mouse X") * Sensitivity;
            RotationY = Input.GetAxis("Mouse Y") * Sensitivity;
            transform.Rotate(Vector3.up * RotationX);

            VerticalRotation -= RotationY;
            VerticalRotation = Mathf.Clamp(VerticalRotation, -90f, 90f);
        }

        if (LockCamera == false)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(VerticalRotation, 0f, 0f);
        }
    }

    public void LockCursor()
    {
        LockCamera = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    public void UnlockCursor()
    {
        LockCamera = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}
