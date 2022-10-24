using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deco_Idx : MonoBehaviour
{
    [SerializeField]
    int idx;
    [SerializeField]
    string furnitName;
    [SerializeField]
    int price;
    [SerializeField]
    string category;

    public int Idx { get { return idx; } }
    public string Name { get { return furnitName; } }
    public int Price { get { return price; } }
    public string Category { get { return category; } }
}

