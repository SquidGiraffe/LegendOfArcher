using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//enemy의 기본 정보를 담고있는 클래스
public class EnemyBase : MonoBehaviour
{
    public float maxHp = 100f;
    public float currentHP = 1000f;

    public float damage = 100f;

    protected float playerRealizeRange = 5f;  
    protected float attackRange = 5f;
    protected float attackCoolTime = 5f;
    protected float attackCoolTimeCacl = 5f;
    protected bool canAtk = true;

    protected float moveSpeed = 2f;

    protected GameObject player;
    protected NavMeshAgent navAgent;
    protected float distance;

    protected GameObject parentRoom;

    protected Animator anim;
    protected Rigidbody rigid;

    public LayerMask layerMask;

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        navAgent = GetComponent<NavMeshAgent>();
        rigid    = GetComponent<Rigidbody>();
        anim     = GetComponent<Animator>();

        //enemy->enemies(enemy,canvas)->Map
        parentRoom = transform.parent.transform.parent.gameObject;
        StartCoroutine(CalcCoolTime());

    }
    //거리로 플레이어 공격 가능 판단
    protected bool CanAttackStateFunc()
    {
        Vector3 targetDir = new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z);
        Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z),
                                    targetDir,
                                    out RaycastHit hit,
                                    30f,
                                    layerMask);
        // player와 enemy사이의 거리
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (hit.transform == null)
        {
            return false;
        }
        //raycasthit이 player이고 player와 enemy사이의 거리가 공격사거리 보다 작다면 true
        if (hit.transform.CompareTag("Player") && distance <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //  공격 쿨타임 계산 함수
    protected virtual IEnumerator CalcCoolTime()
    {
        while (true)
        {
            yield return null;
            if (!canAtk)
            {
                attackCoolTimeCacl -= Time.deltaTime;
                if (attackCoolTimeCacl <=0)
                {
                    attackCoolTimeCacl = attackCoolTime;
                    canAtk = true;
                }
            }
        }
    
    
    }
}
