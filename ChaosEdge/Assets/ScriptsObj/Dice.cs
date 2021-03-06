using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
 
public class Dice : MonoBehaviour
{
    GameObject RollButton;
    private int p_x, p_y, p_z;
    private float timer;
    public int DiceFaceUpNum;
    void Start()
    {
        RollButton = GameObject.Find("Roll").gameObject;
    }
    // Update is called once per frame
    private void OnMouseDown()
    {
        //取XYZ的随机旋转值 
        p_x = Random.Range(1, 30);
        p_y = Random.Range(1, 30);
        p_z = Random.Range(1, 30);
        timer = 0.0f;//点击后计时器清零
    }
    void Update()
    {
        // dice auto rotate
        if (timer < 3.0f)//规定 旋转时间
        {
            //旋转骰子
            transform.Rotate(new Vector3(transform.rotation.x + p_x, transform.rotation.y + p_y, transform.rotation.z + p_z));
        }
        GetNum();
        timer += 0.05f;
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
