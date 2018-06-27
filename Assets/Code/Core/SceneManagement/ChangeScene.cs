// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    13 JUNE 2018
// ----------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyNamespace
{
	public class ChangeScene : MonoBehaviour 
	{
        public string SceneName;

        public void LoadScene()
        {
            SceneManager.LoadScene(SceneName);
        }
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}