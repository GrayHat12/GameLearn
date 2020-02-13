using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private const string WEAPON_LAYER_NAME = "Weapon";

    [SerializeField]
    private Transform weaponHolder;

    private PlayerWeapon currentWeapon;

    private WeaponGraphics currentGraphics;

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;
        GameObject _weaponIns = Instantiate(_weapon.graphics,weaponHolder.position,weaponHolder.rotation);
        _weaponIns.transform.SetParent(weaponHolder);

        currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
        if(currentGraphics==null)
        {
            Debug.LogError("No weapon Graphics Component on " + _weaponIns.name);
        }


        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(_weaponIns,LayerMask.NameToLayer(WEAPON_LAYER_NAME));
        }
    }

}
