using UnityEngine;

public class ConstantSpinner : MonoBehaviour
{
    [Header("Rotation Speeds (degrees per second)")]
    public float xSpeed = 50f;  // rotation around Y axis
    public float zSpeed = 30f;    // rotation around Z axis
    public float ySpeed = 30f;    // rotation around Z axis

    [Header("Rotation Space")]
    public Space rotationSpace = Space.Self; // or Space.World

    void Update()
    {
        // Calculate rotation per frame
        float yRotation = ySpeed * Time.deltaTime;
        float zRotation = zSpeed * Time.deltaTime;
        float xRotation = xSpeed * Time.deltaTime;

        // Apply rotation
        transform.Rotate(xRotation, yRotation, zRotation, rotationSpace);
    }
}
