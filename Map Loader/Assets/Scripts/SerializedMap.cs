using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EGameMode
{
    Unknown = -1,
    
    Default = 0,
    Toy = 1,
    Water = 2,
}

[Serializable]
public class SerializedMap
{
    public string mapName;
    public EGameMode gameMode;

    public Dictionary<SerializedPrefab, List<SerializedTransform>> prefabs;

    public SerializedMap()
    {
        prefabs = new Dictionary<SerializedPrefab, List<SerializedTransform>>();
    }
}

[Serializable]
public class SerializedPrefab
{
    public string objPath;
    public string[] materials;
}

[Serializable]
public class SerializedTransform
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale = Vector3.one;
}
