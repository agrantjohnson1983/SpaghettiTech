using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "SO_BehaviorRuntimeSet", menuName = "ScriptableObjects/RuntimeSet")]
public abstract class so_RuntimeSet<T> : ScriptableObject
{
    private List<T> items = new List<T>();

    public void Initialize()
    {
        items.Clear();
    }

    public T GetItemIndex(int _index)
    {
        return items[_index];
    }

    public void AddToList(T _itemToAdd)
    {
        if(!items.Contains(_itemToAdd))
        {
            items.Add(_itemToAdd);
        }
    }

    public void RemoveFromList(T _itemToRemove)
    {
        if(items.Contains(_itemToRemove))
        {
            items.Remove(_itemToRemove);
        }
    }

    public List<T> ReturnItemList()
    {
        return items;
    }

}
