using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
 
public class Dice : MonoBehaviour
{
    public int DiceFaceUpNum;
    public bool isRotating;
    GameObject Roll;
    void Start()
    {
        Roll = GameObject.Find("Roll").gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        isRotating = Roll.GetComponent<Roll>().isRotating; 
        if(!isRotating) GetNum(); // 如果色子不动,则获取其点数
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
        DiceFaceUpNum = int.Parse(upFace.name);//将朝上面 的名字 转化为int
        //Debug.Log("点数是： " + DiceFaceUpNum);
    }
}
