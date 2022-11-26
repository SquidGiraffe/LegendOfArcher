using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControll : MonoBehaviour
{
    public GameObject enemyCanvasGo;

    private void OnCollisionEnter(Collision collision)
    {
        //충돌이 발생했을 시 Damage()를 실행시키기 위해 enemy GameObject에 해당 script붙임
        if (collision.transform.CompareTag("PlayerProjectile")) 
        {
            enemyCanvasGo.GetComponent<EnemyHPBar>().Dmg();
        }
    }
}
