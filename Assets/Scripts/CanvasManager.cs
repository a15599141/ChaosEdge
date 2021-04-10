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

    public GameObject canvasStation;
    public GameObject canvasShop;
    public GameObject[] playerPanels;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToCanvasStation()
    {
        canvasStation.SetActive(true);
    }

    public void CloseCanvasStation()
    {
        canvasStation.SetActive(false);
    }

    public void OpenCanvasShop()
    {
        canvasShop.SetActive(true);
    }
    public void CloseCanvasShop()
    {
        canvasShop.SetActive(false);
    }

    public void UpdatePlayerPanel()
    {
        int panelIndex = 0;
        foreach (GameObject item in PlayerManager.Instance.playerObjects)
        {
            playerPanels[panelIndex].transform.GetChild(2).GetComponent<Text>().text 
                = "Energy: "+ item.GetComponent<TestedPlayer>().energy.ToString();

            panelIndex++;
        }
    }
}
