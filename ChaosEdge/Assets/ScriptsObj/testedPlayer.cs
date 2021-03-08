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
    public float checktime;
    public bool diceIsRotating;
    public bool playerIsMoving;
    public float timer;
    public int planeNum;
    public int currentRound;
    public int roundCount;
    Vector3 lastPosition;
    Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        Dice = GameObject.Find("Dice").gameObject;
        Planes = GameObject.Find("Planes").gameObject;
        Roll = GameObject.Find("Roll").gameObject;
        checktime = 0;
        diceIsRotating = false;
        planeNum = 0;
        currentRound = 0;
        playerIsMoving = false;
        timer = 4.0f;
        newPosition = new Vector3(0, 0, 0);
        lastPosition = transform.position;
    }
    void checkMoving() // // 检测玩家是否移动
    {
        if (Time.time - checktime > 0.1)
        {
            checktime = Time.time;
            //判断是否有移动
            if ((transform.position - lastPosition).sqrMagnitude > 0.05f)
            {
                playerIsMoving = true;
                //print("is moving");
            }
            else
            {
                playerIsMoving = false;
                //print("stop");
            }
            lastPosition = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
       checkMoving(); // 检测玩家是否还在移动
       diceIsRotating = Dice.GetComponent<Dice>().diceIsRotating; // 获取色子运动状态
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
       }
       

    }
}
