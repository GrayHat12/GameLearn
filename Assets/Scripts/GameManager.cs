using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    #region Player Environment Setup
    public static GameManager instance;

    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceneCamera;

    private void Awake()
    {
        if(instance!=null)
        {
            Debug.LogError("More than 1 GameManager in scene");
            return;
        }
        instance = this;
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null)
            return;
        sceneCamera.SetActive(isActive);
    }

    #endregion

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    [SerializeField]
    private NetworkManager networkManager;

    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();
    private static Dictionary<string, BotManager> bots = new Dictionary<string, BotManager>();//Make it BotManager later

    void Start()
    {
        ;
    }

    void Update()
    {
        int _playerCount = players.Count;
        int _roomSize = Convert.ToInt32(networkManager.matchSize);
        if(_roomSize>_playerCount)
        {
            //Bot Code
        }
    }

    public static void RegisterPlayer(string _netID, PlayerManager _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void RegisterBot(string _netID, BotManager _bot)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        bots.Add(_playerID, _bot);
        _bot.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static PlayerManager GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();
    //    foreach(string _playerID in players.Keys)
    //    {
    //        GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
    //    }
    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

    #endregion
}
