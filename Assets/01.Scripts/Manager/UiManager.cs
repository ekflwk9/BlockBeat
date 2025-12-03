using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class UiManager
{
    private static Dictionary<Type, UiBase> ui = Init();
    private static Transform Instance;

    private static Dictionary<Type, UiBase> Init()
    {
        Instance = new GameObject("UiManager").transform;
        MonoBehaviour.DontDestroyOnLoad(Instance);

        return new();
    }

    /// <summary>
    /// 없는 UI 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void New<T>() where T : UiBase
    {
        var key = typeof(T);
        if (ui.ContainsKey(key)) return;

        var path = Path.Combine("Prefabs", "Ui");
        var spawn = Resources.Load<T>(path);
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
