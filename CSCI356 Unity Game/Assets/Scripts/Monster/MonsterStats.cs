using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class MonsterStats : MonoBehaviour
{
    public Transform player;      // Assign your player object in the Inspector
    public NavMeshAgent agent;

    public enum MonsterState
    {
        Attack,
        Run,
        Angry,
        Wait
    }

    public MonsterState currentMonsterState;
    private int stunCount = 0;
    private bool isAngryCoroutineRunning = false;


    private float baseSpeed = 12f;
    private float angrySpeed = 18f;

    private float baseAcceleration = 8f;
    private float angryAcceleration = 50f;
    
    private float baseAngularSpeed = 120f;
    private float angryAngularSpeed = 500f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentMonsterState = MonsterState.Attack;
    }
    
    void Update()
    {
        if (currentMonsterState == MonsterState.Attack)
        {
            agent.speed = baseSpeed;
            agent.acceleration = baseAcceleration;
            agent.angularSpeed = baseAngularSpeed;
            if (player != null)
            {
                agent.SetDestination(player.position);
            }
            Debug.Log("Monster is in state attack with speed " + agent.speed);
        }
        if (currentMonsterState == MonsterState.Wait)
        {
            agent.SetDestination(transform.position);
            Debug.Log("Monster is waiting");
        }
        if (currentMonsterState == MonsterState.Angry)
        {
            agent.speed = angrySpeed;
            agent.acceleration = angryAcceleration;
            agent.angularSpeed = angryAngularSpeed;
            Debug.Log("Monster is angry! Speed = " + agent.speed);
            if (player != null)
            {
                agent.SetDestination(player.position);
            }
            if (!isAngryCoroutineRunning)
            {
                StartCoroutine(AngryDuration());
            }
        }
    }

    //Temporary stun function to test camera raycasting
    public void Stun()
    {
        stunCount++;
        if (stunCount >= 5)
        {
            Debug.Log("Monster is now ANGRY!");
            currentMonsterState = MonsterState.Angry;
            stunCount = 0;
        }
        else 
        {
            currentMonsterState = MonsterState.Wait;
            StartCoroutine(WaitToAttack());
        }
    }

    IEnumerator WaitToAttack()
    {
        yield return new WaitForSeconds(3);
        currentMonsterState = MonsterState.Attack;
    }

    IEnumerator AngryDuration()
    {
        isAngryCoroutineRunning = true;
        yield return new WaitForSeconds(5);
        currentMonsterState = MonsterState.Attack;
        isAngryCoroutineRunning = false;
    }
}
