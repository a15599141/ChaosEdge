using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SWNetwork;

public class LobbyGUI : MonoBehaviour
{
    public GameObject roomRowPrefab;
    public GameObject roomList;

    public GameObject playerRowPrefab;
    public GameObject teamHeaderRowPrefab;
    public GameObject playerList;

    public GameObject messageRowPrefab;
    public GameObject messageList;

    public InputField newRoomText;
    public GameObject newRoomPopup;
    public GameObject CreatingRoomMessagePopup;
    public GameObject StartingRoomMessagePopup;
    public GameObject ChangingPlayerNameMessagePopup;

    public InputField playerNameText;
    public GameObject registerPlayerPopup;
    

    public InputField playerIdText;
    public InputField messagePlayerText;
    public InputField messageRoomText;
    public Button sendRoomMessageButton;
    public GameObject messagePlayerPopup;
    public GameObject sendRoomMessageErrorPopup;

    public GameObject CreateRoomErrorPopup;
    public GameObject LeaveRoomErrorPopup;
    public GameObject StartRoomErrorPopup;
    public GameObject JoinRoomErrorPopup;

    public GameObject BackToMenuPopup;

    /// <summary>
    /// The current message row count.
    /// </summary>
    int currentMessageRowCount = 0;
    int MAX_MESSAGE_ROW_COUNT = 5;

    /// <summary>
    /// Callbak to invoke when NewGamePopup is closed.
    /// </summary>
    Action<bool, string> newGamePopupCloseCallback;

    /// <summary>
    /// Callbak to invoke when RegisterPlayerPopup is closed.
    /// </summary>
    Action<bool, string> registerPlayerPopupCloseCallback;

    /// <summary>
    /// Callback to invoke when MessagePlayerPopup is closed.
    /// </summary>
    Action<bool, string, string> messagePlayerPopupCloseCallback;

    /// <summary>
    /// Add a row to the Player list.
    /// </summary>
    public void AddRowForPlayer(string title, string objectId, TableRow.SelectedHandler callback)
    {
        AddRowToTable(playerList.transform, playerRowPrefab, title, objectId, callback);
    }
    /// <summary>
    /// Add a row to the Team list.
    /// </summary>
    public void AddRowForTeam(string title)
    {
        AddRowToTable(playerList.transform, teamHeaderRowPrefab, title, null, null);
    }
    /// <summary>
    /// Add a row to the Room list.
    /// </summary>
    public void AddRowForRoom(string title, string objectId, TableRow.SelectedHandler callback)
    {
        AddRowToTable(roomList.transform, roomRowPrefab, title, objectId, callback);
    }
    /// <summary>
    /// Add a row to the Message list.
    /// </summary>
    public void AddRowForMessage(string title, string objectId, TableRow.SelectedHandler callback)
    {
        if (currentMessageRowCount == MAX_MESSAGE_ROW_COUNT)
        {
            //remove the first message when MAX_MESSAGE_ROW_COUNT is reached.
            RemoveChild(messageList.transform);
            currentMessageRowCount--;
        }

        currentMessageRowCount++;
        AddRowToTable(messageList.transform, messageRowPrefab, title, objectId, callback);
        messageRoomText.text = "";
    }
    void AddRowToTable(Transform table, GameObject rowPrefab, string title, string objectId, TableRow.SelectedHandler callback)
    {
        GameObject newRow = Instantiate(rowPrefab, table);
        TableRow tableRow = newRow.GetComponent<TableRow>();
        tableRow.OnSelected += callback;
        tableRow.SetTitle(title);
        tableRow.SetObjectId(objectId);
    }

    // Remove all the rows in the Player list.
    public void ClearPlayerList()
    {
        RemoveAllChildren(playerList.transform);
    }
    // Remove all the rows in the Room list.
    public void ClearRoomList()
    {
        RemoveAllChildren(roomList.transform);
    }
    // Remove all the messages in room chat.
    public void ClearRoomMessage()
    {
        Destroy(messageList.gameObject);
        currentMessageRowCount = 0;
    }

    public void ShowBackToMenuPopup()
    {
        BackToMenuPopup.SetActive(true);
    }
    public void ShowNewRoomPopup(Action<bool, string> callback)
    {
        newRoomPopup.SetActive(true);
        newRoomText.text = "";
        newGamePopupCloseCallback = callback;
    }
    public void ShowRegisterPlayerPopup(Action<bool, string> callback)
    {
        registerPlayerPopup.SetActive(true);
        playerNameText.text = "";
        registerPlayerPopupCloseCallback = callback;
    }
    public void ShowMessagePlayerPopup(string targetPlayer, Action<bool, string, string> callback)
    {
        messagePlayerPopup.SetActive(true);
        playerIdText.text = targetPlayer;
        messagePlayerText.text = "";
        messagePlayerPopupCloseCallback = callback;
    }
    public void ShowCreateRoomErrorPopup()
    {
        CreateRoomErrorPopup.SetActive(true);
    }
    public void ShowJoinRoomErrorPopup()
    {
        JoinRoomErrorPopup.SetActive(true);
    }
    public void ShowLeaveRoomErrorPopup()
    {
        LeaveRoomErrorPopup.SetActive(true);
    }
    public void ShowStartRoomErrorPopup()
    {
        StartRoomErrorPopup.SetActive(true);
    }
    public void ShowSendRoomMessageErrorPopup()
    {
        sendRoomMessageErrorPopup.SetActive(true);
    }

    // Helper methods
    void RemoveAllChildren(Transform parent)
    {
        foreach (Transform childTransform in parent)
        {
            Destroy(childTransform.gameObject);
        }
    }
    void RemoveChild(Transform parent)
    {
        foreach (Transform childTransform in parent)
        {
            Destroy(childTransform.gameObject);
            return;
        }
    }

    public void HandleBackToMenuOk()
    {
        NetworkClient.Lobby.LeaveRoom((successful, error) =>
        {
            if (successful)
            {
                Debug.Log("Left room");
                ClearPlayerList();
                ClearRoomMessage();
            }
            else
            {
                Debug.Log("Failed to leave room " + error);
            }
        });
        SceneManager.LoadScene("HomeScene");
    }
    public void HandleBackToMenuCancel()
    {
        BackToMenuPopup.SetActive(false);
    }

    public void HandleCreateRoomOK()
    {
        if (newRoomText.text.Length > 0)
        {
            CreatingRoomMessagePopup.SetActive(true);
            newRoomPopup.SetActive(false);
            if (newGamePopupCloseCallback != null)
            {
                newGamePopupCloseCallback(true, newRoomText.text);
            }
        }
        else
        {
            // must enter a room name
            newRoomText.Select();
            Debug.LogWarning("Game name is empty.");
        }

    }
    public void HandleCreateRoomCancel()
    {
        newRoomPopup.SetActive(false);
        if (newGamePopupCloseCallback != null)
        {
            newGamePopupCloseCallback(false, null);
        }
    }

    public void HandleRegisterPlayerOK()
    {
        if (playerNameText.text.Length > 0)
        {
            registerPlayerPopup.SetActive(false);
            if (registerPlayerPopupCloseCallback != null)
            {
                // left current joined room once player change  its name in lobby
                NetworkClient.Lobby.LeaveRoom((successful, error) =>
                {
                    if (successful)
                    {
                        Debug.Log("Left room");
                        ClearPlayerList();
                        ClearRoomMessage();
                    }
                    else
                    {
                        Debug.Log("Failed to leave room " + error);
                    }
                });
                registerPlayerPopupCloseCallback(true, playerNameText.text);
            }
        }
        else
        {
            // must enter a player name
            playerNameText.Select();
            Debug.LogWarning("Player name is empty.");
        }
    }
    public void HandleRegisterPlayerCancel()
    {
        registerPlayerPopup.SetActive(false);
        if (registerPlayerPopupCloseCallback != null)
        {
            registerPlayerPopupCloseCallback(false, null);
        }
    }

    public void HandleMessagePlayerOK()
    {
        if (playerIdText.text.Length > 0)
        {
            messagePlayerPopup.SetActive(false);
            if (messagePlayerPopupCloseCallback != null)
            {
                messagePlayerPopupCloseCallback(true, playerIdText.text, messagePlayerText.text);
            }
        }
        else
        {
            Debug.LogWarning("Player id is empty.");
        }
    }
    public void HandleMessagePlayerCancel()
    {
        messagePlayerPopup.SetActive(false);
        if (messagePlayerPopupCloseCallback != null)
        {
            messagePlayerPopupCloseCallback(false, null, null);
        }
    }

    public void HandleCreateRoomErrorOk()
    {
        CreateRoomErrorPopup.SetActive(false);
    }
    public void HandleJoinRoomErrorOk()
    {
        JoinRoomErrorPopup.SetActive(false);
    }
    public void HandleLeaveRoomErrorOk()
    {
        LeaveRoomErrorPopup.SetActive(false);
    }
    public void HandleStartRoomErrorOk()
    {
        StartRoomErrorPopup.SetActive(false);
    }
    public void HandleSendRoomMessageErrorOk()
    {
        sendRoomMessageErrorPopup.SetActive(false);
    }


}
