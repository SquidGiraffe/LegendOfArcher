using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll : MonoBehaviour
{
    public GameObject enemyCanvasGo;

    private void OnCollisionEnter(Collision collision)
    {
        //�浹�� �߻����� �� Damage()�� �����Ű�� ���� enemy GameObject�� �ش� script����
        if (collision.transform.CompareTag("PlayerProjectile")) 
        {
            enemyCanvasGo.GetComponent<EnemyHPBar>().Dmg();
        }
    }
}
