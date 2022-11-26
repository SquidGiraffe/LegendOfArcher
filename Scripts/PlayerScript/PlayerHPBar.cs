using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHPBar : MonoBehaviour
{
    public static PlayerHPBar Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerHPBar>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerHPBar");
                    instance = instanceContainer.AddComponent<PlayerHPBar>();
                }
            }

            return instance;
        
        } 
    
    }

    private static PlayerHPBar instance;


    public Transform player;
    public Slider hpbar;
    public float maxHp;
    public float currentHP;

    public GameObject HPLineFolder;
    float unitHP = 200f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        hpbar.value = currentHP / maxHp;
    }

    public void GetHpBoost()
    {
        maxHp += 150;
        float scaleX = (1000f / unitHP) / (maxHp / unitHP);
        /*
         HorizontalLayoutGroup �� ����ϴ� ������ HPBar�� �̹������� �������� ���ϰ����� ������Ű�� �߰��ϰ��� �Ͽ��� �����̴�
        �� ������Ʈ�� ���� �̹������� scale��ŭ ������ ������ �� �ְԵ�

         ��Ȱ��ȭ�� ���� ������ HPLine ���ο� �ִ� UnitHPBar���� ������ �����ϰ� �� HPBar���� ������ �־����� HPBar���� scale�� �۾�����
         �츮�� ���ϴ� �ٿͰ��� HPBar���� ���� �þ�� ������ ó���� ���� HPBar������ �������Ѵ�
         �̴� HorzontalLayoutGroup���� scale�� ���� ������ �����ϱ� �����̴� �̸� �������� foreach�� ���� ��Ȱ��ȭ�� �����ش�
         */
        HPLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
            foreach (Transform child in HPLineFolder.transform)
            {
                child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
            }
        HPLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
  
    }
}
