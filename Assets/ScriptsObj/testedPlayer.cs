using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class testedPlayer : MonoBehaviour
{
    GameObject Dice;
    GameObject Planes;
    GameObject Roll;
    public int DiceFaceUpNum;
    public bool diceIsRotating;
    public bool playerIsMoving; 
    public float timer;  // 计时器,规定旋转时间
    public int planeNum; // 地图格子标记号码
    public int currentRound; // 当前回合数（掷色子前）
    public int roundCount;
    Vector3 newPosition; // 摇色子后下一个落点坐标
    // Start is called before the first frame update
    void Start()
    {
        Dice = GameObject.Find("Dice").gameObject;
        Planes = GameObject.Find("Planes").gameObject;
        Roll = GameObject.Find("Roll").gameObject;
        planeNum = 0;
        currentRound = 0;
        playerIsMoving = false;
        newPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
       diceIsRotating = Roll.GetComponent<Roll>().diceIsRotating;
       DiceFaceUpNum = Dice.GetComponent<Dice>().DiceFaceUpNum;  // 获取色子点数
       timer = Roll.GetComponent<Roll>().timer;
       if (!diceIsRotating && timer != 4.0f) // 如果色子停止旋转且不处于首回合开始前
       {
            roundCount = Roll.GetComponent<Roll>().roundCount;
           if(currentRound!= roundCount)
           {
                planeNum = planeNum + DiceFaceUpNum;
                if(planeNum > 11) planeNum = planeNum - 12;
                newPosition = Planes.transform.GetChild(planeNum).position; // 取得落点坐标
                currentRound++;
           }
           transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, Time.deltaTime*10); // 每秒10米进行移动
            //根据初始位置和目标的位置检查玩家是否在移动中
            if (Vector3.Distance(transform.localPosition, newPosition) < 0.1f)
            {
                playerIsMoving = false;
            }
            else
            {
                playerIsMoving = true;
            }
       }
    }
}
