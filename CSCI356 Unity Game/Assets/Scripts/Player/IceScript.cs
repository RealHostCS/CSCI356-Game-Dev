using UnityEngine;

public class IceScript : MonoBehaviour
{
    public PlayerMovement m_playerscript;

    public void OnTriggerEnter(Collider gameObject)
    {
        if (gameObject.name == "Player")
        {
            m_playerscript.SlipperyFriction();
        }
    }

    public void OnTriggerExit(Collider gameObject)
    {
        if (gameObject.name == "Player")
        {
            m_playerscript.ResetFriction();
        }
    }
}
