using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsConstruct : MonoBehaviour
{
    public TestedPlayer player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConstructConfirm()
    {
        Material ma = player.transform.GetChild(0).GetComponent<MeshRenderer>().material;// 获取玩家颜色
        Transform tm = player.currentRoute.childNodeList[player.routePosition].transform.GetChild(0);//获取玩家要建造的位置
        tm.GetComponent<MeshRenderer>().material.SetColor("_Color", ma.color);//覆盖玩家颜色到位置
        CanvasManager.Instance.CloseCanvasStation();
        player.dice.rollButton.interactable = true;//释放按钮
    }

    public void ConstructCancel()
    {
        CanvasManager.Instance.CloseCanvasStation();
        player.dice.rollButton.interactable = true;//释放按钮
    }
}
