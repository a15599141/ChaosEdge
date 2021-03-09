﻿using UnityEngine;
using UnityEngine.UI;

public class Roll : MonoBehaviour
{
    GameObject Dice;
    GameObject testedPlayer;
    private int p_x, p_y, p_z; // 色子的旋转坐标变换
    public float timer; // 计时器
    public int DiceFaceUpNum; // 记录色子点数
    public float checktime;
    public bool diceIsRotating; // 记录色子是否在旋转
    public bool playerIsMoving;
    public int roundCount;  // 记录回合数
    Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
        checktime = 0;
        diceIsRotating = false;
        timer = 4.0f; // 初始化计时器，大于规定旋转时间即可
        roundCount = 0;
        Dice = GameObject.Find("Dice").gameObject;
        testedPlayer = GameObject.Find("testedPlayer").gameObject;
        lastPosition = Dice.transform.position;
    }
    void OnClick()
    {
      //取XYZ的随机旋转值 
       p_x = Random.Range(1, 30);
       p_y = Random.Range(1, 30);
       p_z = Random.Range(1, 30);
       timer = 0.0f;//点击后计时器清零
       roundCount++; //回合数加1
       GetComponent<Button>().interactable = false;
       diceIsRotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!diceIsRotating && !playerIsMoving) GetComponent<Button>().interactable = true;
        playerIsMoving = testedPlayer.GetComponent<testedPlayer>().playerIsMoving;
        // dice auto rotate
        if (timer < 3.0f)//规定 旋转时间
        {
            //旋转骰子
            Dice.transform.Rotate(new Vector3(Dice.transform.rotation.x + p_x, Dice.transform.rotation.y + p_y, Dice.transform.rotation.z + p_z));
            timer += 0.02f;
        }
        else diceIsRotating = false;
        
    }
}
