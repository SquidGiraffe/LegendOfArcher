using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy�� FSM�� ���õ� class
public class EnemyMeleeFSM : EnemyBase
{
    public enum State
    { 
        Idle,
        Move,
        Attack 
    };
    //�⺻ ainmstate
    public State currentState = State.Idle;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    protected void Start()
    {
        //EnemyBase start()ȣ��
        base.Start();
        StartCoroutine(FSM());
        parentRoom = transform.parent.transform.parent.gameObject;

    }

    protected virtual void InitEnemy() { }

    protected virtual IEnumerator FSM()
    {
        yield return null;

        //player�� Ʈ���� ���� ���� �� �����̸� �ش� (while�� Ż�� -> �÷��̾� ����)
        while (!parentRoom.GetComponentInChildren<Dungeon>().playerEnter)
        {
            yield return Delay500;
        }

        InitEnemy();
        //�÷��̾ �������� current State�� ���� �ڷ�ƾ�� �����ϵ��� ��
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;
        //�ִϸ��̼��� �ݺ� ������ �����ϱ� ���� if ��
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.SetTrigger("Idle");
        }
        //���� ��Ÿ� ���� �÷��̾� o
        if (CanAttackStateFunc())
        {
           //���� ��Ÿ���� �� á����
            if (canAtk)
            {
                currentState = State.Attack;
            }
            //���� ��Ÿ���� �� �� á����
            else
            {
                currentState = State.Idle;
                transform.LookAt(player.transform.position);
            }
        }
        //���� ��Ÿ��� �÷��̾� x
        else
        {
            currentState = State.Move;
        }
    
    }

    protected virtual void AttackEffect() { }

    protected virtual IEnumerator Attack()
    {
        yield return null;

        //enemy�� ��� ���� player�� ������ �ð��� �ش� �׸��� �÷��̾ �ִ� �ڸ��� �������� �����Ѵ�
        navAgent.stoppingDistance = 0f;
        navAgent.isStopped = true;
        navAgent.SetDestination(player.transform.position);
        yield return Delay500;      //0.5 �� ��

        navAgent.isStopped = false;
        navAgent.speed = 30f;
        canAtk = false;

        // �ִϸ��̼� �ݺ� ������ ���� ���� ���ǹ�
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            anim.SetTrigger("Attack");
        }
        AttackEffect();
        yield return Delay500;
        //���� �ִϸ��̼� ���� �� ���� ������ ���� ��Ų��
        navAgent.speed = moveSpeed;
        navAgent.stoppingDistance = attackRange;
        currentState = State.Idle;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
        // �ִϸ��̼� �ݺ� ������ ���� ���� ���ǹ�
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            anim.SetTrigger("Walk");
        }
        //���� ��ⷯ �� player o && ���� ��Ÿ�� o
        if (CanAttackStateFunc() && canAtk)
        {
            currentState = State.Attack;
        }
        // player ���� �������� player���� �Ÿ��� �� �� ��� ���� 
        else if (distance > playerRealizeRange)
        {
            //canvas�� ���� �����̷��� parent�� ����������
            navAgent.SetDestination(transform.parent.position - Vector3.forward * 5f);
        }
        else
        {
            navAgent.SetDestination(player.transform.position);
        }
    
    }


    
}
