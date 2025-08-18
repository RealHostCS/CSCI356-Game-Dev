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
}
