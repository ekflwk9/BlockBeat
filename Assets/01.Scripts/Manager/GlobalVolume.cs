using DG.Tweening;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class GlobalVolumeManager
{
    private static Volume globalVolume = Init();

    private static Vignette vignette;
    private static ChromaticAberration chromaticAberration;

    private static Volume Init()
    {
        var name = typeof(GlobalVolumeManager).Name;
        var path = Path.Combine("Prefabs", $"{name}");
        var load = Resources.Load<Volume>(path);
        var spawn = MonoBehaviour.Instantiate(load);

        spawn.name = $"[{name}]";
        MonoBehaviour.DontDestroyOnLoad(spawn);

        globalVolume = spawn;
        VolumeInit();

        return spawn;
    }

    public static void VolumeInit()
    {
        chromaticAberration = TryGet<ChromaticAberration>();
        chromaticAberration.active = true;
        chromaticAberration.intensity.overrideState = true;

        vignette = TryGet<Vignette>();
        vignette.active = true;
        vignette.intensity.overrideState = true;
    }

    private static T TryGet<T>() where T : VolumeComponent
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

    public static void SetChromatic(float intensity, float duration = 1f, bool fade = true)
    {
        if (fade) DOTween.To(GetChromaticValue, SetChromaticValue, intensity, duration);
        else chromaticAberration.intensity.value = intensity;
    }

    private static float GetChromaticValue() => chromaticAberration.intensity.value;
    private static void SetChromaticValue(float _value) => chromaticAberration.intensity.value = _value;

    public static void SetVignette(float intensity, float duration = 1f, bool fade = true)
    {
        if (fade) DOTween.To(GetVignetteValue, SetVignetteValue, intensity, 1.0f);
        else vignette.intensity.value = intensity;
    }

    private static float GetVignetteValue() => vignette.intensity.value;
    private static void SetVignetteValue(float _value) => vignette.intensity.value = _value;
}
