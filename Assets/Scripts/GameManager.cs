using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SWNetwork;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Slider BGMSlider, SoundEffectSlider;
    //public AudioSource HomeBGM;
    public Button AIDisableButton;
    public Button TestingPanelDisableButton;


    // Start is called before the first frame update
    void Start()
    {
        AIDisableButton.interactable = false;
        TestingPanelDisableButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        SoundControl();

    }

    public void SoundControl()
    {
        //HomeBGM.volume = BGMSlider.value;
        CanvasManager.Instance.BGM.volume = BGMSlider.value;
        CanvasManager.Instance.battleBGM.volume = BGMSlider.value;

        CanvasManager.Instance.attackSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.defendSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.evadeSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.getDamageSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.repairSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.errorSound.volume = SoundEffectSlider.value;
        CanvasManager.Instance.updateSound.volume = SoundEffectSlider.value;
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
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
    public void OnSpawnerReady(bool finishedSceneSetup, SceneSpawner sceneSpawner)
    {
        //Debug.Log("OnSpawnerReady " + finishedSceneSetup);
        if (!finishedSceneSetup)
        {
            int spawnPointIndex = Random.Range(0, 4);
            int playerPrefabIndex = Random.Range(0, 4);

            //sceneSpawner.SpawnForPlayer(spawnPointIndex, playerPrefabIndex);
            //sceneSpawner.SpawnForNonPlayer(0, 0);
      
            sceneSpawner.SpawnForPlayer(0, spawnPointIndex);
          
            sceneSpawner.PlayerFinishedSceneSetup();
            Debug.Log("OnSpawnerReady " + finishedSceneSetup);
        }
    }
    public void OnHostSpawnerReady(bool alreadySetup, SceneSpawner sceneSpawner) 
    {
        Debug.Log("OnHostSpawnerReady " + alreadySetup);
        if (!alreadySetup)
        {
            sceneSpawner.HostFinishedSceneSetup();
        }
    }
}