using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SWNetwork;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject dice;     //骰子对象
    private Rigidbody diceRb;   //骰子刚体

    public Button rollButton;   //掷骰子按钮
    public TMP_Text roundText;  //游戏轮数显示器

    public bool moveAllowed;    //玩家可移动
    public int diceNumber; //骰子点数
    private int roundCount;     //游戏轮数计数器
    private int rotateLimit = 30;   //骰子转动速度

    // Start is called before the first frame update
    void Start()
    {
        //if(NetworkClient.)
        diceRb = dice.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RollDiceOnClick()
    {
        roundCount++; //回合数加1
        roundText.text = "Round: " + roundCount.ToString();//更新回合数
        rollButton.interactable = false; // 禁用摇色子按钮
        StartCoroutine("RollDice");//启动骰子协程
    }

    //协程控制骰子转动
    private IEnumerator RollDice()
    {
        //转动骰子
        for (int i = 0; i < rotateLimit; i++)
        {
            dice.transform.Rotate(new Vector3(dice.transform.rotation.x + RandomRotation(), dice.transform.rotation.y + RandomRotation(), dice.transform.rotation.z + RandomRotation()));
            yield return new WaitForSeconds(0.02f);
        }

        //每0.05秒验证一次骰子是否依旧在转动
        while (true)
        {
           
            if (diceRb.velocity.Equals(new Vector3(0.0f, 0.0f, 0.0f)))
            {
                //Debug.Log("stop rolling");
                moveAllowed = true;
                diceNumber = dice.GetComponent<Dice>().GetNum();
                yield break;//结束协程
            }
            else
            {
                //Debug.Log("still rolling");
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    private int RandomRotation()
    {
        return Random.Range(rotateLimit, -rotateLimit);
    }

    // 联机向
    public void Exit()
    {
        SceneManager.LoadScene("HomeScene");
        NetworkClient.Instance.DisconnectFromRoom();
        NetworkClient.Lobby.LeaveRoom(HandleLeaveRoom);
    }
    void HandleLeaveRoom(bool okay, SWLobbyError error)
    {
        if (!okay)
        {
            Debug.LogError(error);
        }
        Debug.Log("Left room");
        SceneManager.LoadScene("HomeScene");
    }
    public void OnHostSpawnerReady(bool alreadySetup, SceneSpawner sceneSpawner)
    {
        Debug.Log("OnHostSpawnerReady " + alreadySetup);
        if (!alreadySetup)
        {
            sceneSpawner.SpawnForPlayer(0, 0); //spawn for player 1
            sceneSpawner.SpawnForPlayer(1, 1); //spawn for player 2
            sceneSpawner.SpawnForPlayer(2, 2); //spawn for player 3
            sceneSpawner.SpawnForPlayer(3, 3); //spawn for player 4
            sceneSpawner.SpawnForNonPlayer(0, 4); // spawn for the dice, its index in nonPlayerPrefab is 0, its spawnPointIndex is 4
            sceneSpawner.HostFinishedSceneSetup();
        }
    }
    public void OnSpawnerReady(bool alreadySetup, SceneSpawner sceneSpawner)
    {
        Debug.Log("OnSpawnerReady " + alreadySetup);

        // Check alreadySetup to see if the scene has been set up before. 
        // If it is true, it means the player disconnected and reconnected to the game. 
        // In this case, we should not spawn a new Player GameObject for the player.
        if (!alreadySetup)
        {
            // If alreadySetup is false, it means the player just started the game. 
            // We randomly select a SpawnPoint and ask the SceneSpawner to spawn a Player GameObject. 
            // we have 1 playerPrefabs so playerPrefabIndex is 0.
            // We have 4 spawnPoints so we generated a random int between 0 to 3.

            //int playerPrefabIndex = Random.Range(0, 3);
            //int spawnPointIndex = Random.Range(0, 3);
            //sceneSpawner.SpawnForPlayer(playerPrefabIndex, spawnPointIndex);

            // Tell the spawner that we have finished setting up the scene. 
            // alreadySetup will be true when SceneSpawn becomes ready next time.
            //sceneSpawner.PlayerFinishedSceneSetup();
        }
    }

}