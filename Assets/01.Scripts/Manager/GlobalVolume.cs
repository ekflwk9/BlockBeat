using DG.Tweening;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolume : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;

    private Vignette vignette;
    private ChromaticAberration chromaticAberration;

#if UNITY_EDITOR
    private void Reset()
    {
        globalVolume = this.TryGetComponent<Volume>();
        this.name = typeof(GlobalVolumeManager).Name;
    }
#endif

    private void Awake()
    {
        chromaticAberration = TryGet<ChromaticAberration>();
        chromaticAberration.active = true;
        chromaticAberration.intensity.overrideState = true;

        vignette = TryGet<Vignette>();
        vignette.active = true;
        vignette.intensity.overrideState = true;
    }

    private T TryGet<T>() where T : VolumeComponent
    {
        if (globalVolume.profile.TryGet<T>(out var _component))
        {
            return _component;
        }

        else
        {
            Service.Log($"GlobalVolumeManager에 {_component.name}이란 볼륨은 추가되지 않음");
            return null;
        }
    }

    public void SetChromatic(float intensity, float duration = 1f, bool fade = true)
    {
        if (fade) DOTween.To(GetChromaticValue, SetChromaticValue, intensity, duration);
        else chromaticAberration.intensity.value = intensity;
    }

    private float GetChromaticValue() => chromaticAberration.intensity.value;
    private void SetChromaticValue(float _value) => chromaticAberration.intensity.value = _value;

    public void SetVignette(float intensity, float duration = 1f, bool fade = true)
    {
        if (fade) DOTween.To(GetVignetteValue, SetVignetteValue, intensity, 1.0f);
        else vignette.intensity.value = intensity;
    }

    private float GetVignetteValue() => vignette.intensity.value;
    private void SetVignetteValue(float _value) => vignette.intensity.value = _value;
}

public static class GlobalVolumeManager
{
    public static GlobalVolume Instance { get; private set; } = Init();

    private static GlobalVolume Init()
    {
        var name = typeof(GlobalVolumeManager).Name;
        var path = Path.Combine("Prefabs", $"{name}");
        var load = Resources.Load<GlobalVolume>(path);
        var spawn = MonoBehaviour.Instantiate(load);
        spawn.name = $"[{name}]";

        MonoBehaviour.DontDestroyOnLoad(spawn.gameObject);
        return spawn;
    }
}
