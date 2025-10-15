using NUnit.Framework;
using UnityEngine;

public class InventoryManager : MonoBehaviour 
{
    public GameObject flashlight;
    public GameObject cameraItem;

    public GameObject flashlightImage;
    public GameObject cameraImage;
    public GameObject sprayCanImage;
    public GameObject player;
    private FlashLight flashlightScript;

    public GameObject SprayCan;
    private int currentItem = 1;
    public bool hasKey;
    public int CurrentKey;

    
    void Start()
    {
        hasKey = false;
        CurrentKey = 0;
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

    public void PickUpKey(int n)
    {
        CurrentKey = n;
        hasKey = true;
    }

    public void DeActivate(GameObject target)
    {
        target.SetActive(false);
    }
    
    public void Activate(GameObject target)
    {
        target.SetActive(true);
    }

    private void EquipItem(int itemNumber)
    {
        currentItem = itemNumber;

        if (currentItem == 1)
        {
            flashlight.SetActive(true);
            Activate(flashlightImage);
            flashlightScript.enabled = true;
            cameraItem.SetActive(false);
            DeActivate(cameraImage);
            SprayCan.SetActive(false);
            DeActivate(sprayCanImage);
        }

        if (currentItem == 2)
        {
            cameraItem.SetActive(true);
            Activate(cameraImage);
            flashlight.SetActive(false);
            DeActivate(flashlightImage);
            flashlightScript.enabled = false;
            SprayCan.SetActive(false);
            DeActivate(sprayCanImage);
        }

        if (currentItem == 3)
        {
            cameraItem.SetActive(false);
            DeActivate(cameraImage);
            flashlight.SetActive(false);
            Activate(flashlightImage);
            flashlightScript.enabled = false;
            Activate(sprayCanImage);
            SprayCan.SetActive(true);
        }
    }
}
