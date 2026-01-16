using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class AddressablesManager
{
    private static Dictionary<string, Object> resources = new();

    /// <summary>
    /// 리소스 로드
    /// </summary>
    /// <param name="_resourcesName"></param>
    public static async void Load<T>(string _resourcesName) where T : Object
    {
        if (resources.ContainsKey(_resourcesName)) return;

        var handle = Addressables.LoadAssetAsync<T>(_resourcesName);
        await handle.Task;

        var load = handle.Result;

        if (handle.Status == AsyncOperationStatus.Succeeded) resources.Add(_resourcesName, handle.Result);
        else Service.Log($"{_resourcesName}로드 안됨");
    }

    /// <summary>
    /// 로드된 리소스 반환
    /// </summary>
    /// <param name="_resourcesName"></param>
    /// <returns></returns>
    public static Object Get<T>(string _resourcesName) where T : Object
    {
        if (resources.ContainsKey(_resourcesName)) return resources[_resourcesName] as T;
        else Service.Log($"{_resourcesName}는 없는 리소스");

        return null;
    }

    /// <summary>
    /// 리소스 릴리즈
    /// </summary>
    /// <param name="_resourcesName"></param>
    public static void Release(string _resourcesName)
    {
        if (resources.ContainsKey(_resourcesName))
        {
            Addressables.Release(_resourcesName);
            resources.Remove(_resourcesName);
        }
    }
}
