using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class SceneChangeManager
{
    public enum SceneName
    {
        Intro,
        Main,
        Rank,
        Shop,
        Result,
        Play,
    }

    public static SceneName currentScene { get; private set; }
    private static Dictionary<SceneName, string> scene = Init();
    private static event Action endEvent;

    private static Dictionary<SceneName, string> Init()
    {
        SceneManager.sceneLoaded += EndLoad;
        return new(capacity: Enum.GetValues(typeof(SceneName)).Length);
    }
    private static void EndLoad(Scene _scene, LoadSceneMode _mode)
    {
        endEvent?.Invoke();
        endEvent = null;
    }

    /// <summary>
    /// 씬 전환시 이벤트 추가
    /// </summary>
    /// <param name="_endFunc"></param>
    public static void Add(Action _endFunc)
    {
        endEvent += _endFunc;
    }

    /// <summary>
    /// 씬 전환
    /// </summary>
    public static void Change(SceneName _sceneName)
    {
        if (!scene.ContainsKey(_sceneName)) scene.Add(_sceneName, _sceneName.ToString());
        currentScene = _sceneName;

        Json.Save();
        SceneManager.LoadScene(scene[_sceneName]);
    }
}
