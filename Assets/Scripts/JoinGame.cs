using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour
{

    List<GameObject> roomList = new List<GameObject>();

    private NetworkManager networkManager;
    [SerializeField]
    private Text status;
    [SerializeField]
    private GameObject roomListItemPrefab;
    [SerializeField]
    private Transform roomListParent;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0,20,"",false,0,0,OnMatchList);
        status.text = "Loading...";
    }
    public void OnMatchList(bool success,string extendInfo,List<MatchInfoSnapshot> matchList)
    {
        if(!success)
        {
            status.text = "Error Loading Room List : \n" + extendInfo;
            return;
        }
        if (matchList.Count == 0)
        {
            status.text = "No Rooms Found";
            return;
        }
        status.text = "";
        foreach(MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGeo = Instantiate(roomListItemPrefab);
            _roomListItemGeo.transform.SetParent(roomListParent);

            RoomListItem _roomListItem = _roomListItemGeo.GetComponent<RoomListItem>();
            if(_roomListItem != null)
            {
                _roomListItem.Setup(match,JoinRoom);
            }

            roomList.Add(_roomListItemGeo);
        }
    }

    private void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        ClearRoomList();
        //Debug.Log("Joining " + _match.name);
        status.text = "Joining " + _match.name;
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
    }

}
