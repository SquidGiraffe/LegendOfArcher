using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSet : MonoBehaviour
{
    public static EffectSet Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<EffectSet>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("EffectSet");
                    instance = instanceContainer.AddComponent<EffectSet>();
                }

            }

            return instance;
        }

    }
    private static EffectSet instance;

    [Header("Monster")]
    public GameObject dogAttackEffect;
    public GameObject dogDamageEffect;


    [Header("Player")]
    public GameObject playerAttackEffect;
    public GameObject playerDamageEffect;

     void Start()
    {
        StartCoroutine(delete());
    }
    IEnumerator delete()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(instance);
    }
}
