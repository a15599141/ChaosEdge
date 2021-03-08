using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
 
public class Dice : MonoBehaviour
{
    Rigidbody rigDice;
    public int DiceFaceUpNum;
    public bool diceIsRotating; // 记录色子是否在旋转
    private Vector3 a;
    void Start()
    {
         a = new Vector3(0.0f, 0.0f, 0.0f);
        rigDice = this.transform.GetComponent<Rigidbody>();
        diceIsRotating = false;
    }

    void checkDiceRotation()
    {
        print(rigDice.velocity);
        if (rigDice.velocity == a) diceIsRotating = false;
        else diceIsRotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        checkDiceRotation();
        if (!diceIsRotating) GetNum(); // 如果色子不动,则获取其点数
    }

    void GetNum()
    {
        Transform[] obj = new Transform[6];//声明数组存放色子的六个面
        Transform upFace= transform.GetChild(0).GetChild(0);//声明朝上的面 
        for (int i = 0; i < 6; i++)//循环判断哪个面朝上
        {
            obj[i] = transform.GetChild(0).GetChild(i);
            if (obj[i].position.y > upFace.position.y)
            {
                upFace = obj[i];
            }
        }
        DiceFaceUpNum = int.Parse(upFace.name);//将朝上面的名字 转化为int
        //Debug.Log("点数是： " + DiceFaceUpNum);
    }
}
