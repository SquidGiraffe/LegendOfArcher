using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{

    public Slider hpSlider;     //��ü
    public Slider backHpSlider; //hp���� ȿ���� ��Ÿ���� ���� �����̴�
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
        //enemy�� �̵��� HpBar�� ���� �����̵��� ����
        transform.position = enemy.position;
        //���� ������ �̿��� hp�� ���̴� �ӵ� ����
        hpSlider.value = Mathf.Lerp(hpSlider.value,currentHp/maxHp,Time.deltaTime);

        if (backHpHit)
        {
            //������ �����̴��� value ���� �̸� ���߱⿡ ������ ���
            backHpSlider.value = Mathf.Lerp(backHpSlider.value, hpSlider.value, Time.deltaTime * 2f);
            //hp�� 0�̻��̶�� �ݺ��� �� �ֵ���
            if (hpSlider.value >= backHpSlider.value -0.01f)
            {
                backHpHit = false;
                //���� ������ �ϴ��� �ణ�� ���̴� �ֱ⿡ ���� ���� ���� �������� ������ܤ�
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
