using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WhaleArray
{
    public WhaleData[] items;
}

[Serializable]
public class WhaleData
{
    public int id_whale;
    public string latitude;
    public string longitude;
    public string image_path;
}