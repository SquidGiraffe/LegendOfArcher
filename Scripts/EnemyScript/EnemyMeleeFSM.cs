using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy의 FSM과 관련된 class
public class EnemyMeleeFSM : EnemyBase
{
    public enum State
    { 
        Idle,
        Move,
        Attack 
    };
    //기본 ainmstate
    public State currentState = State.Idle;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    protected void Start()
    {
        //EnemyBase start()호출
        base.Start();
        StartCoroutine(FSM());
        parentRoom = transform.parent.transform.parent.gameObject;

    }

    protected virtual void InitEnemy() { }

    protected virtual IEnumerator FSM()
    {
        yield return null;

        //player가 트리거 내에 없을 시 딜레이를 준다 (while문 탈출 -> 플레이어 입장)
        while (!parentRoom.GetComponentInChildren<Dungeon>().playerEnter)
        {
            yield return Delay500;
        }

        InitEnemy();
        //플레이어가 들어왔으니 current State에 따른 코루틴을 실행하도록 함
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;
        //애니메이션의 반복 실행을 제어하기 위한 if 문
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.SetTrigger("Idle");
        }
        //공격 사거리 내에 플레이어 o
        if (CanAttackStateFunc())
        {
           //공격 쿨타임이 다 찼을때
            if (canAtk)
            {
                currentState = State.Attack;
            }
            //공격 쿨타임이 다 안 찼을때
            else
            {
                currentState = State.Idle;
                transform.LookAt(player.transform.position);
            }
        }
        //공격 사거리내 플레이어 x
        else
        {
            currentState = State.Move;
        }
    
    }

    protected virtual void AttackEffect() { }

    protected virtual IEnumerator Attack()
    {
        yield return null;

        //enemy를 잠시 멈춰 player가 반응할 시간을 준다 그리고 플레이어가 있던 자리를 목적지로 설정한다
        navAgent.stoppingDistance = 0f;
        navAgent.isStopped = true;
        navAgent.SetDestination(player.transform.position);
        yield return Delay500;      //0.5 초 후

        navAgent.isStopped = false;
        navAgent.speed = 30f;
        canAtk = false;

        // 애니메이션 반복 실행을 막기 위한 조건문
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            anim.SetTrigger("Attack");
        }
        AttackEffect();
        yield return Delay500;
        //공격 애니메이션 실행 후 원래 값으로 복구 시킨다
        navAgent.speed = moveSpeed;
        navAgent.stoppingDistance = attackRange;
        currentState = State.Idle;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
        // 애니메이션 반복 실행을 막기 위한 조건문
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            anim.SetTrigger("Walk");
        }
        //공격 사기러 내 player o && 공격 쿨타임 o
        if (CanAttackStateFunc() && canAtk)
        {
            currentState = State.Attack;
        }
        // player 감지 범위보다 player와의 거리가 더 먼 경우 직진 
        else if (distance > playerRealizeRange)
        {
            //canvas도 같이 움직이려면 parent도 움직여야함
            navAgent.SetDestination(transform.parent.position - Vector3.forward * 5f);
        }
        else
        {
            navAgent.SetDestination(player.transform.position);
        }
    
    }


    
}
