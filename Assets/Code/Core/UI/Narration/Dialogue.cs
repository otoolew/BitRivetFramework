using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Holds all information we need about a single dialogue.
/// Passes into the Dialoge Controller when we want to start a new dialogue.
/// </summary>
[System.Serializable]
public class Dialogue
{
    public string speakerName;
    public Sprite speakerImage;
    [TextArea(1,100)]
    public string[] sentences;
	
}
