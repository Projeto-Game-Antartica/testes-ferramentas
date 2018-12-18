using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataArray {
    public Data[] items;
}

[Serializable]
public class Data
{
    public string key_ptbr;
    public string key_en;
    public string description_ptbr;
}
