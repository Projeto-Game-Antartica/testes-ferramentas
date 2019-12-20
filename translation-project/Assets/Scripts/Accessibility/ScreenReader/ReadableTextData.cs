using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Classes to map JSON file
 */
[Serializable]
public class TextData
{
    public string key;
    public string readabletext_ptbr;
    public string readabletext_en;
}

[Serializable]
public class TextDataArray
{
    public TextData[] items;
}
