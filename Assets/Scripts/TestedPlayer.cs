using System.Collections;
using UnityEngine;
using SWNetwork;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestedPlayer : MonoBehaviour
{
    public int routePosition;//玩家所在格子位置
    public bool isEngaging; //玩家是否遭遇
    public bool isOnTradeStation;//玩家是否在交易站

    //玩家属性
    public string id;
    int energy;//玩家当前能量币
    int maxHP; //飞船总血量
    int currHP;//飞船当前血量
    int atk;//飞船攻击
    int def;//飞船防御
    int evo;//飞船闪避
    public List<Item> playerItemBag = new List<Item>(); //玩家道具背包
    public List<Equipment> playerEquipmentBag = new List<Equipment>(); //玩家战斗装备背包
    private void Awake() //初始状态及属性
    {
        isEngaging = false;
        isOnTradeStation = false;
        energy = 100;
        maxHP = 6;
        currHP = 6;
        atk = 3;
        def = 2;
        evo = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
  
    }
    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("TradeStation"))
        {
            Debug.Log("TradeStation arrived");
            isOnTradeStation = true;
        }
        else if (other.transform.CompareTag("Player"))
        {
            //判断当前玩家进入战斗状态
            if (PlayerManager.Instance.currPlayer == this)
            {
                Debug.Log(name + " vs " + other.name + "battle incomming");
                isEngaging = true;
            }
        }
       
    }
    public int getEnergy()
    {
        return energy;
    }

    public bool setEnergy(int e) //扣钱e设置负数，加钱设置e为正数
    {
        if (energy+e<0)
        {
            return false;
        }
        else
        {
            energy += e;
            return true;
        }
    }
    public int getMaxHP()
    {
        return maxHP;
    }
    public int getCurrHP()
    {
        return currHP;
    }

    public int getATK()
    {
        return atk;
    }
    public int getDEF()
    {
        return def;
    }
    public int getEVO()
    {
        return evo;
    }
}
