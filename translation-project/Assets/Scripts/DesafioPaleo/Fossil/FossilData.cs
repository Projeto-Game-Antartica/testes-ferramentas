using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FossilArray
{
    public FossilData[] items;
}

[Serializable]
public class FossilData
{
    public int id_fossil;
    public string caracteristica;
    public string classificacao;
    public string era;
    public string description;
    public string image_path;

    public bool identified;
}