using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iRequireHands
{
    // Use this to implement the number of hands needed for an action

    public Sprite HandUseSprite
    {
        get;
        set;
    }

    public int NumberOfHandsNeeded
    {
        get;
        set;
    }

    // Use this to store indexes of hands that are used
    public List<int> HandIndexList
    {
        get;
        set;
    }

    public int ReturnNumberOfHands()
    {
        return NumberOfHandsNeeded;
    }
}
