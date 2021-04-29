using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> playerObjects;//玩家对象集合
    public List<GameObject> spawnPoints;//出生点对象集合
    public Dice dice;
    public bool moveAllowed;//判断当前玩家是否可以移动
    int playerNumber; //玩家数量

    public TestedPlayer currPlayer;//当前玩家
    public int currPlayerIndex;

    int steps; //当前玩家需要移动的格子数
    int tempSteps;//缓存剩余格子数
    bool isMoving; //判断当前玩家是否移动中

    float moveSpeed = 10.0f;

    //启动单例
    private static PlayerManager _instance;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
            }

            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        //playerObjects = GameObject.FindGameObjectsWithTag("Player").ToList();//获取所有玩家
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList();//获取所有出生点
        playerNumber = playerObjects.Count;//玩家总数

        //初始化玩家
        for (int i = 0; i < playerNumber; i++)
        {
            currPlayer = playerObjects[i].GetComponent<TestedPlayer>();
            currPlayer.transform.position = spawnPoints[i].transform.position;
            currPlayer.routePosition = spawnPoints[i].transform.GetSiblingIndex();
            currPlayer.energy = 1000;
        }

        //选中第一个作为当前玩家
        currPlayerIndex = 0;
        currPlayer = playerObjects[currPlayerIndex].GetComponent<TestedPlayer>();


        //更新UI界面玩家信息
        CanvasManager.Instance.UpdatePlayerPanel();

    }

    // Update is called once per frame
    void Update()
    {
        if (moveAllowed)
        {
            moveAllowed = false;
            steps = dice.diceNumber;
            //Debug.Log("dice number: " + steps);
            StartCoroutine(PlayerMove());
        }
    }

    IEnumerator PlayerMove()
    {
        if (isMoving)
        {
            yield break;
        }
        isMoving = true;

        while (steps > 0)
        {
            currPlayer.routePosition = (currPlayer.routePosition + 1) % Route.Instacnce.childNodeList.Count;

            Vector3 nextPos = Route.Instacnce.childNodeList[currPlayer.routePosition].position;
            //判断是否到达下一格
            while (MoveToNextNode(nextPos))
            {
                yield return null;
            }
            //移动完毕停顿
            yield return new WaitForSeconds(0.1f);
            steps--;


            //处理玩家遭遇战斗
            if (currPlayer.engagement)
            {
                tempSteps = steps;//缓存剩余格子数
                steps = 0;//停止当前移动
                CanvasManager.Instance.IsConfirm(ConfirmType.Battle);//选择是否战斗
            }
            else if (currPlayer.onTradeStation)
            {
                tempSteps = steps;//缓存剩余格子数
                steps = 0;//停止当前移动
                CanvasManager.Instance.IsConfirm(ConfirmType.TradeStation);//选择是否进入商店
            }
        }
        

        //处理格子事件
        if (!currPlayer.engagement&&!currPlayer.onTradeStation)
        {
            CanvasManager.Instance.IsConfirm(ConfirmType.Construction);
        }
        isMoving = false;
    }

    bool MoveToNextNode(Vector3 goal)
    {
        return goal != (currPlayer.transform.position = Vector3.MoveTowards(currPlayer.transform.position, goal, moveSpeed * Time.deltaTime));
    }


    public void EndTheTurn()
    {
        //currPlayerIndex = (currPlayerIndex + 1) % playerNumber;
        currPlayerIndex = 0;
        currPlayer = playerObjects[currPlayerIndex].GetComponent<TestedPlayer>();
        CanvasManager.Instance.UpdatePlayerPanel();
        dice.rollButton.interactable = true;//释放按钮
        currPlayer.onTradeStation = false;
    }

    public void BattleCancel()
    {
        currPlayer.engagement = false;
        currPlayer.onTradeStation = false;
        steps = tempSteps;
        tempSteps = 0;
        StartCoroutine(PlayerMove());
    }
}
