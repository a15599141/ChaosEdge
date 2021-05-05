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
    public GameObject[] playerPanels; //玩家属性面板
    public RawImage playerPanelHighlight;//玩家属性面板高亮

    public GameObject canvasBattle;

    public GameObject canvasMessage;
    public Text textMessage;
    public GameObject canvasConfirm;
    public Button buttonConfirm;
    public Button buttonCancel;
    public Text textConfirm;

    public GameObject canvasShop;
    public RawImage itemHighlight;
    public RawImage equipmentHighlight;
    public RawImage spaceShipHighlight;
    public Button[] items;     // 商店道具按钮
    public Button[] equipments;// 商店战斗装备按钮
    public Button[] spaceShips;// 商店飞船按钮
    public Button buyButton; // 购买按钮
    
    // 商店物品列表
    List<Item> itemList = new List<Item>();
    List<Equipment> equipmentList = new List<Equipment>();
    List<SpaceShip> spaceShipList = new List<SpaceShip>();

    //储存商店物品图片，储存顺序查看inspector
    public RawImage[] itemImages; //道具图片
    public RawImage[] equipmentImages; //战斗装备图片
    public RawImage[] spaceShipImages;//飞船图片

    //定义背包容量
    public RawImage[] bagImages; // UI界面的背包图片
    public static int itemBagMaxCapacity = 2;
    public static int equipmentBagMaxCapacity = 3;

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
        playerPanelHighlight.transform.position = playerPanels[PlayerManager.Instance.currPlayerIndex].transform.position; //当前玩家面板高亮
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
                textConfirm.text = "Construct? $5";
                buttonConfirm.onClick.AddListener(ConstructConfirm);
                buttonCancel.onClick.AddListener(ConstructCancel);
                break;
            case ConfirmType.energyNotEnough:
                textConfirm.text = "Energy not enough";
                buttonConfirm.onClick.AddListener(ConfirmExit);
                buttonCancel.onClick.AddListener(ConfirmExit);
                break;
            case ConfirmType.itemBagIsFull:
                textConfirm.text = "Item bag is full";
                buttonConfirm.onClick.AddListener(ConfirmExit);
                buttonCancel.onClick.AddListener(ConfirmExit);
                break;
            case ConfirmType.equipmentBagIsFull:
                textConfirm.text = "Equipment bag is full";
                buttonConfirm.onClick.AddListener(ConfirmExit);
                buttonCancel.onClick.AddListener(ConfirmExit);
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
    public void UpdatePlayerBag() //更新当前玩家背包图片
    {
        foreach (RawImage img in bagImages) img.texture = null; // 清空材质

        for (int i = 0; i < PlayerManager.Instance.currPlayer.playerItemBag.Count; i++) //从当前玩家背包加载道具图片材质
        {
            bagImages[i].texture = PlayerManager.Instance.currPlayer.playerItemBag[i].image.texture; //替换材质
            bagImages[i].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = PlayerManager.Instance.currPlayer.playerItemBag[i].name;//鼠标悬停细节显示（名字）
            bagImages[i].transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "Current HP +"+PlayerManager.Instance.currPlayer.playerItemBag[i].HP;//鼠标悬停细节显示（效果）
        }
        for (int i = 0; i < PlayerManager.Instance.currPlayer.playerEquipmentBag.Count; i++) //从当前玩家背包加载战斗装备图片材质
        {
            bagImages[i + itemBagMaxCapacity].texture = PlayerManager.Instance.currPlayer.playerEquipmentBag[i].image.texture; //替换材质
            bagImages[i + itemBagMaxCapacity].transform.GetChild(0).GetChild(2).GetComponent<Text>().text = PlayerManager.Instance.currPlayer.playerEquipmentBag[i].name;//鼠标悬停细节显示（名字）
            //鼠标悬停细节显示（效果）
            bagImages[i + itemBagMaxCapacity].transform.GetChild(0).GetChild(3).GetComponent<Text>().text =
                "ATK+"+PlayerManager.Instance.currPlayer.playerEquipmentBag[i].ATK+
                " DEF+" + PlayerManager.Instance.currPlayer.playerEquipmentBag[i].DEF+
                " EVO+" + PlayerManager.Instance.currPlayer.playerEquipmentBag[i].EVO;
        }
            
    }
    public void TradeStationInitialize()
    {
        //按图片顺序，添加Item到列表
        itemList.Add(new Item("Fix packs1", 3, "Based on your current health, grant your spaceship a +3 HP value this turn. It consumes three points of energy", 3, itemImages[0]));
        itemList.Add(new Item("Fix packs2", 5, "Based on your current health, grant your spaceship a +5 HP value this turn. It consumes eight points of energy.", 5, itemImages[1]));
        itemList.Add(new Item("Fix packs3", 15, "Based on your current health, grant your spaceship a +8 HP value this turn. It consumes fifteen points of energy", 9, itemImages[2]));
        //添加Equipment到列表
        equipmentList.Add(new Equipment("Ammo1", 3, "Grant your spaceship a +1 attack value this turn. It consumes three points of energy.", 1, 1, 0, 0, equipmentImages[0]));
        equipmentList.Add(new Equipment("Ammo2", 5, "Grant your spaceship a +2 attack value this turn. It consumes five points of energy.", 2, 2, 0, 0, equipmentImages[1]));
        equipmentList.Add(new Equipment("Ammo3", 9, "Grant your spaceship a +3 attack value this turn. It consumes nine points of energy.", 3, 3, 0, 0, equipmentImages[2]));
        equipmentList.Add(new Equipment("Shield1", 3, "Grant your spaceship a +1 defense value this turn. It consumes three points of energy.", 1, 0, 1, 0, equipmentImages[3]));
        equipmentList.Add(new Equipment("Shield2", 5, "Grant your spaceship a +2 defense value this turn. It consumes five points of energy.", 2, 0, 2, 0, equipmentImages[4]));
        equipmentList.Add(new Equipment("Shield3", 7, "Grant your spaceship a +3 defense value this turn. It consumes seven points of energy.", 3, 0, 3, 0, equipmentImages[5]));
        equipmentList.Add(new Equipment("Engine1", 2, "Grant your spaceship a +1 dodge value this turn. It consumes two points of energy.", 1, 0, 0, 1, equipmentImages[6]));
        equipmentList.Add(new Equipment("Engine2", 7, "Grant your spaceship a +2 dodge value this turn. It consumes seven points of energy.", 2, 0, 0, 2, equipmentImages[7]));
        equipmentList.Add(new Equipment("Engine3", 10, "Grant your spaceship a +3 dodge value this turn. It consumes ten points of energy.", 3, 0, 0, 3, equipmentImages[8]));
        //添加SpaceShip到列表//
        spaceShipList.Add(new SpaceShip("SpaceShip1", 20, "Replace your current spaceship with the following properties of Level 1 spaceship that costs 20 Energy", 1, 8,  4, 3, 2));
        spaceShipList.Add(new SpaceShip("SpaceShip2", 25, "Replace your current spaceship with the following properties of Level 2 spaceship that costs 25 Energy", 2, 10, 6, 5, 4));
        spaceShipList.Add(new SpaceShip("SpaceShip3", 30, "Replace your current spaceship with the following properties of Level 3 spaceship that costs 30 Energy.", 3, 12, 8, 7, 6));
        spaceShipList.Add(new SpaceShip("SpaceShip1", 20, "Replace your current spaceship with the following properties of Level 1 spaceship that costs 20 Energy", 1, 8, 4, 3, 2));
        spaceShipList.Add(new SpaceShip("SpaceShip2", 25, "Replace your current spaceship with the following properties of Level 2 spaceship that costs 25 Energy", 2, 10, 6, 5, 4));
        spaceShipList.Add(new SpaceShip("SpaceShip3", 30, "Replace your current spaceship with the following properties of Level 3 spaceship that costs 30 Energy.", 3, 12, 8, 7, 6));
        spaceShipList.Add(new SpaceShip("SpaceShip1", 20, "Replace your current spaceship with the following properties of Level 1 spaceship that costs 20 Energy", 1, 8, 4, 3, 2));
        spaceShipList.Add(new SpaceShip("SpaceShip2", 25, "Replace your current spaceship with the following properties of Level 2 spaceship that costs 25 Energy", 2, 10, 6, 5, 4));
        spaceShipList.Add(new SpaceShip("SpaceShip3", 30, "Replace your current spaceship with the following properties of Level 3 spaceship that costs 30 Energy.", 3, 12, 8, 7, 6));
        spaceShipList.Add(new SpaceShip("SpaceShip1", 20, "Replace your current spaceship with the following properties of Level 1 spaceship that costs 20 Energy", 1, 8, 4, 3, 2));
        spaceShipList.Add(new SpaceShip("SpaceShip2", 25, "Replace your current spaceship with the following properties of Level 2 spaceship that costs 25 Energy", 2, 10, 6, 5, 4));
        spaceShipList.Add(new SpaceShip("SpaceShip3", 30, "Replace your current spaceship with the following properties of Level 3 spaceship that costs 30 Energy.", 3, 12, 8, 7, 6));

        // 商店物品按钮添加点击事件
        foreach (Button item in items) item.onClick.AddListener(() =>
        {
            itemHighlight.transform.position = item.transform.position;
            int idx = Array.IndexOf(items, item); // 获取道具按钮下标
            string str = "CanvasTradeStation/PanelForDrag/Items/Detail/"; //商店物品详情的目录
            //点击商店物体后，根据按钮下标，更新右侧物品详情（道具）
            GameObject.Find(str + "Name").GetComponent<Text>().text = itemList[idx].name;
            GameObject.Find(str + "EnergyCost").GetComponent<Text>().text = itemList[idx].energyCost + " Energy";
            GameObject.Find(str + "Description").GetComponent<Text>().text = itemList[idx].description;
            GameObject.Find(str + "Effect").GetComponent<Text>().text =
            "Recovery: HP +" + itemList[idx].HP;
            GameObject.Find(str + "Image").GetComponent<RawImage>().texture = itemList[idx].image.texture;
            // 商店道具购买按钮添加监听事件
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() =>
            {
                TestedPlayer currentPlayer = PlayerManager.Instance.currPlayer;//获取当前玩家对象
                if(currentPlayer.setEnergy(-itemList[idx].energyCost)) //如果买得起道具
                {
                    if (currentPlayer.playerItemBag.Count == itemBagMaxCapacity) IsConfirm(ConfirmType.itemBagIsFull); //如果背包已满,弹出提示框
                    else
                    {
                        currentPlayer.playerItemBag.Add(itemList[idx]);//放入背包
                        playerPanels[PlayerManager.Instance.currPlayerIndex].transform.GetChild(2).GetComponent<Text>().text 
                         = "Energy: " + currentPlayer.getEnergy(); // 更新玩家能源数据
                    }                                 
                }else IsConfirm(ConfirmType.energyNotEnough); //如果能源不够,弹出提示框

                UpdatePlayerBag();
            });
        });
        foreach (Button equipment in equipments) equipment.onClick.AddListener(() =>
        {
            equipmentHighlight.transform.position = equipment.transform.position;
            int idx = Array.IndexOf(equipments, equipment);
            string str = "CanvasTradeStation/PanelForDrag/Equipments/Detail/"; //详情的目录
            //点击商店物体后，更新右侧物品详情（战斗装备）
            GameObject.Find(str + "Name").GetComponent<Text>().text = equipmentList[idx].name;
            GameObject.Find(str + "EnergyCost").GetComponent<Text>().text = equipmentList[idx].energyCost + " Energy";
            GameObject.Find(str + "Description").GetComponent<Text>().text = equipmentList[idx].description;
            GameObject.Find(str + "Effect").GetComponent<Text>().text =
            "ATK+" + equipmentList[idx].ATK +
            "  DEF+" + equipmentList[idx].DEF +
            "  EVO+" + equipmentList[idx].EVO;
            GameObject.Find(str + "Image").GetComponent<RawImage>().texture = equipmentList[idx].image.texture;
            // 添加战斗装备购买按钮监听事件
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() =>
            {
                TestedPlayer currentPlayer = PlayerManager.Instance.currPlayer;//获取当前玩家对象
                if (currentPlayer.setEnergy(-equipmentList[idx].energyCost)) //如果买得起该装备
                {
                    if (currentPlayer.playerEquipmentBag.Count == equipmentBagMaxCapacity) IsConfirm(ConfirmType.equipmentBagIsFull); //如果背包已满,弹出提示框
                    else
                    {
                        currentPlayer.playerEquipmentBag.Add(equipmentList[idx]);//放入背包
                        playerPanels[PlayerManager.Instance.currPlayerIndex].transform.GetChild(2).GetComponent<Text>().text
                         = "Energy: " + currentPlayer.getEnergy(); // 更新玩家能源数据
                    }
                }
                else IsConfirm(ConfirmType.energyNotEnough); //如果能源不够,弹出提示框

                UpdatePlayerBag();
            });
        });
        foreach (Button spaceShip in spaceShips) spaceShip.onClick.AddListener(() =>
        {
            spaceShipHighlight.transform.position = spaceShip.transform.position;
            int idx = Array.IndexOf(spaceShips, spaceShip);
            string str = "CanvasTradeStation/PanelForDrag/SpaceShips/Detail/"; //详情的目录
            //点击商店物体后，更新右侧物品详情（飞船）
            GameObject.Find(str + "Name").GetComponent<Text>().text = spaceShipList[idx].name;
            GameObject.Find(str + "EnergyCost").GetComponent<Text>().text = spaceShipList[idx].energyCost.ToString() + " Energy";
            GameObject.Find(str + "Description").GetComponent<Text>().text = spaceShipList[idx].description;
            GameObject.Find(str + "Effect").GetComponent<Text>().text =
            "HP+" + spaceShipList[idx].HP +
            "  ATK+" + spaceShipList[idx].ATK +
            "  DEF+" + spaceShipList[idx].DEF +
            "  EVO+" + spaceShipList[idx].EVO;
            GameObject.Find(str + "Image").GetComponent<RawImage>().texture = spaceShip.GetComponent<RawImage>().texture;
            // 添加购买按钮监听事件
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() =>
            {
                int i = PlayerManager.Instance.currPlayerIndex;
                TestedPlayer currentPlayer = PlayerManager.Instance.playerObjects[i].GetComponent<TestedPlayer>();//获取当前玩家对象
                if (currentPlayer.setEnergy(-spaceShipList[idx].energyCost)) // 如果买得起
                {   // 更新玩家面板
                    playerPanels[i].transform.GetChild(0).GetComponent<RawImage>().texture
                     = spaceShip.GetComponent<RawImage>().texture;
                    playerPanels[i].transform.GetChild(2).GetComponent<Text>().text
                     = "Energy: " + currentPlayer.getEnergy();
                    playerPanels[i].transform.GetChild(3).GetComponent<Text>().text
                        = "HP: " + currentPlayer.getCurrHP() + "/" + spaceShipList[idx].HP;
                    playerPanels[i].transform.GetChild(4).GetComponent<Text>().text
                        = "ATK: " + spaceShipList[idx].ATK;
                    playerPanels[i].transform.GetChild(5).GetComponent<Text>().text
                        = "DEF: " + spaceShipList[idx].DEF;
                    playerPanels[i].transform.GetChild(6).GetComponent<Text>().text
                        = "EVO: " + spaceShipList[idx].EVO;
                }
                else IsConfirm(ConfirmType.energyNotEnough); // 如果能源不够,弹出提示框
            });
        });


    }
}
