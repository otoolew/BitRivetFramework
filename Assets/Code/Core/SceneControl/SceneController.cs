// ----------------------------------------------------------------------------
// Author:  William O'Toole
// Project: BitRivet Framework
// Date:    20 JUNE 2018
// ----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class SceneController : MonoBehaviour 
{
    public SceneItem[] Scenes;
    public CanvasGroup screenFadeCanvas;
    public float fadeDuration = 1f;

    [SerializeField]
    private string currentScene;
    public string CurrentScene
    {
        get { return currentScene; }
        private set { currentScene = value; }
    }

    private bool isFading;

    public Events.EventFadeComplete OnSceneChangeStart;
    public Events.EventFadeComplete OnSceneChangeComplete;


    public void FadeAndLoadScene(string sceneName)
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }
    }
    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));
        yield return SceneManager.LoadSceneAsync(sceneName);
        OnSceneChangeStart.Invoke(true);
        yield return StartCoroutine(Fade(0f));
        OnSceneChangeComplete.Invoke(true);
        currentScene = sceneName;
    }
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }
    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;
        screenFadeCanvas.blocksRaycasts = true;
        float fadeSpeed = Mathf.Abs(screenFadeCanvas.alpha - finalAlpha) / fadeDuration;
        while (!Mathf.Approximately(screenFadeCanvas.alpha, finalAlpha))
        {
            screenFadeCanvas.alpha = Mathf.MoveTowards(screenFadeCanvas.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
        isFading = false;
        screenFadeCanvas.blocksRaycasts = false;
    }
}