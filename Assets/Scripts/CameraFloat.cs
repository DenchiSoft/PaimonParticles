using UnityEngine;

public class CameraFloat : MonoBehaviour
{
    [Range(0, 1)]
    public float BaseSpeed;

    private float leftRightSpeed;
    private float upDownSpeed;
    private float slowdownFactor = 0.91f;

    void FixedUpdate()
    {
        checkKey(KeyCode.LeftArrow, KeyCode.A, ref leftRightSpeed, BaseSpeed);
        checkKey(KeyCode.RightArrow, KeyCode.D, ref leftRightSpeed, -BaseSpeed);
        checkKey(KeyCode.UpArrow, KeyCode.W, ref upDownSpeed, BaseSpeed);
        checkKey(KeyCode.DownArrow, KeyCode.S, ref upDownSpeed, -BaseSpeed);

        upDownSpeed *= slowdownFactor;
        leftRightSpeed *= slowdownFactor;

        upDownSpeed = Mathf.Abs(upDownSpeed) > 0.001f ? upDownSpeed : 0;
        leftRightSpeed = Mathf.Abs(leftRightSpeed) > 0.001f ? leftRightSpeed : 0;

        var angles = transform.eulerAngles;
        angles.x += upDownSpeed * 0.5f;
        angles.y += leftRightSpeed * 0.5f;
        transform.eulerAngles = angles;
    }

    private void checkKey(KeyCode key1, KeyCode key2, ref float value, float add)
    {
        value += (Input.GetKey(key1) || Input.GetKey(key2)) ? add : 0;
    }
}
