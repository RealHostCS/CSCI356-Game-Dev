using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject cameraItem;
    public GameObject player;
    private FlashLight flashlightScript;
    private int currentItem = 1;
    void Start()
    {
        flashlightScript = player.GetComponent<FlashLight>();
        EquipItem(1);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItem(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipItem(2);
        }
    }

    private void EquipItem(int itemNumber)
    {
        currentItem = itemNumber;

        if (currentItem == 1)
        {
            flashlight.SetActive(true);
            flashlightScript.enabled = true;
            cameraItem.SetActive(false);
        }

        if (currentItem == 2)
        {
            cameraItem.SetActive(true);
            flashlight.SetActive(false);
            flashlightScript.enabled = false;
        }
    }
}
