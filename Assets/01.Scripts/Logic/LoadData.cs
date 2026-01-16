using UnityEngine;
using System.Collections.Generic;
using System;

public class LoadData
{
    private List<string> loadNames = new(capacity: 50);
    private Dictionary<SceneChangeManager.SceneName, Action> load = new();

    public void Load(SceneChangeManager.SceneName _sceneName)
    {
        for (int i = 0; i < loadNames.Count; i++)
        {
            AddressablesManager.Release(loadNames[i]);
        }
    }
}
