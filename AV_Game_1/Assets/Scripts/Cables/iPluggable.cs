using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlugType { power, XLR, DMX, HDMI, SDI, rca, NONE }
public interface iPluggable
{
    GameObject plugObj { get; set; }

    Sprite connectionSprite { get; set; }

    ePlugType TypePlug { get; set; }

    bool IsInput { get; set; }

    bool IsPluggedIn { get; set; }

    bool IsAvailableToPlugIn { get; set; }

    float UnplugCooldownTime { get; set; }

    int Index { get; set; }

    public void SetPlugAvailable(bool _isAvailableToPlugIn);

    public bool CheckIfCorrectConnection(ePlugType _type);

    void PlugConnect();

    void PlugDisconnect();

    void SetConnection(GameObject _connectionToSet, float _powerDrainAmount);

    void SetIndex(int _index);
}
