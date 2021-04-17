using System.Collections;
using UnityEngine;
using SWNetwork;

public class TestedPlayer : MonoBehaviour
{
    public int routePosition;//玩家所在格子位置
    public bool engagement; //玩家是否遭遇

    //玩家属性
    string id;
    public int energy;//玩家当前能量币
    int maxHP; //飞船总血量
    int currHP;//飞船当前血量
    int atk;//飞船攻击
    int def;//飞船防御
    int evo;//飞船闪避
    int bag;//飞船格子大小


    private void Awake()
    {
        engagement = false;
        energy = 1000;
        maxHP = 5;
        currHP = 5;
        atk = 1;
        def = 1;
        evo = 1;
        bag = 16;
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
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("battle incomming");
            engagement = true;
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
}
