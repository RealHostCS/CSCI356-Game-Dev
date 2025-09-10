using UnityEngine;

public class PowerUpOrb : MonoBehaviour
{
    public GameObject Monster;
    public GameObject Player;

    private MonsterStats monsterstat;

    void Start()
    {
        // Always assign the reference in Start
        if (Monster != null)
        {
            monsterstat = Monster.GetComponent<MonsterStats>();
            if (monsterstat == null)
            {
                Debug.LogError("MonsterStats component not found on Monster GameObject!");
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
            //MonsterStateChange();
            Destroy(gameObject);
        }
    }
/*
    public void MonsterStateChange()
    {
        if (monsterstat != null)
        {
            monsterstat.currentMonsterState = MonsterStats.MonsterState.Run;
            Debug.Log("Monster state changed to RUN!");
        }
        else
        {
            Debug.LogWarning("MonsterStats reference is missing, cannot change state!");
        }
    }
    */
}
