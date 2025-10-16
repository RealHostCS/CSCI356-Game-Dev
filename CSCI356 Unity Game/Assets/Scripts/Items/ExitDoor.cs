using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int doorNumber;
    public string sceneName;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager player = other.GetComponent<InventoryManager>();
            if (player.hasKey == true & player.CurrentKey == doorNumber)
            {
                SelectScene(sceneName);
            }
        }
    }
}

// public class DoorSceneMapping
// {
//     public int doorNumber;
//     public string sceneName;
// }
