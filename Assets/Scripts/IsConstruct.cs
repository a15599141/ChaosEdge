using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsConstruct : MonoBehaviour
{
    
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
        TestedPlayer player = PlayerManager.Instance.currPlayer;
        Material ma = player.transform.GetChild(0).GetComponent<MeshRenderer>().material;// 获取玩家颜色
        Transform tm = Route.Instacnce.childNodeList[player.routePosition].transform.GetChild(0);//获取玩家要建造的位置
        tm.GetComponent<MeshRenderer>().material.SetColor("_Color", ma.color);//覆盖玩家颜色到位置

        player.energy -= 100;
        OnExit();
    }

    public void ConstructCancel()
    {
        OnExit();
    }

    public void OnExit()
    {
        CanvasManager.Instance.CloseCanvasStation();
        PlayerManager.Instance.dice.rollButton.interactable = true;//释放按钮
        PlayerManager.Instance.EndTheTurn();
    }
}
