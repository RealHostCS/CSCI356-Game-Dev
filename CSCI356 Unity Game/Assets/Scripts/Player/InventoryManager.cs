using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject flashlight;
    public GameObject cameraItem;
    private int currentItem = 1;
    void Start()
    {
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
            cameraItem.SetActive(false);
        }

        if (currentItem == 2)
        {
            cameraItem.SetActive(true);
            flashlight.SetActive(false);
        }
    }
}
