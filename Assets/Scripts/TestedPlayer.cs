using System.Collections;
using UnityEngine;
using SWNetwork;
using System.Collections.Generic;
using UnityEngine.UI;

public class TestedPlayer : MonoBehaviour
{
    public int routePosition;//玩家所在格子位置
    public bool isEngaging; //玩家是否遭遇
    public TestedPlayer tarPlayer;//目标玩家
    public bool isTargetPlayer;// 玩家战斗目标是否为玩家 true=玩家 false=玩家的空间站
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
        energy = 10;
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
                tarPlayer = other.GetComponent<TestedPlayer>();
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
    public void DirectSetEnergy(int e) //直接设置能源
    {
        if (e >= 0)energy = e;
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

    public void Repaire()
    {
        currHP = maxHP;
    }

    //战斗
    public string Battle(int diceNum1, int diceNum2,Station sta)
    {
        int res;
        if (isTargetPlayer)
        {
            res =  (atk + diceNum1) - (tarPlayer.def + diceNum2);//计算战斗伤害
            res = res <= 0 ? 1 : res;//小于1固定1点伤害
            tarPlayer.currHP -= res;
            return tarPlayer.name + " got " + res + " damage from " + name;
        }
        else
        {
            res = (atk + diceNum1) - (sta.def + diceNum2);
            res = res <= 0 ? 1 : res;
            sta.setHP(res);
            return tarPlayer.name + "'station got " + res + " damage from " + name;
        }
    }
}
