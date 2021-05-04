using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    public RawImage playerPanelHighlight;

    public GameObject canvasShop;
    public RawImage itemHighlight;
    public RawImage equipmentHighlight;
    public RawImage spaceShipHighlight;

    public GameObject canvasBattle;

    public GameObject canvasMessage;
    public Text textMessage;
    public GameObject canvasConfirm;
    public Button buttonConfirm;
    public Button buttonCancel;
    public Text textConfirm;
    
    public GameObject[] playerPanels;
    // 商店物品
    public Button[] items; 
    public Button[] equipments;
    public Button[] spaceShips;

    // 商店物品构造体及列表
    struct Item
    {
        public string name;
        public int energyCost;
        public string description;
        public int HP;
        public Item(string n, int e, string d, int H){ name = n; energyCost = e; description = d; HP = H; }
    }; List<Item> itemList = new List<Item>();
    struct Equipment
    {
        public string name;
        public int energyCost;
        public string description;
        public int level;
        public int ATK;
        public int DEF;
        public int EVO;
        public Equipment(string n, int e, string d, int l, int A, int D, int E)
        { name = n; energyCost = e; description = d; level = l; ATK = A; DEF = D; EVO = E; }
    }; List<Equipment> equipmentList = new List<Equipment>();
    struct SpaceShip
    {
        public string name;
        public int energyCost;
        public string description;
        public int level;
        public int HP;
        public int ATK;
        public int DEF;
        public int EVO;
        public SpaceShip(string n, int e, string d, int l, int H, int A, int D, int E)
        { name = n; energyCost = e; description = d; level = l; HP = H; ATK = A; DEF = D; EVO = E; }
    }; List<SpaceShip> spaceShipList = new List<SpaceShip>();

    public TMP_Text roundText;  //游戏轮数显示器
    public int roundCount;     //游戏轮数计数器

    // Start is called before the first frame update
    void Start()
    {
        roundCount = 1; //初始回合为1
        TradeStationInitialize(); // 商店初始化

    }
    // Update is called once per frame
    void Update()
    {
        //当前回合玩家属性面板高亮
        if (PlayerManager.Instance.currPlayerIndex == 0) playerPanelHighlight.transform.position = playerPanels[0].transform.position;
        if (PlayerManager.Instance.currPlayerIndex == 1) playerPanelHighlight.transform.position = playerPanels[1].transform.position;
        if (PlayerManager.Instance.currPlayerIndex == 2) playerPanelHighlight.transform.position = playerPanels[2].transform.position;
        if (PlayerManager.Instance.currPlayerIndex == 3) playerPanelHighlight.transform.position = playerPanels[3].transform.position;

    }

    public void showMessage(string msg)
    {
        textMessage.text = msg;
        canvasMessage.SetActive(true);
        StartCoroutine(delayMessage());
    }
    IEnumerator delayMessage()
    {
        yield return new WaitForSeconds(1.5f);
        canvasMessage.SetActive(false);
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
                textConfirm.text = "Constract? $5";
                buttonConfirm.onClick.AddListener(ConstructConfirm);
                buttonCancel.onClick.AddListener(ConstructCancel);
                break;
        }
    }

    public void ConstructConfirm()
    {

        TestedPlayer player = PlayerManager.Instance.currPlayer;
        //消费能量
        if (player.setEnergy(-5))
        {
            Material ma = player.transform.GetChild(0).GetComponent<MeshRenderer>().material;// 获取玩家颜色
            Transform tm = Route.Instacnce.childNodeList[player.routePosition].transform.GetChild(0);//获取玩家要建造的位置
            tm.GetComponent<MeshRenderer>().material.SetColor("_Color", ma.color);//覆盖玩家颜色到位置
            PlayerManager.Instance.stations[player.routePosition] = new Station(player);//添加入station集合
        }
        else
        {
            showMessage("not enough energy!");
        }
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
            playerPanels[panelIndex].transform.GetChild(1).GetComponent<Text>().text
                = "Name: " + player.id;
            playerPanels[panelIndex].transform.GetChild(2).GetComponent<Text>().text
                = "Energy: " + player.getEnergy();
            playerPanels[panelIndex].transform.GetChild(3).GetComponent<Text>().text
                = "HP: " + player.getCurrHP() + "/" +player.getMaxHP();

            playerPanels[panelIndex].transform.GetChild(4).GetComponent<Text>().text
                = "ATK: " + player.getATK();

            playerPanels[panelIndex].transform.GetChild(5).GetComponent<Text>().text
                = "DEF: " + player.getDEF();
            panelIndex++;
        }
        roundText.text = "ROUND " + roundCount.ToString(); //更新回合数
    }

    public void TradeStationInitialize()
    {
        //添加Item到列表
        itemList.Add(new Item("HP Recovery1", 3, "This is a Recovery", 1));
        itemList.Add(new Item("HP Recovery2", 4, "This is a Recovery", 2));
        itemList.Add(new Item("HP Recovery3", 5, "This is a Recovery", 3));
        //添加Equipment到列表
        equipmentList.Add(new Equipment("Ammo1", 3, "This is a ammo", 1, 1, 0, 0));
        equipmentList.Add(new Equipment("Ammo2", 5, "This is a ammo", 2, 2, 0, 0));
        equipmentList.Add(new Equipment("Ammo3", 9, "This is a ammo", 3, 3, 0, 0));
        equipmentList.Add(new Equipment("Shield1", 3, "This is a shield", 1, 0, 1, 0));
        equipmentList.Add(new Equipment("Shield2", 5, "This is a shield", 2, 0, 2, 0));
        equipmentList.Add(new Equipment("Shield3", 7, "This is a shield", 3, 0, 3, 0));
        equipmentList.Add(new Equipment("Engine1", 3, "This is a engine", 1, 0, 0, 1));
        equipmentList.Add(new Equipment("Engine2", 5, "This is a engine", 2, 0, 0, 2));
        equipmentList.Add(new Equipment("Engine3", 9, "This is a engine", 3, 0, 0, 3));

        //添加SpaceShip到列表
        spaceShipList.Add(new SpaceShip("SpaceShip1", 20, "This is a SpaceShip", 1, 8,  4, 3, 2));
        spaceShipList.Add(new SpaceShip("SpaceShip2", 25, "This is a SpaceShip", 2, 10, 6, 5, 4));
        spaceShipList.Add(new SpaceShip("SpaceShip3", 30, "This is a SpaceShip", 3, 12, 8, 7, 6));
        spaceShipList.Add(new SpaceShip("SpaceShip1", 20, "This is a SpaceShip", 1, 8, 4, 3, 2));
        spaceShipList.Add(new SpaceShip("SpaceShip2", 25, "This is a SpaceShip", 2, 10, 6, 5, 4));
        spaceShipList.Add(new SpaceShip("SpaceShip3", 30, "This is a SpaceShip", 3, 12, 8, 7, 6));
        spaceShipList.Add(new SpaceShip("SpaceShip1", 20, "This is a SpaceShip", 1, 8, 4, 3, 2));
        spaceShipList.Add(new SpaceShip("SpaceShip2", 25, "This is a SpaceShip", 2, 10, 6, 5, 4));
        spaceShipList.Add(new SpaceShip("SpaceShip3", 30, "This is a SpaceShip", 3, 12, 8, 7, 6));
        spaceShipList.Add(new SpaceShip("SpaceShip1", 20, "This is a SpaceShip", 1, 8, 4, 3, 2));
        spaceShipList.Add(new SpaceShip("SpaceShip2", 25, "This is a SpaceShip", 2, 10, 6, 5, 4));
        spaceShipList.Add(new SpaceShip("SpaceShip3", 30, "This is a SpaceShip", 3, 12, 8, 7, 6));


        // 商店物品选中高亮显示, 选种物品详情显示
        
        foreach (Button item in items) item.onClick.AddListener(() =>
        {
            itemHighlight.transform.position = item.transform.position;
            int idx = Array.IndexOf(items, item);
            string str = "CanvasTradeStation/Items/Detail/"; //详情的目录
            GameObject.Find(str + "Name").GetComponent<Text>().text = itemList[idx].name;
            GameObject.Find(str + "EnergyCost").GetComponent<Text>().text = itemList[idx].energyCost + " Energy";
            GameObject.Find(str + "Description").GetComponent<Text>().text = itemList[idx].description;
            GameObject.Find(str + "Effect").GetComponent<Text>().text =
            "Recovery: HP +" + itemList[idx].HP;
            GameObject.Find(str + "Image").GetComponent<RawImage>().texture = item.GetComponent<RawImage>().texture;
        });
        foreach (Button equipment in equipments) equipment.onClick.AddListener(() =>
        {
            equipmentHighlight.transform.position = equipment.transform.position;
            int idx = Array.IndexOf(equipments, equipment);
            string str = "CanvasTradeStation/Equipments/Detail/"; //详情的目录
            GameObject.Find(str + "Name").GetComponent<Text>().text = equipmentList[idx].name;
            GameObject.Find(str + "EnergyCost").GetComponent<Text>().text = equipmentList[idx].energyCost + " Energy";
            GameObject.Find(str + "Description").GetComponent<Text>().text = equipmentList[idx].description;
            GameObject.Find(str + "Effect").GetComponent<Text>().text =
            "ATK+" + equipmentList[idx].ATK +
            "  DEF+" + equipmentList[idx].DEF +
            "  EVO+" + equipmentList[idx].EVO;
            GameObject.Find(str + "Image").GetComponent<RawImage>().texture = equipment.GetComponent<RawImage>().texture;
        });
        foreach (Button spaceShip in spaceShips) spaceShip.onClick.AddListener(() =>
        {
            spaceShipHighlight.transform.position = spaceShip.transform.position;
            int idx = Array.IndexOf(spaceShips, spaceShip);
            string str = "CanvasTradeStation/SpaceShips/Detail/"; //详情的目录
            GameObject.Find(str + "Name").GetComponent<Text>().text = spaceShipList[idx].name;
            GameObject.Find(str + "EnergyCost").GetComponent<Text>().text = spaceShipList[idx].energyCost.ToString() + " Energy";
            GameObject.Find(str + "Description").GetComponent<Text>().text = spaceShipList[idx].description;
            GameObject.Find(str + "Effect").GetComponent<Text>().text =
            "HP+" + spaceShipList[idx].HP +
            "  ATK+" + spaceShipList[idx].ATK +
            "  DEF+" + spaceShipList[idx].DEF +
            "  EVO+" + spaceShipList[idx].EVO;
            GameObject.Find(str + "Image").GetComponent<RawImage>().texture = spaceShip.GetComponent<RawImage>().texture;
        });
    }
}
