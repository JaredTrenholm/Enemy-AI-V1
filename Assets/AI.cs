using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    enum State
    {
        patrolling,
        chasing,
        searching,
        attacking,
        retreating
    }

    private float TimeBeforeSwitching = 0f;
    private float TimeTarget = 1f;
    private Vector3 PlayerLastPos;
    public static int PatrolAmount = 4;
    public Vector3[] PatrolPos = new Vector3[PatrolAmount];
    private int PatrolTarget = 0;
    private State state;

    public GameObject Player;
    public NavMeshAgent agent;

    public Sight sight;

    public GameObject Patrolling;
    public GameObject Searching;
    public GameObject Chasing;
    public GameObject Attacking;
    public GameObject Retreating;
    private float Waiting = 0f;
    private float WaitTarget = 2f;


    private void Update()
    {
        Debug.Log(state);
        switch (state)
        {
            case State.searching:
                Searching.SetActive(true);
                Patrolling.SetActive(false);
                Chasing.SetActive(false);
                Attacking.SetActive(false);
                Retreating.SetActive(false);
                break;
            case State.patrolling:
                Searching.SetActive(false);
                Patrolling.SetActive(true);
                Chasing.SetActive(false);
                Attacking.SetActive(false);
                Retreating.SetActive(false);
                break;
            case State.attacking:
                Searching.SetActive(false);
                Patrolling.SetActive(false);
                Chasing.SetActive(false);
                Attacking.SetActive(true);
                Retreating.SetActive(false);
                break;
            case State.chasing:
                Searching.SetActive(false);
                Patrolling.SetActive(false);
                Chasing.SetActive(true);
                Attacking.SetActive(false);
                Retreating.SetActive(false);
                break;
            case State.retreating:
                Searching.SetActive(false);
                Patrolling.SetActive(false);
                Chasing.SetActive(false);
                Attacking.SetActive(false);
                Retreating.SetActive(true);
                break;
        }

        if (sight.CanSee == true) { 
                    
                        PlayerLastPos = Player.gameObject.transform.position;
                        ChangeState(State.chasing);
                        agent.SetDestination(PlayerLastPos);
        }

        if(state == State.patrolling)
        {
            if((this.gameObject.transform.position.x == PatrolPos[PatrolTarget].x)&& (this.gameObject.transform.position.z == PatrolPos[PatrolTarget].z))
            {
                PatrolTarget = PatrolTarget + 1;
                if(PatrolTarget == PatrolAmount)
                {
                    PatrolTarget = 0;
                }
            }
            agent.SetDestination(PatrolPos[PatrolTarget]);
        } else if (state == State.retreating)
        {
            if ((this.gameObject.transform.position.x == PatrolPos[PatrolTarget].x) && (this.gameObject.transform.position.z == PatrolPos[PatrolTarget].z))
            {
                ChangeState(State.patrolling);
                PatrolTarget = PatrolTarget + 1;
                if (PatrolTarget == PatrolAmount)
                {
                    PatrolTarget = 0;
                }
            }
            agent.SetDestination(PatrolPos[PatrolTarget]);
        }


        if (TimeBeforeSwitching >= TimeTarget)
        {

            if (sight.CanSee == true)
            {
                if ((Player.transform.position.x < this.gameObject.transform.position.x + 2) && (Player.transform.position.x > this.gameObject.transform.position.x - 2) && (Player.transform.position.y < this.gameObject.transform.position.y + 2) && (Player.transform.position.y > this.gameObject.transform.position.y - 2) && (Player.transform.position.z < this.gameObject.transform.position.z + 2) && (Player.transform.position.z > this.gameObject.transform.position.z - 2))
                {
                    ChangeState(State.attacking);
                } else
                {
                    ChangeState(State.chasing);
                }

                       
            }
            else
            {
                if (state == State.chasing) ChangeState(State.searching);
            }
            switch (state)
            {
                case State.searching:
                    if((this.gameObject.transform.position.x == PlayerLastPos.x) && (this.gameObject.transform.position.z == PlayerLastPos.z))
                    {
                        if(Waiting >= WaitTarget)
                        {
                            ChangeState(State.retreating);
                        } else
                        {
                            Waiting = Waiting + Time.deltaTime;
                        }
                    } else
                    {
                    }
                    break;
                case State.patrolling:
                    
                    break;
                case State.attacking:
                    agent.SetDestination(this.gameObject.transform.position);
                    break;
                case State.chasing:
                    agent.SetDestination(PlayerLastPos);
                    break;
                case State.retreating:
                    
                    break;
            }
            
        } else
        {
            TimeBeforeSwitching = TimeBeforeSwitching + Time.deltaTime;
        }
    }

    private void ChangeState(State stateTarget)
    {
        state = stateTarget;
    }
}
