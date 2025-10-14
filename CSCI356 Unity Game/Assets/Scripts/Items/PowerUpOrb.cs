using UnityEngine;

public class PowerUpOrb : MonoBehaviour
{
    public GameObject Monster;
    public GameObject Player;

    private MonsterStates monsterstate;

    void Start()
    {
        // Always assign the reference in Start
        if (Monster != null)
        {
            monsterstate = Monster.GetComponent<MonsterStates>();
            if (monsterstate == null)
            {
                Debug.LogError("MonsterStates component not found on Monster GameObject!");
            }
        }
        else
        {
            Debug.LogError("Monster GameObject not assigned in Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Monster State Change");
        if (other.CompareTag("Player"))
        {
            MonsterStateChange();
            Destroy(gameObject);
        }
    }

    public void MonsterStateChange()
    {
        if (monsterstate != null)
        {
            monsterstate.currentMonsterState = MonsterStates.MonsterState.attack;
            Debug.Log("Monster state changed to RUN!");
        }
        else
        {
            Debug.LogWarning("MonsterStats reference is missing, cannot change state!");
        }
    }
}
