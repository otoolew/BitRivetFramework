using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SceneItem {

    /// <summary>
    /// The id - used in persistence
    /// </summary>
    public string id;

    /// <summary>
    /// The human readable level name
    /// </summary>
    public string name;

    /// <summary>
    /// The description of the level - flavour text
    /// </summary>
    public string description;

    /// <summary>
    /// The name of the scene to load
    /// </summary>
    public string sceneName;
}