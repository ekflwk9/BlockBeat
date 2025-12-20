using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum SoundName
    {
    
    }

    private static AudioSource music = Init();
    private static AudioSource[] effect;

    private static Dictionary<SoundName, AudioClip> sounds = new();
    private static int effectIndex;

    private static AudioSource Init()
    {
        var manager = new GameObject("[SoundManager]");
        MonoBehaviour.DontDestroyOnLoad(manager);

        music = manager.AddComponent<AudioSource>();
        music.playOnAwake = false;
        music.loop = true;

        var effectCount = 10;
        effect = new AudioSource[effectCount];

        for (int i = 0; i < effectCount; i++)
        {
            effect[i] = manager.AddComponent<AudioSource>();
            effect[i].playOnAwake = false;
        }

        return music;
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    public static void OnEffect(SoundName _soundName)
    {
        if(!sounds.ContainsKey(_soundName))
        {
            Service.Log($"{_soundName.ToString()}이라는 사운드는 로드되지 않음");
            return;
        }

        effectIndex++;
        if (effect.Length <= effectIndex) effectIndex = 0;

        effect[effectIndex].clip = sounds[_soundName];
        effect[effectIndex].Play();
    }

    /// <summary>
    /// 배경음 재생
    /// </summary>
    public static void OnMusic(SoundName _soundName)
    {
        if (!sounds.ContainsKey(_soundName))
        {
            Service.Log($"{_soundName.ToString()}이라는 사운드는 로드되지 않음");
            return;
        }

        music.clip = sounds[_soundName];
        music.Play();
    }

    /// <summary>
    /// 효과음 볼륨 음소거 여부 (모바일 전용)
    /// </summary>
    /// <param name="_isOff"></param>
    public static void SetEffectVolume()
    {
        var newVolume = Json.GetEffectSound() ? 1f : 0f;
        Json.SetEffectSound();
        Json.Save();

        for (int i = 0; i < effect.Length; i++)
        {
            effect[i].volume = newVolume;
        }
    }

    /// <summary>
    /// 음악 볼륨 음소거 여부 (모바일 전용)
    /// </summary>
    /// <param name="_isOff"></param>
    public static void SetMusicVolume()
    {
        music.volume = Json.GetMusicSound() ? 1f : 0f;
        Json.SetMusicSound();
        Json.Save();
    }
}
