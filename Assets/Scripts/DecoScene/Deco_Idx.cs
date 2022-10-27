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

    public int Idx { get { return idx; } set { idx = value; } }
    public string Name { get { return furnitName; } set { furnitName = value; } }
    public int Price { get { return price; } set { price = value; } }
    public string Category { get { return category; } set { category = value; } }
}

