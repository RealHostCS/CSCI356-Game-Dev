using System;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{

    public enum MonsterState
    {
        Angry,
        scared,
        Attack,
        Hiding,
        stalk,
    }

    public MonsterState currentMonsterState = MonsterState.stalk;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
