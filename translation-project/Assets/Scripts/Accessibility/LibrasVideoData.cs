using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Classes to map JSON file
 */
[Serializable]
public class LibrasVideoData
{
    public string text;
    public string rel_path;
}

[Serializable]
public class LibrasVideoDataArray
{
    public string abs_path;

    public LibrasVideoData[] videos;
}
