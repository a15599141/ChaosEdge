using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SWNetwork;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /* --------- 联机向 ----------- */
    // 退出游戏，返回主界面
    public void Exit() 
    {
        SceneManager.LoadScene("HomeScene");
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
    // 游戏场景初始化
    public void OnHostSpawnerReady(bool alreadySetup, SceneSpawner sceneSpawner) 
    {
        Debug.Log("OnHostSpawnerReady " + alreadySetup);
        if (!alreadySetup)
        {
            sceneSpawner.SpawnForPlayer(0, 0); //spawn for player 1
            sceneSpawner.SpawnForPlayer(1, 1); //spawn for player 2
            sceneSpawner.SpawnForPlayer(2, 2); //spawn for player 3
            sceneSpawner.SpawnForPlayer(3, 3); //spawn for player 4
            sceneSpawner.HostFinishedSceneSetup();
        }
    }
}