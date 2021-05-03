using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    private static CanvasManager _instance;

    public static CanvasManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Canvas Manager").GetComponent<CanvasManager>();
            }
            return _instance;
        }
    }

    public GameObject canvasBasic;
    private RawImage playerPanelHighlight;

    public GameObject canvasShop;
    public RawImage itemHighlight;
    public RawImage equipmentHighlight;
    public RawImage spaceShipHighlight;

    public GameObject canvasBattle;
    public GameObject canvasConfirm;
    public Button buttonConfirm;
    public Button buttonCancel;
    public Text textConfirm;
    
    public GameObject[] playerPanels;
    public Button[] items;
    public Button[] equipments;
    public Button[] spaceShips;

    // Start is called before the first frame update
    void Start()
    {
        playerPanelHighlight = canvasBasic.GetComponentInChildren<RawImage>();
        foreach (Button item in items) item.onClick.AddListener(itemSelect);
        //foreach (Button equipment in equipments) equipment.onClick.AddListener(BattleConfirm);
        //foreach (Button spaceShip in spaceShips) spaceShip.onClick.AddListener(BattleConfirm);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.currPlayerIndex == 0) playerPanelHighlight.transform.position = playerPanels[0].transform.position;
        if (PlayerManager.Instance.currPlayerIndex == 1) playerPanelHighlight.transform.position = playerPanels[1].transform.position;
        if (PlayerManager.Instance.currPlayerIndex == 2) playerPanelHighlight.transform.position = playerPanels[2].transform.position;
        if (PlayerManager.Instance.currPlayerIndex == 3) playerPanelHighlight.transform.position = playerPanels[3].transform.position;
    }

    public void IsConfirm(ConfirmType type)
    {
        canvasConfirm.SetActive(true);
        switch (type)
        {
            case ConfirmType.isBattle:
                textConfirm.text = "Battle?";
                buttonConfirm.onClick.AddListener(BattleConfirm);
                buttonCancel.onClick.AddListener(BattleCancel);
                break;
            case ConfirmType.isTradeStation:
                textConfirm.text = "Trade?";
                buttonConfirm.onClick.AddListener(TradeStationConfirm);
                buttonCancel.onClick.AddListener(TradeStationCancel);
                break;
            case ConfirmType.isConstruction:
                textConfirm.text = "Constract? $100";
                buttonConfirm.onClick.AddListener(ConstructConfirm);
                buttonCancel.onClick.AddListener(ConstructCancel);
                break;
        }
    }
    public void ConstructConfirm()
    {
        TestedPlayer player = PlayerManager.Instance.currPlayer;
        Material ma = player.transform.GetChild(0).GetComponent<MeshRenderer>().material;// 获取玩家颜色
        Transform tm = Route.Instacnce.childNodeList[player.routePosition].transform.GetChild(0);//获取玩家要建造的位置
        tm.GetComponent<MeshRenderer>().material.SetColor("_Color", ma.color);//覆盖玩家颜色到位置

        player.energy -= 100;
        ConstructCancel();
    }

    public void ConstructCancel()
    {
        PlayerManager.Instance.EndTheTurn();
        ConfirmExit();
    }

    public void BattleConfirm()
    {

    }

    public void BattleCancel()
    {
        PlayerManager.Instance.BattleCancel();
        ConfirmExit();
    }
    public void TradeStationConfirm()
    {
        canvasShop.SetActive(true);
        ConfirmExit();
    }
    public void TradeStationCancel()
    {
        PlayerManager.Instance.TradeCancel();
        ConfirmExit();
    }

    void ConfirmExit()
    {
        canvasConfirm.SetActive(false);
        buttonConfirm.onClick.RemoveAllListeners();
        buttonCancel.onClick.RemoveAllListeners();
    }

    public void OpenCanvasShop()
    {
        canvasShop.SetActive(true);
    }
    public void CloseTradeStation()
    {
        itemHighlight.gameObject.SetActive(false);
        equipmentHighlight.gameObject.SetActive(false);
        spaceShipHighlight.gameObject.SetActive(false);
        PlayerManager.Instance.currPlayer.isOnTradeStation = false;
        PlayerManager.Instance.EndTheTurn();
        canvasShop.SetActive(false);
    }

    public void UpdatePlayerPanel()
    {
        int panelIndex = 0;
        foreach (GameObject item in PlayerManager.Instance.playerObjects)
        {
            TestedPlayer player = item.GetComponent<TestedPlayer>();
            playerPanels[panelIndex].transform.GetChild(2).GetComponent<Text>().text 
                = "Energy: "+ player.energy.ToString();
            playerPanels[panelIndex].transform.GetChild(3).GetComponent<Text>().text
                = "HP: " + player.getCurrHP() + "/" +player.getMaxHP();
            panelIndex++;
        }
    }

    public void itemSelect()
    {
        itemHighlight.gameObject.SetActive(true);
        //itemHighlight.transform.position = button.transform.position;
    }
}
