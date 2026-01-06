using System.Collections.Generic;
using UnityEngine;

public static class CoroutineManager
{
    //private static WaitForFixedUpdate waitFixed = new();
    private static Dictionary<float, WaitForSeconds> wait = new(capacity : 50);

    /// <summary>
    /// 캐싱된 WaitForSeconds 반환
    /// </summary>
    /// <param name="_timer"></param>
    /// <returns></returns>
    public static WaitForSeconds Wait(float _timer)
    {
        if (!wait.ContainsKey(_timer)) wait.Add(_timer, new WaitForSeconds(_timer));
        return wait[_timer];
    }

    /// <summary>
    /// FixedUpdate가 모두 완료될 때까지 멈추는 코루틴
    /// </summary>
    /// <returns></returns>
    //public static WaitForFixedUpdate WaitFixed()
    //{
    //    return waitFixed;
    //}
}
