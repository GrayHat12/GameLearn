using UnityEngine;

public class Util
{
    
    public static void SetLayerRecursively(GameObject _object, int _newLayer)
    {
        if (_object == null)
            return;
        _object.layer = _newLayer;
        foreach(Transform _child in _object.transform)
        {
            if (_child == null)
                continue;
            SetLayerRecursively(_child.gameObject, _newLayer);
        }
    }

}
