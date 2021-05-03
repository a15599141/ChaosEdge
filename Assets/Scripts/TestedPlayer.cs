using System.Collections;
using UnityEngine;
using SWNetwork;

public class TestedPlayer : MonoBehaviour
{
    public int routePosition;//玩家所在格子位置
    public bool isEngaging; //玩家是否遭遇
    public bool isOnTradeStation;//玩家是否在交易站
    public bool canConstructHere; //玩家是否可以在此建造

    //玩家属性
    public string id;
    int energy;//玩家当前能量币
    int maxHP; //飞船总血量
    int currHP;//飞船当前血量
    int atk;//飞船攻击
    int def;//飞船防御
    //int evo;//飞船闪避
    //int bag;//飞船格子大小

    private void Awake()
    {
        isEngaging = false;
        isOnTradeStation = false;
        canConstructHere = false;
        energy = 10;
        maxHP = 6;
        currHP = 6;
        atk = 3;
        def = 2;
        //evo = 1;
        //bag = 16;
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
    public string getEnergy()
    {
        return energy.ToString();
    }

    public bool setEnergy(int e)
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

    public string getMaxHP()
    {
        return maxHP.ToString();
    }
    public string getCurrHP()
    {
        return currHP.ToString();
    }

    public string getATK()
    {
        return atk.ToString();
    }
    public string getDEF()
    {
        return def.ToString();
    }
}
