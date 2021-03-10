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
    public int currentPlaneNum; // 当前玩家所在的格子号码
    public int nextPlaneNum; // 玩家所在的下一个格子号码
    public int finalPlaneNum;  // 玩家掷色子后的目标格子号码
    public int currentRound; // 当前回合数（掷色子前）
    public int roundCount;  // 总回合数
    Vector3 nextPosition; // 摇色子后下一个落点坐标
    // Start is called before the first frame update
    void Start()
    {
        Dice = GameObject.Find("Dice").gameObject;
        Planes = GameObject.Find("Planes").gameObject;
        Roll = GameObject.Find("Roll").gameObject;
        currentPlaneNum = 0;
        nextPlaneNum = 0;
        finalPlaneNum = 0;
        currentRound = 0;
        playerIsMoving = false;
        nextPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
       diceIsRotating = Roll.GetComponent<Roll>().diceIsRotating; // 获取色子运动状态
       DiceFaceUpNum = Dice.GetComponent<Dice>().DiceFaceUpNum;  // 获取色子点数
       timer = Roll.GetComponent<Roll>().timer; // 获取计时器
       roundCount = Roll.GetComponent<Roll>().roundCount; // 获取总回合数
       if (!diceIsRotating && timer != 4.0f) // 如果色子停止旋转且不处于首回合开始前(首回合开始前色子会有个下落过程)
        {
           
           if (currentRound != roundCount) 
           {
                finalPlaneNum = currentPlaneNum + DiceFaceUpNum; // 计算目标格子号码
                if (finalPlaneNum > 11) finalPlaneNum = finalPlaneNum - 12; // 经过一圈则需减去格子数
                if (Vector3.Distance(transform.localPosition, Planes.transform.GetChild(nextPlaneNum).position) > 0.1f) //是否移动到下一格
                {
                    nextPosition = Planes.transform.GetChild(nextPlaneNum).position;  // 取得落点坐标
                    playerIsMoving = true;
                }
                else if(Vector3.Distance(transform.localPosition, nextPosition) < 0.01f) // 移动到下一格了吗
                {
                    if (Vector3.Distance(transform.localPosition, Planes.transform.GetChild(finalPlaneNum).position) < 0.1f)//这一格是目标点吗？
                    {
                        currentPlaneNum = finalPlaneNum; 
                        playerIsMoving = false;
                        currentRound++;
                    }
                    nextPlaneNum++;
                    if (nextPlaneNum == 12) nextPlaneNum = 0;
                }
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, nextPosition, Time.deltaTime * 10.0f); // 每秒10米进行移动
                
            }
        }
        

    }
}
