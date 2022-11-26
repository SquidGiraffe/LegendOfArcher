using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//enemy�� �⺻ ������ ����ִ� Ŭ����
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
    //�Ÿ��� �÷��̾� ���� ���� �Ǵ�
    protected bool CanAttackStateFunc()
    {
        Vector3 targetDir = new Vector3(player.transform.position.x - transform.position.x, 0f, player.transform.position.z - transform.position.z);
        Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z),
                                    targetDir,
                                    out RaycastHit hit,
                                    30f,
                                    layerMask);
        // player�� enemy������ �Ÿ�
        distance = Vector3.Distance(player.transform.position, transform.position);

        if (hit.transform == null)
        {
            return false;
        }
        //raycasthit�� player�̰� player�� enemy������ �Ÿ��� ���ݻ�Ÿ� ���� �۴ٸ� true
        if (hit.transform.CompareTag("Player") && distance <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //  ���� ��Ÿ�� ��� �Լ�
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
