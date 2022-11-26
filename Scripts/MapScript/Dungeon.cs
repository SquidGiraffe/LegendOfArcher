using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{

    public List<GameObject> monsterListRoom = new List<GameObject>();
    public bool isClear =false;
    public bool playerEnter = false;
    Targetting targetting;
    StageManager stageManager;
     void Start()
    {     
        targetting   = FindObjectOfType<Targetting>();
        stageManager = FindObjectOfType<StageManager>();
        
    }
    void Update()
    {
        if (targetting.playerEnter == true && monsterListRoom.Count <= 0 && !isClear)
        {
            isClear = true;
          //Debug.Log("Clear");
        }
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            targetting.monsterList = new List <GameObject>(monsterListRoom);
            playerEnter = true;
            targetting.playerEnter = playerEnter;
        }

        if (other.CompareTag("Monster"))
        {
            monsterListRoom.Add(other.transform.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && targetting.monsterList.Count <=0)
        {
            playerEnter = false;
            targetting.playerEnter = playerEnter;
            targetting.monsterList.Clear();
        }
        if (other.CompareTag("Monster"))
        {
            monsterListRoom.Remove(other.transform.gameObject);
        }
    }
}
