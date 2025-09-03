using System;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{

    public enum MonsterState
    {
        Attack,
        Run
    }

    public MonsterState currentMonsterState = MonsterState.Attack;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Temporary stun function to test camera raycasting
    public void Stun()
    {
        Debug.Log("Monster was stunned by camera");
    }
}
