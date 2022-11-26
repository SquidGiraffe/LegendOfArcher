using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{

    public Slider hpSlider;     //본체
    public Slider backHpSlider; //hp감소 효과를 나타내기 위한 슬라이더
    public Transform enemy;
    EnemyRich enemyRich;
    public float maxHp;
    public float currentHp;
    public bool backHpHit =false;
    public float Playerdamage;

    private void Start()
    {
        enemyRich   = transform.parent.GetComponentInChildren<EnemyRich>();
    }

    void Update()
    {
        maxHp = enemyRich.maxHp;
        currentHp = enemyRich.currentHP;
        //enemy의 이동에 HpBar가 같이 움직이도록 만듦
        transform.position = enemy.position;
        //선형 보간을 이용해 hp가 깎이는 속도 조절
        hpSlider.value = Mathf.Lerp(hpSlider.value,currentHp/maxHp,Time.deltaTime);

        if (backHpHit)
        {
            //위에서 슬라이더의 value 값을 미리 구했기에 대입해 사용
            backHpSlider.value = Mathf.Lerp(backHpSlider.value, hpSlider.value, Time.deltaTime * 2f);
            //hp가 0이상이라면 반복할 수 있도록
            if (hpSlider.value >= backHpSlider.value -0.01f)
            {
                backHpHit = false;
                //선형 보간을 하더라도 약간의 차이는 있기에 둘이 같은 값을 가지도록 만들어줌ㄴ
                backHpSlider.value = hpSlider.value;
            }
        }

    }
    public void Dmg()
    {
        currentHp -= Playerdamage;
        Invoke("BackHpFun", 0.1f);
    }
    void BackHpFun()
    {
        backHpHit = true;  
    }
}
