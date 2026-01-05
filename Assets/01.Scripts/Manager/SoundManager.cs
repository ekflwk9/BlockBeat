using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SoundManager
{
    public enum SoundName
    {
        Block,
        Buy,
        Coin,
        Evade,
        Dead,
        Touch,
        GameOver,
        MainMusic,
    }

    private static AudioSource music;
    private static AudioSource[] effect;

    private static Dictionary<SoundName, AudioClip> sounds = new();
    private static int effectIndex = Init();

    private static int Init()
    {
        var manager = new GameObject("[SoundManager]");
        MonoBehaviour.DontDestroyOnLoad(manager);

        InitMusic(manager);
        InitEffect(manager);
        InitSound();

        return 0;
    }

    private static void InitMusic(GameObject _instance)
    {
        music = _instance.AddComponent<AudioSource>();
        music.playOnAwake = false;
        music.loop = true;
    }

    private static void InitEffect(GameObject _instance)
    {
        var effectCount = 10;
        effect = new AudioSource[effectCount];

        for (int i = 0; i < effectCount; i++)
        {
            effect[i] = _instance.AddComponent<AudioSource>();
            effect[i].playOnAwake = false;
        }
    }

    private static void InitSound()
    {
        var soundNames = (SoundName[])Enum.GetValues(typeof(SoundName));
        var path = "Sound";

        for (int i = 0; i < soundNames.Length; i++)
        {
            var fileName = Path.Combine(path, soundNames[i].ToString());
            var load = Resources.Load<AudioClip>(fileName);
            if (load) sounds.Add(soundNames[i], load);
        }
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    public static void OnEffect(SoundName _soundName)
    {
        if (!sounds.ContainsKey(_soundName))
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
    public static void OnMusic(SoundName _soundName, bool _isLoop = true)
    {
        if (!sounds.ContainsKey(_soundName))
        {
            Service.Log($"{_soundName.ToString()}이라는 사운드는 로드되지 않음");
            return;
        }

        else if(music.clip && Equals(sounds[_soundName], music.clip))
        {
            return;
        }

        music.loop = _isLoop;
        music.clip = sounds[_soundName];
        music.Play();
    }

    /// <summary>
    /// 사운드 정지 (일시 정지 아님)
    /// </summary>
    public static void OffMusic()
    {
        music.clip = null;
        music.Stop();
    }

    /// <summary>
    /// 효과음 볼륨 음소거 여부 (모바일 전용)
    /// </summary>
    /// <param name="_isOff"></param>
    public static void SetEffectVolume()
    {
        Json.SetEffectSound();
        var newVolume = Json.GetEffectSound() ? 1f : 0f;

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
        Json.SetMusicSound();
        music.volume = Json.GetMusicSound() ? 1f : 0f;
    }
}
