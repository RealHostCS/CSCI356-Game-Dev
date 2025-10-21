using UnityEngine;

public class ConstantSpinner : MonoBehaviour
{
    [Header("Rotation Speeds (degrees per second)")]
    public float xSpeed = 50f; 
    public float zSpeed = 30f;    
    public float ySpeed = 30f;   

    [Header("Rotation Space")]
    public Space rotationSpace = Space.Self; 

    void Update()
    {
      
        float yRotation = ySpeed * Time.deltaTime;
        float zRotation = zSpeed * Time.deltaTime;
        float xRotation = xSpeed * Time.deltaTime;

        
        transform.Rotate(xRotation, yRotation, zRotation, rotationSpace);
    }
}
