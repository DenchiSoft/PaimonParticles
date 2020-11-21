using UnityEngine;

public class CameraFloat : MonoBehaviour
{
    [Range(0, 1)]
    public float BaseSpeed;

    private int noKeyPressedCounter;
    private int noKeyPressedLimit = 120;

    private float leftRightSpeed;
    private float upDownSpeed;
    private float slowdownFactor = 0.91f;

    void FixedUpdate()
    {
        bool keysPressed = false;

        keysPressed |= checkKey(KeyCode.LeftArrow, KeyCode.A, ref leftRightSpeed, BaseSpeed);
        keysPressed |= checkKey(KeyCode.RightArrow, KeyCode.D, ref leftRightSpeed, -BaseSpeed);
        keysPressed |= checkKey(KeyCode.UpArrow, KeyCode.W, ref upDownSpeed, BaseSpeed);
        keysPressed |= checkKey(KeyCode.DownArrow, KeyCode.S, ref upDownSpeed, -BaseSpeed);

        upDownSpeed *= slowdownFactor;
        leftRightSpeed *= slowdownFactor;

        upDownSpeed = Mathf.Abs(upDownSpeed) > 0.001f ? upDownSpeed : 0;
        leftRightSpeed = Mathf.Abs(leftRightSpeed) > 0.001f ? leftRightSpeed : 0;

        var angles = transform.eulerAngles;
        angles.x += upDownSpeed * 0.5f;
        angles.y += leftRightSpeed * 0.5f;
        transform.eulerAngles = angles;

        noKeyPressedCounter = keysPressed ? 0 : noKeyPressedCounter + 1;

        // Auto-rotate if no key pressed.
        if (noKeyPressedCounter > noKeyPressedLimit)
        {
            leftRightSpeed = Mathf.Lerp(0, 0.3f, (noKeyPressedCounter - noKeyPressedLimit) / 150.0f);
        }
    }

    private bool checkKey(KeyCode key1, KeyCode key2, ref float value, float add)
    {
        bool keysPressed = Input.GetKey(key1) || Input.GetKey(key2);
        value += keysPressed ? add : 0;
        return keysPressed;
    }
}
