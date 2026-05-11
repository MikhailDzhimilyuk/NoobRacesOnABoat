using UnityEngine;
using System.Collections;

public class CameraController : MonoCache
{
    [SerializeField] private Joystick JoystickCamera;
    private float sensitivity = 1.0f;
    private float maxYAngle = 90.0f;
    private float rotationX = 0.0f;

    private void Start()
    {
        PauseController.SetPause();
        StartCoroutine(StartGame());
    }

    public override void OnTick()
    {
        float mouseX;
        float mouseY;
        if (Application.isMobilePlatform)
        {
            mouseX = JoystickCamera.Horizontal * 2;
            mouseY = JoystickCamera.Vertical * 2;
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X") / 2;
            mouseY = Input.GetAxis("Mouse Y") / 2;
        }

        if (PauseController.CheckIsPause() == false)
        {
            float anglesX = transform.localEulerAngles.x;
            float rotX = -mouseY * sensitivity;

            transform.parent.Rotate(0, mouseX * sensitivity, 0);

            if ( !( (anglesX < 8 && rotX < 0) || (anglesX > 38 && rotX > 0) ) )
            {
                transform.Rotate(rotX, 0, 0);
            }

            
            if ((anglesX > 45 || anglesX < -45) || (transform.localEulerAngles.y < 269 || transform.localEulerAngles.y > 271))
            {
                transform.localRotation = Quaternion.Euler(10, 270, 0);
            }

            rotationX -= mouseY * sensitivity;
            rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(1);
        PauseController.SetUnpause();
    }
}
