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
        //�÷��̾� ���� ���� �׸���
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRealizeRange);
        //���� ���� �׸���
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Start()
    {
        //EnemyMeleeFSM().Start()����
        base.Start();
        //enemy �� ��Ÿ� ����, ��Ÿ�� ����
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
            //activeInHierachy�� �ڽĺ��� �θ��� setActive()�� ������ ����
            // meleeAttackArea�� dog�� �ڽ��̹Ƿ� dog�� SetActive()�� ������ �ްԵ�
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
            //dog�� canvas �� �� ����� ���ؼ� �θ� ����
            Targetting.Instance.monsterList.Remove(transform.gameObject);
            //Ÿ�� �ε��� �ʱ�ȭ
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
            //�浹�� �߻��� ��ġ�� ����Ʈ �ν��Ͻ� ����
            Instantiate(EffectSet.Instance.dogDamageEffect, collision.contacts[0].point, Quaternion.Euler(90, 0, 0));
        }
    }
}
