using System;
using System.Collections.Generic;
using UnityEngine;

// Attach a list of special drops metadata to an object
public class SpecialEnemyDrops : MonoBehaviour
{
    public List<SpecialDrop> specialDrops = new();
}


// Specialised class for special drops

[Serializable]
public class SpecialDrop
{
    public Transform obj;
    public float chance;
}