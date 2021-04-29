using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SWNetwork;

public class DiceOnline : MonoBehaviour
{
    public GameObject dice;
    public Button rollButton;   //掷骰子按钮
    public TMP_Text roundText;  //游戏轮数显示器
    public int roundCount;     //游戏轮数计数器

    Transform[] sixFaces = new Transform[6];// 声明数组, 存放色子的六个面坐标
    Transform upFace;
    public int diceNumber; //骰子点数

    NetworkID networkID;
    RemoteEventAgent remoteEventAgent;
    SyncPropertyAgent syncPropertyAgent;

    // Start is called before the first frame update
    void Start()
    {
        networkID = GetComponent<NetworkID>();
        remoteEventAgent = GetComponent<RemoteEventAgent>();
        syncPropertyAgent = GetComponent<SyncPropertyAgent>();
        dice.SetActive(false);
        rollButton = GameObject.Find("Canvas/RollButton").GetComponent<Button>();
        rollButton.onClick.AddListener(RollDiceOnClick);
    }

    // Update is called once per frame
    void Update()
    {  
        //roundText.text = "ROUND " + roundCount.ToString(); //更新回合数
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(diceNumber.ToString())) //如果骰子停止旋转
        {
            GetComponent<Animator>().Play("idle" + diceNumber.ToString(), 0); //播放骰子闲置动画
            PlayerManager.Instance.moveAllowed = true; // 允许玩家移动
        }
    }

    public void RollDiceOnClick() 
    {
        //roundCount++; //回合数加1
        //roundText.text = "ROUND " + roundCount.ToString(); //更新回合数
        if (networkID.IsMine)
        {
            rollButton.interactable = false; // 禁用摇色子按钮
            diceNumber = Random.Range(1, 7); // 生成1到6的随机整数，作为最后的骰子点数
            GetComponent<Animator>().Play("Rotate to " + diceNumber.ToString(), 0);// 根据点数播放骰子相应动画
        }

        //StartCoroutine(RollDice());      // 启动骰子协程  
    }
    //协程控制骰子转动
    /*IEnumerator RollDice()
    {
        //每0.05秒验证一次骰子是否依旧在转动
        while (true)
        {
            if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(diceNumber.ToString()))
            {
                //Debug.Log("stop rolling");
                GetComponent<Animator>().Play("idle" + diceNumber.ToString(), 0);
                PlayerManager.Instance.moveAllowed = true;
                yield break;//结束协程
            }
            else
            {
                //Debug.Log("still rolling");
                yield return new WaitForSeconds(0.05f);
            }
        }
    }*/
    /*public int GetNum()
    {
        upFace = transform.GetChild(0); // 初始化朝上的面 
        for (int i = 0; i < 6; i++) // 遍历六个面
        {
            sixFaces[i] = transform.GetChild(i);
            if (sixFaces[i].position.y > upFace.position.y) // 判断每个面y坐标的值大小确定那个面朝上
            {
                upFace = sixFaces[i]; // 重新赋值向上的面
            }
        }
        return int.Parse(upFace.name);//将朝上面 的名字 转化为int
        //Debug.Log("点数是： " + DiceFaceUpNum);
    }*/

    /* -------- 联机向-------- */

}
