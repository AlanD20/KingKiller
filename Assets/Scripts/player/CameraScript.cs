using UnityEngine;
using Photon.Pun;

public class CameraScript : MonoBehaviour
{
    public Transform pl;
    // public static float SenseX = 50f;
    // public static float SenseY = 50f;
    // float LookUpDown = 4f;
    //?new camera script
    Vector2 targetDirection;
    Vector2 targetCharacterDirection;
    [SerializeField]
    Vector2 clampInDegrees = new Vector2(360, 120);

    public static Vector2 sensitivity = new Vector2(1f, 0.5f);
    Vector2 smoothing = new Vector2(1, 1);
    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;
    void Start()
    {
        // Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // targetDirection = transform.localRotation.eulerAngles; //! new
        // if (pl)//! new
        //     targetCharacterDirection = pl.transform.localRotation.eulerAngles;//! new
    }

    void Update()
    {
        if (!GameUI.GamePaused)
        {
            /* //? old script
            float mouseX = Input.GetAxis("Mouse X") * SenseX * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * SenseY * Time.deltaTime;
            pl.eulerAngles += Vector3.up * mouseX;
            LookUpDown -= mouseY;
            LookUpDown = Mathf.Clamp(LookUpDown, -30f, 30f);
            transform.localRotation = Quaternion.Euler(LookUpDown, 0f, 0f);
            */

            var targetOrientation = Quaternion.Euler(targetDirection);
            var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);
            var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));
            _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
            _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

            _mouseAbsolute += _smoothMouse;
            if (clampInDegrees.x < 360)
                _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

            if (clampInDegrees.y < 360)
                _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

            transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

            if (pl)
            {
                var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
                pl.transform.localRotation = yRotation * targetCharacterOrientation;
            }
            else
            {
                var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
                transform.localRotation *= yRotation;
            }
        }
    }
}
