using UnityEngine;

public class InventoryManager : MonoBehaviour 
{
    public GameObject flashlight;
    public GameObject cameraItem;
    public GameObject player;
    private FlashLight flashlightScript;

    public GameObject SprayCan;
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
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipItem(3);
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
            SprayCan.SetActive(false);
        }

        if (currentItem == 2)
        {
            cameraItem.SetActive(true);
            flashlight.SetActive(false);
            flashlightScript.enabled = false;
            SprayCan.SetActive(false);
        }

        if (currentItem == 3)
        {
            cameraItem.SetActive(false);
            flashlight.SetActive(false);
            flashlightScript.enabled = false;
            SprayCan.SetActive(true);
        }
    }
}
