using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    GameObject Dice;
    GameObject Planes;
    GameObject Roll;
    public int DiceFaceUpNum;
    public int count;
    public bool isRotating;
    public bool isMoving;
    Vector3 newPosition;
    // Start is called before the first frame update
    void Start()
    {
        Dice = GameObject.Find("dice/Dice").gameObject;
        Planes = GameObject.Find("Planes").gameObject;
        Roll = GameObject.Find("Roll").gameObject;
        count = 0;
        newPosition = new Vector3(0, 0, 0);
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
      
            isRotating = Roll.GetComponent<Roll>().isRotating;
            if (!isRotating)
            {
                DiceFaceUpNum = Dice.GetComponent<Dice>().DiceFaceUpNum;  // 获取色子点数
                count += DiceFaceUpNum;
                newPosition = Planes.transform.GetChild(DiceFaceUpNum).position; // 计算落点坐标
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, 0.1f); // 进行移动
                isMoving = true;
            }
        if(Time.time - checktime > 3)
        {
            checktime = Time.time;
            if ((transform.position - lastpos).sqrMagnitude > 0.5f)
            {
                print("在移动");
            }
            else
            {
                print("停止dao");
            }
            lastpos = transform.position;
        }

    }
}
