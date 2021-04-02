using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SWNetwork;
using UnityEngine.SceneManagement;

public class Dice : MonoBehaviour
{
    private Rigidbody diceRb;   //骰子刚体
    RemoteEventAgent remoteEventAgent;

    public Button rollButton;   //掷骰子按钮
    public TMP_Text roundText;  //游戏轮数显示器

    public bool moveAllowed;    //玩家可移动
    public int diceNumber; //骰子点数
    private int roundCount;     //游戏轮数计数器
    private int rotateLimit = 30;   //骰子转动速度

    // Start is called before the first frame update
    void Start()
    {
        diceRb = GetComponent<Rigidbody>();
        remoteEventAgent = GetComponent<RemoteEventAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RollDiceOnClick()
    {
        roundCount++; //回合数加1
        roundText.text = "ROUND " + roundCount.ToString();//更新回合数
        rollButton.interactable = false; // 禁用摇色子按钮
        remoteEventAgent.Invoke("RollDice");
        StartCoroutine("RollDice");//启动骰子协程
    }
    //协程控制骰子转动
    private IEnumerator RollDice()
    {
        //转动骰子
        for (int i = 0; i < rotateLimit; i++)
        {
            transform.Rotate(new Vector3(transform.rotation.x + RandomRotation(), transform.rotation.y + RandomRotation(), transform.rotation.z + RandomRotation()));
            yield return new WaitForSeconds(0.02f);
        }

        //每0.05秒验证一次骰子是否依旧在转动
        while (true)
        {

            if (diceRb.velocity.Equals(new Vector3(0.0f, 0.0f, 0.0f)))
            {
                //Debug.Log("stop rolling");
                moveAllowed = true;
                diceNumber = GetNum();
                yield break;//结束协程
            }
            else
            {
                //Debug.Log("still rolling");
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    private int RandomRotation()
    {
        return Random.Range(rotateLimit, -rotateLimit);
    }

    public int GetNum()
    {
        Transform[] obj = new Transform[6];//声明数组存放色子的六个面
        Transform upFace= transform.GetChild(0).GetChild(0);//声明朝上的面 
        for (int i = 0; i < 6; i++)//循环判断哪个面朝上
        {
            obj[i] = transform.GetChild(0).GetChild(i);
            if (obj[i].position.y > upFace.position.y) // 判断每个面y坐标的值
            {
                upFace = obj[i];
            }
        }
        return int.Parse(upFace.name);//将朝上面 的名字 转化为int
        //Debug.Log("点数是： " + DiceFaceUpNum);
    }
}
