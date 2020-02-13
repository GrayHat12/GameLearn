using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(BotManager))]
public class BotSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDraw";
    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;



    void Start()
    {
        
        DisableComponents();
        AssignRemoteLayer();
        if(isLocalPlayer)
        {

            //SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("No Player UI Component on Player UI Prefab");
                return;
            }
            ui.SetController(GetComponent<PlayerController>());
            //GetComponent<BotManager>().SetupPlayer();
        }

    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        BotManager _bot = GetComponent<BotManager>();
        GameManager.RegisterBot(_netID, _bot);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void OnDisable()
    {
        Destroy(playerUIInstance);
        if (isLocalPlayer)
        {
            //GameManager.instance.SetSceneCameraActive(true);
            //GameManager.UnRegisterBot(transform.name);
        }
    }

}
