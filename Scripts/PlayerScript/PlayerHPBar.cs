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
         HorizontalLayoutGroup 를 사용하는 이유는 HPBar의 이미지들을 수평으로 동일간격을 유지시키며 추가하고자 하였기 때문이다
        이 컴포넌트로 인해 이미지들의 scale만큼 간격을 유지할 수 있게됨

         비활성화를 하지 않을시 HPLine 내부에 있는 UnitHPBar들의 개수는 동일하고 그 HPBar들의 간격이 멀어지고 HPBar들의 scale이 작아질뿐
         우리가 원하는 바와같이 HPBar들의 수가 늘어나며 간격이 처음과 같은 HPBar구현을 하지못한다
         이는 HorzontalLayoutGroup에서 scale에 따라 간격을 설정하기 때문이니 이를 막기위해 foreach문 전에 비활성화를 시켜준다
         */
        HPLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
            foreach (Transform child in HPLineFolder.transform)
            {
                child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
            }
        HPLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
  
    }
}
