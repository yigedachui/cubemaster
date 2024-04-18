using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T>
{
    public List<T> poolList;

    public Pool()
    {
        poolList = new List<T>();
    }

    public void InPool(T t)
    {
        if (!poolList.Contains(t))
        {
            poolList.Add(t);
        }
    }

}
