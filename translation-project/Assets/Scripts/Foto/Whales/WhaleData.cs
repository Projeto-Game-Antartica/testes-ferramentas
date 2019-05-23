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
    public string whale_name;
    public string latitude;
    public string longitude;
    public string borda;
    public string ponta;
    public string entalhe;
    public string manchas;
    public string riscos;
    public string marcas;
    public string pigmentacao;
    public string image_path;
}