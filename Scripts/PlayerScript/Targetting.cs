using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetting : MonoBehaviour
{
    public  static Targetting Instance
   {
       get
       
       {
           if (instance ==null)
           {
               instance = FindObjectOfType<Targetting>();
               if (instance== null)
               {
                    var instanceCotainer = new GameObject("Targetting");
                    instance = instanceCotainer.AddComponent<Targetting>();
               }
           }


           return instance;
       }  
   }
    private static Targetting instance;

    public bool getATarget = false;
    public bool playerEnter;
    public float attackSpeed = 1f;

    private float TargetDistForAtk;
    float curDist = 0;
    float closetDist = 100f;
    float targetDist = 100f;
    
    int closeDistIndex = 0;
    public int targetIndex = -1; 


    public LayerMask layerMask;
    Vector3 targetPos;


    Animator anim;
    public List<GameObject> monsterList;
    public PlayerProjectile playerProjectile;
    public Player player;
    public GameObject playerProj;
    public Transform attackPoint;
    private void Awake()
    {
        transform.position           = player.transform.position;
        anim                         = player.GetComponent<Animator>();
    }
    private void Update()
    {
        PlayerEnter();
        SetTarget();
        AttackTarget();
    }
    void OnDrawGizmos()
     {
        if (getATarget)
        {

            if (monsterList.Count>0)
            {
                for (int i = 0; i < monsterList.Count; i++)
                {
                    if (monsterList[i] == null) { return; }

                    RaycastHit Hit;
                    Vector3 tempVec = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);

                    bool isHit = Physics.Raycast(tempVec,
                                                 monsterList[i].transform.position - transform.position,
                                                 out Hit,
                                                 30f,
                                                 layerMask);

                    if (isHit && Hit.transform.CompareTag("Monster"))
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                    }
                    Gizmos.DrawRay(tempVec, monsterList[i].transform.position - transform.position);//변경 

                }

            }
        }
    }

    void PlayerEnter()
    {
        if (playerEnter == true)
        {
            monsterList = new List<GameObject>(monsterList);
            getATarget = true;
        }
    }
    void SetTarget()
    {

        if (playerEnter)
        {
            //리스트에 obj있는지 확인
            if (monsterList.Count > 0)
            {
                curDist = 0f;       //target과 나의 현재 거리
                closeDistIndex = 0; //raycast여부와 상관없이 나와 가장 가까운 obj의 index
                targetIndex = -1;   //target의 index

                //for문으로 각각의 obj의 player사이의 거리를 구하고 raycast시행
                for (int i = 0; i < monsterList.Count; i++)
                {
                    if (monsterList[i] == null) return;
                    //Distance는 float값 반환
                    curDist = Vector3.Distance(transform.position, monsterList[i].transform.position);

                    RaycastHit hit;
                    bool isHit = Physics.Raycast(transform.position,
                                    monsterList[i].transform.position - transform.position,
                                    out hit,
                                    30f,
                                    layerMask);
                    //raycast o && tag: Monster
                    if (isHit && hit.transform.CompareTag("Monster"))
                    {
                        //for문을 돌며 raycast성공한 obj들중 가장 가까운 obj의 index와 거리를 구한다
                        if (targetDist >= curDist)
                        {
                            targetIndex = i;
                            targetDist = curDist;
                            TargetDistForAtk = curDist;
                        }

                    }

                    //raycast와 관계없이 가장 가까운 obj를 구함
                    if (closetDist >= curDist)
                    {
                        closeDistIndex = i;
                        closetDist = curDist;
                    }
                }

                //for문 내에서 raycast성공이 없었을 경우 가장 가까운 obj의 값들을줌
                if (targetIndex == -1)
                {
                    targetIndex = closeDistIndex;
                    targetDist = closetDist;
                    TargetDistForAtk = closetDist;
                }

                // 타겟을 탐색하는 기준이 되는 값들을 초기화함
                closetDist = 100f;
                targetDist = 100f;
                getATarget = true;
            }

        }
       
    
    
    }
    void AttackTarget()
    {
       
        //타겟을 못찾았거나 몬스터 리스트가 비어있는 경우
        if (targetIndex == -1 || monsterList.Count == 0)
        {
            anim.SetBool("isAttack", false);
        }
        //몬스터 타겟팅 성공 && 이동x
        if (!player.isInput && getATarget && monsterList.Count > 0 && (TargetDistForAtk < 10.0f))
        {
           player.transform.LookAt(monsterList[targetIndex].transform);
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.SetBool("isIdle"  , false);
                anim.SetBool("isWalk"  , false);
                anim.SetBool("isAttack", true );
                //Debug.Log(anim.GetCurrentAnimatorStateInfo(0));
            }           
        }
        //이동o
        else if (player.isInput)
        {
       //     Debug.Log("Targetting :player.isInput :" + player.isInput);
       //     Debug.Log("(anim.GetCurrentAnimatorStateInfo(0).IsName(Walk):" + anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"));
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                anim.SetBool("isIdle"  , false);
                anim.SetBool("isWalk"  , true);
                anim.SetBool("isAttack", false);

            }
        }
        //  이동 x && 타겟 x || 리스트 x
        else
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                anim.SetBool("isIdle", true);
                anim.SetBool("isWalk", false);
                anim.SetBool("isAttack", false);
            }
     
        }
    
    
    }
    
    void Attack()
    {
        if (!player.isInput && getATarget && monsterList.Count > 0)
        {
            anim.SetFloat("attackSpeed", attackSpeed);
            targetPos = monsterList[targetIndex].transform.position;
            Instantiate(playerProj, attackPoint.position, player.transform.rotation);
            playerProjectile = playerProj.GetComponent<PlayerProjectile>();
            playerProjectile.targetPos = targetPos;
        }
    }

}
