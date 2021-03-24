using UnityEngine;
 
public class Dice : MonoBehaviour
{
    void Start()
    {

    }
    // Update is called once per frame
    public void Update()
    {

    }

    public int GetNum()
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
        return int.Parse(upFace.name);//将朝上面 的名字 转化为int
        //Debug.Log("点数是： " + DiceFaceUpNum);
    }
}
