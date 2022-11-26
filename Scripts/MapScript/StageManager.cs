using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageManager : MonoBehaviour
{
    //없다면 찾아보고 그래도 없다면 만든다

    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                //StageManager Type확인
                instance = Transform.FindObjectOfType<StageManager>();
                //StageManager x  ->GameObject 생성 -> AddComponent<StageManager>();
                if (instance == null)
                {
                    var instanceContainer = new GameObject("StageManager");
                    instance = instanceContainer.AddComponent<StageManager>();
                }
            }

            return instance;
        }


    }
    private static StageManager instance;
    public GameObject player;

    [System.Serializable]
    public class StartPositionArray
    { 
          public List<Transform> DungeonStartPositions = new List<Transform>();    
    }
    public StartPositionArray[] StartPositionArrays;

    public List<Transform> AngelStartPos = new List<Transform>();
    public List<Transform> BossStartPos = new List<Transform>();
    public Transform LastBossStartPos;

    public int currentStage  = 0;
    int lastStage = 20;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void NextStage()
    {
        currentStage++;
        if (currentStage > lastStage)
        {
            return;
        }

        //1~4
        if (currentStage % 5 !=0)
        {
            //stage 구분을 위한 변수
            int arrayIndex  = currentStage / 10;
            int randomIndex = Random.Range(0, StartPositionArrays[arrayIndex].DungeonStartPositions.Count);
            player.transform.position = StartPositionArrays[arrayIndex].DungeonStartPositions[randomIndex].position;
            StartPositionArrays[arrayIndex].DungeonStartPositions.RemoveAt(randomIndex);
        }
        else//Angel || Boss
        {

            //Angel
            if (currentStage %10 == 5 )
            {
                int randomIndex = Random.Range(0, AngelStartPos.Count);
                player.transform.position = AngelStartPos[randomIndex].position;
                AngelStartPos.RemoveAt(randomIndex);
            }
            else
            {
                if (currentStage == lastStage)
                {
                    player.transform.position = LastBossStartPos.position;
                }
                else
                {
                    int randomIndex = Random.Range(0, BossStartPos.Count);
                    player.transform.position = BossStartPos[randomIndex].position;
                    BossStartPos.RemoveAt(randomIndex);
                }
            }
        }
    
    }
}
