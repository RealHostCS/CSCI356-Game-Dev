using UnityEngine;

public class FlashLight : MonoBehaviour
{
    // Assign the flashlight GameObject in the Inspector
    public GameObject flashlight;

    private bool isOn = true;

    void Start()
    {
        if (flashlight == null)
        {
            Debug.LogError("Flashlight GameObject not assigned in Inspector!");
        }
    }

    void Update()
    {
        // Left mouse button toggles light
        if (Input.GetMouseButtonDown(0))
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        isOn = !isOn;
        flashlight.SetActive(isOn); // enable/disable entire GameObject
    }
}
