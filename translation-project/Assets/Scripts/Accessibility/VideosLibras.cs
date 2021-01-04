using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Classes to map JSON file
 */
[Serializable]
public class VideosLibras
{
    public string abs_path;
    public SceneVideos[] scenes;
}

[Serializable]
public class SceneVideos
{
    public string scene_name;


    public VideoData[] videos;
}

[Serializable]
public class VideoData
{
    public string text;
    
    public string rel_path;
}
