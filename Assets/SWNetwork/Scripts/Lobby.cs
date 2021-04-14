using System.Collections.Generic;
using UnityEngine;
using System;
using SWNetwork;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Lobby : MonoBehaviour
{
    public LobbyGUI GUI;
    public Canvas canvas; // lobby entry

    Dictionary<string, string> playersDict_; // Used to display players in different teams.
    RoomCustomData roomData_; // Current room's custom data.

    int currentRoomPageIndex_ = 0;// Current page index of the room list. 
    int MAX_PLAYER_NUM = 4;

    public Text LobbyPing;
    public InputField customPlayerIdField; // InputField for entering custom playerId
    string playerName_; // Player entered name
    public Text playerNameText; // player name display in lobby
    public TMP_Text LobbyEntryTitle; // Game title in lobby entry
    public Button EntryRegisterButton;// Button for checking into SocketWeaver services
    public Button EntryBackHomeButton;// Button for checking into SocketWeaver services
    public Text EntryRegisterText;// text of Register Button
    public Button EnterLobbyButton;// Button for joining or creating room
    public Dropdown gameRegionDrowDown;// Dropdown for selecting the game region
    void Start()
    {
        // Subscribe to Lobby events
        NetworkClient.Lobby.OnNewPlayerJoinRoomEvent += Lobby_OnNewPlayerJoinRoomEvent;
        NetworkClient.Lobby.OnRoomReadyEvent += Lobby_OnRoomReadyEvent;
        NetworkClient.Lobby.OnFailedToStartRoomEvent += Lobby_OnFailedToStartRoomEvent;
        NetworkClient.Lobby.OnPlayerLeaveRoomEvent += Lobby_OnPlayerLeaveRoomEvent;
        NetworkClient.Lobby.OnRoomCustomDataChangeEvent += Lobby_OnRoomCustomDataChangeEvent;
        NetworkClient.Lobby.OnRoomMessageEvent += Lobby_OnRoomMessageEvent;
        NetworkClient.Lobby.OnPlayerMessageEvent += Lobby_OnPlayerMessageEvent;
        NetworkClient.Lobby.OnLobbyConnectedEvent += Lobby_OnLobbyConncetedEvent;

        // allow player to register in Lobby Entry
        canvas.GetComponent<CanvasGroup>().alpha = 0;

    }
    void Update()
    {
        LobbyPing.text = "ping: " + NetworkClient.Instance.LobbyPing + "ms";
        if (Input.GetKeyDown(KeyCode.KeypadEnter)||Input.GetKeyDown(KeyCode.Return))
        {
            SendRoomMessage();
        }
    }
    void OnDestroy()
    {
        // Unsubscrible to Lobby events
        NetworkClient.Lobby.OnNewPlayerJoinRoomEvent -= Lobby_OnNewPlayerJoinRoomEvent;
        NetworkClient.Lobby.OnRoomReadyEvent -= Lobby_OnRoomReadyEvent;
        NetworkClient.Lobby.OnFailedToStartRoomEvent -= Lobby_OnFailedToStartRoomEvent;
        NetworkClient.Lobby.OnPlayerLeaveRoomEvent -= Lobby_OnPlayerLeaveRoomEvent;
        NetworkClient.Lobby.OnRoomCustomDataChangeEvent -= Lobby_OnRoomCustomDataChangeEvent;
        NetworkClient.Lobby.OnRoomMessageEvent -= Lobby_OnRoomMessageEvent;
        NetworkClient.Lobby.OnPlayerMessageEvent -= Lobby_OnPlayerMessageEvent;
        NetworkClient.Lobby.OnLobbyConnectedEvent -= Lobby_OnLobbyConncetedEvent;
    }

    // Lobby entry
    public void BackToHomeMenu()
    {
        SceneManager.LoadScene("HomeScene");
    }
    public void RegisterInLobbyEntry()
    {
        playerName_ = customPlayerIdField.text;
        EntryRegisterText.text = "Registering...";
        EntryRegisterButton.interactable = false;
        if (playerName_.Length > 0)
        {
            // use the user entered playerId to check into SocketWeaver. Make sure the PlayerId is unique.
            NetworkClient.Instance.CheckIn(playerName_, (bool ok, string error) =>
            {
                if (ok)
                {
                    playerNameText.text = playerName_;
                    UpdateGameRegionDropdownOptions();
                    // display region selection
                    EnterLobbyButton.gameObject.SetActive(true);
                    gameRegionDrowDown.gameObject.SetActive(true);
                    customPlayerIdField.gameObject.SetActive(false);
                    EntryRegisterButton.gameObject.SetActive(false);
                    EntryBackHomeButton.gameObject.SetActive(false);
                }
                else
                {
                    EntryRegisterText.text = "Register";
                    EntryRegisterButton.interactable = true;
                    Debug.LogError("Check-in failed: " + error);
                }
                
            });
        }
        else
        {
            EntryRegisterText.text = "Register";
            EntryRegisterButton.interactable = true;
            customPlayerIdField.Select();
        }
        
    }
    void UpdateGameRegionDropdownOptions()
    {
        if (NetworkClient.Instance == null)
        {
            return;
        }

        gameRegionDrowDown.ClearOptions();
        int currentValue = 0;

        for (int i = 0; i < NetworkClient.Instance.AvailableNodeRegions.Length; i++)
        {
            NodeRegion nodeRegion = NetworkClient.Instance.AvailableNodeRegions[i];
            if (nodeRegion.name.Equals(NetworkClient.Instance.NodeRegion))
            {
                currentValue = i;
            }
            gameRegionDrowDown.options.Add(new Dropdown.OptionData(nodeRegion.description));
        }

        gameRegionDrowDown.value = currentValue;
        gameRegionDrowDown.onValueChanged.AddListener(GameRegionChanged);
    }
    public void GameRegionChanged(int value)
    {
        NodeRegion nodeRegion = NetworkClient.Instance.AvailableNodeRegions[value];
        NetworkClient.Instance.NodeRegion = nodeRegion.name;
    }
    public void EnterLobby()
    {
        EnterLobbyButton.gameObject.SetActive(false);
        gameRegionDrowDown.gameObject.SetActive(false);
        LobbyEntryTitle.gameObject.SetActive(false);
        canvas.GetComponent<CanvasGroup>().alpha = 1;
    }

    // In lobby
    public void RegisterInLobby()
    {
        GUI.ShowRegisterPlayerPopup((bool ok, string playerName) =>
        {
            if (ok)
            {
                // store the playerName
                // playerName also used to register local player to the lobby server
                playerName_ = playerName;
                GUI.ChangingPlayerNameMessagePopup.SetActive(true);
                NetworkClient.Instance.CheckIn(playerName, (bool successful, string error) =>
                {
                    if (!successful)
                    {
                        Debug.LogError(error);
                    }
                    else
                    {
                        GUI.ChangingPlayerNameMessagePopup.SetActive(false);
                        playerNameText.text = playerName_;
                    }
                });
            }
        });
    }
    public void OnRoomSelected(string roomId)
    {
        Debug.Log("OnRoomSelected: " + roomId);
        // Join the selected room
        
            NetworkClient.Lobby.JoinRoom(roomId, (successful, reply, error) =>
            {
                if (successful)
                {
                    Debug.Log("Joined room " + reply);
                    // refresh the player list
                    GetPlayersInCurrentRoom();
                }
                else
                {
                    GUI.ShowJoinRoomErrorPopup();
                    Debug.Log("Failed to Join room " + error);
                }
            });

    }
    public void GetPlayersInCurrentRoom()
    {
        NetworkClient.Lobby.GetPlayersInRoom((successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Got players " + reply);

                // store the playerIds and player names in a dictionary.
                // The dictionary is later used to populate the player list.
                playersDict_ = new Dictionary<string, string>();
                foreach (SWPlayer player in reply.players)
                {
                    playersDict_[player.id] = player.GetCustomDataString();
                }

                // fetch the room custom data.
                GetRoomCustomData();
            }
            else
            {
                Debug.Log("Failed to get players " + error);
            }
        });
    }
    public void GetRoomCustomData()
    {
        NetworkClient.Lobby.GetRoomCustomData((successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Got room custom data " + reply);
                // Deserialize the room custom data.
                roomData_ = reply.GetCustomData<RoomCustomData>();
                if (roomData_ != null)
                {
                    RefreshPlayerList();
                }
            }
            else
            {
                Debug.Log("Failed to get room custom data " + error);
            }
        });
    }
    public void CreateNewRoom()
    {
        GUI.ShowNewRoomPopup((bool ok, string roomName) =>
        {
            if (ok)
            {
                roomData_ = new RoomCustomData();
                roomData_.name = roomName;
                roomData_.team1 = new TeamCustomData();
                roomData_.team2 = new TeamCustomData();
                roomData_.team3 = new TeamCustomData();
                roomData_.team4 = new TeamCustomData();
                roomData_.team1.players.Add(NetworkClient.Lobby.PlayerId);
                // use the serializable roomData_ object as room's custom data.
                NetworkClient.Lobby.CreateRoom(roomData_, true, MAX_PLAYER_NUM, (successful, reply, error) =>
                {
                    if (successful)
                    {
                        Debug.Log("Room created " + reply);
                        // refresh the room list
                        GetRooms();
                        // refresh the player list
                        GetPlayersInCurrentRoom();
                        GUI.CreatingRoomMessagePopup.SetActive(false);
                    }
                    else
                    {
                        GUI.ShowCreateRoomErrorPopup();
                        Debug.Log("Failed to create room " + error);
                    }
                });
            }
        });
    }
    public void SendRoomMessage()
    {
        string message = GUI.messageRoomText.text;
        NetworkClient.Lobby.MessageRoom(message, (bool successful, SWLobbyError error) =>
        {
            if (successful)
            {
                if(message.Length > 0)
                {
                    Debug.Log("Sent room message");
                    string msg = "Sent to room: " + message;
                    GUI.AddRowForMessage(msg, null, null);
                }
                else GUI.messageRoomText.Select();
            }
            else
            {
                GUI.ShowSendRoomMessageErrorPopup();
                Debug.Log("Failed to send room message " + error);
            }
        });
    }
    public void OnPlayerSelected(string playerId)
    {
        Debug.Log("OnPlayerSelected: " + playerId);

        // demonstrate player message API
        GUI.ShowMessagePlayerPopup(playerId, (bool ok, string targetPlayerId, string message) =>
        {
            if (ok)
            {
                Debug.Log("Send player message " + "playerId= " + " message= " + message);
                NetworkClient.Lobby.MessagePlayer(playerId, message, (bool successful, SWLobbyError error) =>
                {
                    if (successful)
                    {
                        Debug.Log("Sent player message");
                        string msg = "Sent to " + ": " + message;
                        GUI.AddRowForMessage(msg, null, null);
                    }
                    else
                    {
                        Debug.Log("Failed to send player messagem " + error);
                    }
                });
            }
        });
    }
    void RefreshPlayerList()
    {
        // Use the room custom data, and the playerId and player Name dictionary to populate the player lsit
        if (playersDict_ != null)
        {
            GUI.ClearPlayerList();
            GUI.AddRowForTeam("Player 1");
            foreach (string pId in roomData_.team1.players)
            {
                String playerName = playersDict_[pId];
                GUI.AddRowForPlayer(playerName, pId, OnPlayerSelected);
            }

            GUI.AddRowForTeam("Player 2");
            foreach (string pId in roomData_.team2.players)
            {
                String playerName = playersDict_[pId];
                GUI.AddRowForPlayer(playerName, pId, OnPlayerSelected);
            }

            GUI.AddRowForTeam("Player 3");
            foreach (string pId in roomData_.team3.players)
            {
                String playerName = playersDict_[pId];
                GUI.AddRowForPlayer(playerName, pId, OnPlayerSelected);
            }

            GUI.AddRowForTeam("Player 4");
            foreach (string pId in roomData_.team4.players)
            {
                String playerName = playersDict_[pId];
                GUI.AddRowForPlayer(playerName, pId, OnPlayerSelected);
            }
        }
    }
    public void GetRooms()
    {
        // Get the rooms for the current page.
        NetworkClient.Lobby.GetRooms(currentRoomPageIndex_, 6, (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Got rooms " + reply);

                // Remove rooms in the rooms list
                GUI.ClearRoomList();

                foreach (SWRoom room in reply.rooms)
                {
                    Debug.Log(room);
                    // Deserialize the room custom data.
                    RoomCustomData rData = room.GetCustomData<RoomCustomData>();
                    if (rData != null)
                    {
                        // Add rooms to the rooms list.
                        GUI.AddRowForRoom(rData.name, room.id, OnRoomSelected);
                    }
                }
            }
            else
            {
                Debug.Log("Failed to get rooms " + error);
            }
        });
    }
    public void NextPage()
    {
        currentRoomPageIndex_++;
        GetRooms();
    }
    public void PreviousPage()
    {
        currentRoomPageIndex_--;
        GetRooms();
    }
    public void LeaveRoom()
    {
        NetworkClient.Lobby.LeaveRoom((successful, error) =>
        {
            if (successful)
            {
                Debug.Log("Left room");
                GUI.ClearPlayerList();
                GUI.ClearRoomMessage();
                GetRooms();
            }
            else
            {
                GUI.ShowLeaveRoomErrorPopup();
                Debug.Log("Failed to leave room " + error);
            }
        });
    }
    public void StartRoom()
    {
        GUI.StartingRoomMessagePopup.SetActive(true);
        if (NetworkClient.Lobby.IsOwner)
        {
            NetworkClient.Lobby.StartRoom((okay, error) =>
            {
                if (okay)
                {
                    // Lobby server has sent request to SocketWeaver. The request is being processed.
                    // If socketweaver finds suitable server, Lobby server will invoke the 'OnRoomReadyEvent'.
                    // If socketweaver cannot find suitable server, Lobby server will invoke the OnFailedToStartRoomEvent.
                    Debug.Log("Started room");
                }
                else
                {
                    GUI.StartingRoomMessagePopup.SetActive(false);
                    Debug.Log("Failed to start room " + error);
                }
            });
        }
        else
        {
            GUI.StartingRoomMessagePopup.SetActive(false);
            GUI.ShowStartRoomErrorPopup();
            Debug.Log("Failed to connect to room");
        }
    }
    void ConnectToRoom()
    {
        NetworkClient.Instance.ConnectToRoom(HandleConnectedToRoom);
    }
    void HandleConnectedToRoom(bool connected)
    {
        if (connected)
        {
            Debug.Log("Connected to room");
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            Debug.Log("Failed to connect to room");
        }
    }

    // lobby delegate events
    void Lobby_OnLobbyConncetedEvent() // called after check in 
    {
        Debug.Log("Lobby_OnLobbyConncetedEvent");
        // Register the player using the entered player name.
        NetworkClient.Lobby.Register(playerName_, (successful, reply, error) =>
        {
            if (successful)
            {
                Debug.Log("Lobby registered " + reply);
                GetRooms();
            }
            else
            {
                Debug.Log("Lobby failed to register " + error);
            }
        });
    } 
    void Lobby_OnNewPlayerJoinRoomEvent(SWJoinRoomEventData eventData)
    {
        Debug.Log("Player joined room");
        Debug.Log(eventData);
        // Store the new playerId and player name pair
        playersDict_[eventData.newPlayerId] = eventData.GetString();
        if (NetworkClient.Lobby.IsOwner)
        {
            // Find the team has space and assign the new player to it.
            if (roomData_.team2.players.Count < roomData_.team1.players.Count)
            {
                roomData_.team2.players.Add(eventData.newPlayerId);
            }
            else if (roomData_.team3.players.Count < roomData_.team2.players.Count)
            {
                roomData_.team3.players.Add(eventData.newPlayerId);
            }
            else if (roomData_.team4.players.Count < roomData_.team3.players.Count)
            {
                roomData_.team4.players.Add(eventData.newPlayerId);
            }
            else if (roomData_.team1.players.Count < roomData_.team4.players.Count)
            {
                roomData_.team1.players.Add(eventData.newPlayerId);
            }
            // Update the room custom data
            NetworkClient.Lobby.ChangeRoomCustomData(roomData_, (bool successful, SWLobbyError error) =>
            {
                if (successful)
                {
                    Debug.Log("ChangeRoomCustomData successful");
                    RefreshPlayerList();
                }
                else
                {
                    Debug.Log("ChangeRoomCustomData failed: " + error);
                }
            });
        }
    }
    void Lobby_OnPlayerLeaveRoomEvent(SWLeaveRoomEventData eventData)
    {
        Debug.Log("Player left room: " + eventData);

        if (NetworkClient.Lobby.IsOwner)
        {
            // Remove the players from both team.
            roomData_.team1.players.RemoveAll(eventData.leavePlayerIds.Contains);
            roomData_.team2.players.RemoveAll(eventData.leavePlayerIds.Contains);
            roomData_.team3.players.RemoveAll(eventData.leavePlayerIds.Contains);
            roomData_.team4.players.RemoveAll(eventData.leavePlayerIds.Contains);
            // Update the room custom data
            NetworkClient.Lobby.ChangeRoomCustomData(roomData_, (bool successful, SWLobbyError error) =>
            {
                if (successful)
                {
                    Debug.Log("ChangeRoomCustomData successful");
                    RefreshPlayerList();
                }
                else
                {
                    Debug.Log("ChangeRoomCustomData failed: " + error);
                }
            });
        }
    }
    void Lobby_OnRoomCustomDataChangeEvent(SWRoomCustomDataChangeEventData eventData)
    {
        Debug.Log("Room custom data changed: " + eventData);

        SWRoom room = NetworkClient.Lobby.RoomData;
        roomData_ = room.GetCustomData<RoomCustomData>();

        // Room custom data changed, refresh the player list.
        RefreshPlayerList();
    }
    void Lobby_OnRoomMessageEvent(SWMessageRoomEventData eventData)
    {
        string msg = "Room message: " + eventData.data;
        GUI.AddRowForMessage(msg, null, null);
    }
    void Lobby_OnPlayerMessageEvent(SWMessagePlayerEventData eventData)
    {
        string msg = eventData.playerId + ": " + eventData.data;
        GUI.AddRowForMessage(msg, null, null);
    }
    void Lobby_OnRoomReadyEvent(SWRoomReadyEventData eventData)
    {
        Debug.Log("Room is ready: roomId= " + eventData.roomId);
        // Room is ready to join and its game servers have been assigned.
        ConnectToRoom();
    }
    void Lobby_OnFailedToStartRoomEvent(SWFailedToStartRoomEventData eventData)
    {
        Debug.Log("Failed to start room: " + eventData);
    }
}
