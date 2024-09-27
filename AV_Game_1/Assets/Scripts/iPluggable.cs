using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iPluggable
{
    GameObject plugObj { get; set; }

    Sprite connectionSprite { get; set; }

    bool IsInput { get; set; }

    bool IsPluggedIn { get; set; }
    // Start is called before the first frame update
    bool IsAvailableToPlugIn { get; set; }

    public void SetPlugAvailable(GameObject _sourceToConnectObj, bool _isAvailableToPlugIn);

    void PlugConnect();

    void PlugDisconnect();

}
