using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public static class UiManager
{
    public static Transform Instance { get; private set; } = Init();
    private static Dictionary<Type, UiBase> ui = new();

    private static Transform Init()
    {
        FindEventSystem();
        SpawnManager();
        return Instance;
    }

    private static void SpawnManager()
    {
        Instance = new GameObject("[UiManager]").transform;
        MonoBehaviour.DontDestroyOnLoad(Instance);
    }

    private static void FindEventSystem()
    {
        var eventSystem = MonoBehaviour.FindAnyObjectByType<EventSystem>();
        if (eventSystem) MonoBehaviour.Destroy(eventSystem.gameObject);

        var spawn = new GameObject("[EventSystem]");
        spawn.AddComponent<EventSystem>();
        spawn.AddComponent<InputSystemUIInputModule>();
        MonoBehaviour.DontDestroyOnLoad(spawn);
    }

    /// <summary>
    /// 없는 UI 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void New<T>() where T : UiBase
    {
        var key = typeof(T);

        if (ui.ContainsKey(key))
        {
            var value = ui[key];

            if (!value) ui.Remove(key);
            else return;
        }

        var path = Path.Combine("Ui", $"{key.Name}");
        var load = Resources.Load<T>(path);
        var spawn = MonoBehaviour.Instantiate(load);

        ui.Add(key, spawn);
    }

    /// <summary>
    /// 해당 UI 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Get<T>() where T : UiBase
    {
        New<T>();
        return ui[typeof(T)] as T;
    }

    /// <summary>
    /// Ui 활성화
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void On<T>() where T : UiBase
    {
        New<T>();
        ui[typeof(T)].On();
    }

    /// <summary>
    /// Ui 비활성화
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void Off<T>() where T : UiBase
    {
        New<T>();
        ui[typeof(T)].Off();
    }
}
