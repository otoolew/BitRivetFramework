using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyNamespace
{
    [CreateAssetMenu(fileName = "NewDialogueLine", menuName = "Dialogue/Dialogue Line")]
	public class DialogueLine : ScriptableObject 
	{
        public Sprite portraitImage;
        public List<string> dialogueLines;
		// Use this for initialization
		void Start () {
		    	
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
