using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SWNetwork;

public class Dice : MonoBehaviour
{
    private Rigidbody diceRb;   //骰子刚体

    public Button rollButton;   //掷骰子按钮

    public TMP_Text roundText;  //游戏轮数显示器
    private int roundCount;     //游戏轮数计数器

    public bool moveAllowed;    //玩家可移动

    Transform[] sixFaces = new Transform[6];// 声明数组, 存放色子的六个面坐标
    Transform upFace;
    public Vector3 currentPosition;
    private int rotateLimit = 30;   //骰子转动速度
    private int randomRotation; // 骰子转动随机值
    public int diceNumber; //骰子点数

    SyncPropertyAgent syncPropertyAgent;

    // Start is called before the first frame update
    void Start()
    {
        diceRb = GetComponent<Rigidbody>(); // 获取刚体属性
        syncPropertyAgent = GetComponent<SyncPropertyAgent>();
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
        StartCoroutine("RollDice");//启动骰子协程
    }
    //协程控制骰子转动
    private IEnumerator RollDice()
    {
        //转动骰子
        for (int i = 0; i < rotateLimit; i++)
        {
            randomRotation = Random.Range(rotateLimit, -rotateLimit);
            transform.Rotate(new Vector3(transform.rotation.x + randomRotation, transform.rotation.y + randomRotation, transform.rotation.z + randomRotation));
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
    public int GetNum()
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
    }

    /* -------- 联机向-------- */
    public void OnDiceTransformReady()
    {
        Debug.Log("OnDiceTransformReady");
        // Get the current value of the "Dice Transform" SyncProperty.
        currentPosition = syncPropertyAgent.GetPropertyWithName("diceTransform").GetVector3Value();
        int version = syncPropertyAgent.GetPropertyWithName("diceTransform").version;
       
        if (version == 0)
        {
            syncPropertyAgent.Modify("diceTransform", transform.position);
        }
    }
    public void OnDiceTransformChanged()
    {
        Debug.Log("OnDiceTransformChanged");
        // Update the hpSlider when player hp changes
        currentPosition = syncPropertyAgent.GetPropertyWithName("diceTransform").GetVector3Value();
        transform.position = currentPosition;
    }
}
