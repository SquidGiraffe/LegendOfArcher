using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDog : EnemyMeleeFSM
{
    public GameObject enemyCanvasGo;
    public GameObject meleeAttackArea;
    bool isDeleted;
    private void OnDrawGizmosSelected()
    {
        //플레이어 감지 범위 그리기
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRealizeRange);
        //공격 범위 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Start()
    {
        //EnemyMeleeFSM().Start()실행
        base.Start();
        //enemy 별 사거리 공격, 쿨타임 설정
        attackCoolTime = 2f;
        attackCoolTimeCacl = attackCoolTime;

        attackRange = 3f;
        navAgent.stoppingDistance = 1f;

        StartCoroutine(ResetAttackArea());
    }

    IEnumerator ResetAttackArea()
    {
        while (true)
        {
            yield return null;
            //activeInHierachy는 자식보다 부모의 setActive()에 영향을 받음
            // meleeAttackArea는 dog의 자식이므로 dog의 SetActive()에 영향을 받게됨
            if (!meleeAttackArea.activeInHierarchy && currentState == State.Attack)
            {
                yield return new WaitForSeconds(attackCoolTime);
                meleeAttackArea.SetActive(true);
            }
        }
    
    
    }

    protected override void InitEnemy()
    {
        isDeleted = false;
        maxHp += (StageManager.Instance.currentStage + 1) * 100f;
        currentHP = maxHp;
        damage += (StageManager.Instance.currentStage + 1) * 10f;
    }

    protected override void AttackEffect()
    {
        Instantiate(EffectSet.Instance.dogAttackEffect, transform.position, Quaternion.Euler(90, 0, 0));

    }
    void Update()
    {
        if (currentHP <= 0 && !isDeleted)
        {
            navAgent.isStopped = true;

            rigid.gameObject.SetActive(false);
            //dog와 canvas 둘 다 지우기 위해서 부모에 접근
            Targetting.Instance.monsterList.Remove(transform.gameObject);
            //타겟 인덱스 초기화
           Targetting.Instance.targetIndex = -1;
            Destroy(transform.parent.gameObject);
            isDeleted = true;
            return;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("PlayerProjectile"))
        {
            PlayerProjectile playerProjectile = collision.gameObject.GetComponent<PlayerProjectile>();
            enemyCanvasGo.GetComponent<EnemyHPBar>().Playerdamage = playerProjectile.damage;
            enemyCanvasGo.GetComponent<EnemyHPBar>().Dmg();
            currentHP -= playerProjectile.damage;
            //충돌일 발생한 위치에 이펙트 인스턴스 생성
            Instantiate(EffectSet.Instance.dogDamageEffect, collision.contacts[0].point, Quaternion.Euler(90, 0, 0));
        }
    }
}
