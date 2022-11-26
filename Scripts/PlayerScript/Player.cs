using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float hAxis;
    float vAxis;
    public int speed;
    Vector3 moveVec;
    Vector2 inputDirection;

    //Components
    Animator anim;
    Rigidbody rigid;

   
    public bool isInput;
    List<MonsterInfo> monstersInfo;

    //bool 변수관리

    struct MonsterInfo
    {
      public Vector3 monsPos;
      public Vector3 offset;
      public GameObject monsterObj;
    }

     void Awake()
    {
        anim         = GetComponent<Animator>();
        rigid        = GetComponent<Rigidbody>();
    }

    void Update()
    {
        InputManager();
        Turn();
    }

    void InputManager()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");


    }

     public void Move(Vector2 inputDirection,bool isBorder ,bool isInput)
    {
         moveVec = new Vector3(inputDirection.x,0, inputDirection.y).normalized;

        //충돌 x  && 입력 O
        if (!isBorder && isInput)
        {
            transform.position += (moveVec * speed * Time.deltaTime);
            anim.SetBool("isWalk", true);
        }
        //충돌 o  || 입력 x
        else if (isBorder || !isInput)
        {
            anim.SetBool("isWalk", false);
        }

    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);   
    }

    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }



     void FixedUpdate()
    {
        FreezeRotation();

    }

    private void OnTriggerEnter(Collider other)
    {
         if (other.transform.CompareTag("NextRoom"))
       {
         //  Debug.Log("GetNexRoom");
           StageManager.Instance.NextStage();
       }
 //      if (other.transform.CompareTag("HpBooster"))
 //    {
 //        PlayerHPBar.Instance.GetHpBoost();
 //        Destroy(other.gameObject);
 //    }

        if (other.transform.CompareTag("MeleeAtk"))
        {
            other.transform.parent.GetComponent<EnemyDog>().meleeAttackArea.SetActive(false);
            PlayerHPBar.Instance.currentHP -= other.transform.parent.GetComponent<EnemyDog>().damage * 2f;

            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                anim.SetTrigger("Damage");
                Instantiate(EffectSet.Instance.playerDamageEffect, Targetting.Instance.attackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }
        else if (other.transform.CompareTag("EnemyProjectile"))
        {
            PlayerHPBar.Instance.currentHP -= other.transform.parent.GetComponent<EnemyRich>().damage * 2f;
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Damage"))
            {
                anim.SetTrigger("Damage");
                Instantiate(EffectSet.Instance.playerDamageEffect, Targetting.Instance.attackPoint.position, Quaternion.Euler(90, 0, 0));
            }
        }
    }

}
