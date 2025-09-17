using UnityEngine;

public class MudScript : MonoBehaviour
{
    public PlayerMovement m_playerscript;

    public void OnTriggerEnter(Collider gameObject)
    {
        if (gameObject.name == "Player")
        {
            m_playerscript.StartSlow();
        }
    }

    public void OnTriggerExit(Collider gameObject)
    {
        if (gameObject.name == "Player")
        {
            m_playerscript.ResetSpeed();
        }
    }
}
