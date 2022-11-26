using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRich : EnemyMeleeFSM
{
    GameObject Player;
    Dungeon Map;

    public GameObject DangerMarker;
    public GameObject EnemyBolt;

    public Transform BoltGenPosition;

    RichBolt richBolt;

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
        playerRealizeRange = 10f;
        attackRange = 5f;
        Player = GameObject.FindGameObjectWithTag("Player");
        //Rich->EnemyRich(Rich,Canvas)->Map(EnemyRich)
        Map = transform.parent.transform.parent.gameObject.GetComponentInChildren<Dungeon>();
        StartCoroutine(WaitPlayer());
    }
    IEnumerator WaitPlayer()
    {
        yield return null;

        while (!Map.playerEnter)
        {
            yield return new WaitForSeconds(0.5f);
                 
        }
        yield return new WaitForSeconds(2f);
        transform.LookAt(Player.transform.position);
        DangerMarkerShoot();

        yield return new WaitForSeconds(1f);
        Shoot();
    
    }

    //발사체 궤적 표시줄
    void DangerMarkerShoot()
    {
        Vector3 NewPosition = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        Physics.Raycast(NewPosition,
                        transform.forward,
                        out RaycastHit hit,
                        30f,
                        layerMask);

        if (hit.transform.CompareTag("Obstacle"))
               {
                   GameObject DangerMarkerClone = Instantiate(DangerMarker, NewPosition, transform.rotation);
                   DangerMarkerClone.GetComponent<DangerLine>().EndPosition = hit.point;

               }

 
        
    }
    
    //발사체 발사
    void Shoot()
    {
        Instantiate(EnemyBolt, BoltGenPosition.position, transform.rotation);
        richBolt = EnemyBolt.GetComponent<RichBolt>();
    }
}
