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
                    Gizmos.DrawRay(tempVec, monsterList[i].transform.position - transform.position);//���� 

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
            //����Ʈ�� obj�ִ��� Ȯ��
            if (monsterList.Count > 0)
            {
                curDist = 0f;       //target�� ���� ���� �Ÿ�
                closeDistIndex = 0; //raycast���ο� ������� ���� ���� ����� obj�� index
                targetIndex = -1;   //target�� index

                //for������ ������ obj�� player������ �Ÿ��� ���ϰ� raycast����
                for (int i = 0; i < monsterList.Count; i++)
                {
                    if (monsterList[i] == null) return;
                    //Distance�� float�� ��ȯ
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
                        //for���� ���� raycast������ obj���� ���� ����� obj�� index�� �Ÿ��� ���Ѵ�
                        if (targetDist >= curDist)
                        {
                            targetIndex = i;
                            targetDist = curDist;
                            TargetDistForAtk = curDist;
                        }

                    }

                    //raycast�� ������� ���� ����� obj�� ����
                    if (closetDist >= curDist)
                    {
                        closeDistIndex = i;
                        closetDist = curDist;
                    }
                }

                //for�� ������ raycast������ ������ ��� ���� ����� obj�� ��������
                if (targetIndex == -1)
                {
                    targetIndex = closeDistIndex;
                    targetDist = closetDist;
                    TargetDistForAtk = closetDist;
                }

                // Ÿ���� Ž���ϴ� ������ �Ǵ� ������ �ʱ�ȭ��
                closetDist = 100f;
                targetDist = 100f;
                getATarget = true;
            }

        }
       
    
    
    }
    void AttackTarget()
    {
       
        //Ÿ���� ��ã�Ұų� ���� ����Ʈ�� ����ִ� ���
        if (targetIndex == -1 || monsterList.Count == 0)
        {
            anim.SetBool("isAttack", false);
        }
        //���� Ÿ���� ���� && �̵�x
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
        //�̵�o
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
        //  �̵� x && Ÿ�� x || ����Ʈ x
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
