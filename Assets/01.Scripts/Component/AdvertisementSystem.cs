using UnityEngine;
using UnityEngine.Advertisements;

#if UNITY_ANDROID || UNITY_IOS
public class AdvertisementSystem : IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private static AdvertisementSystem instance = new();
    public const int maxPassCount = 2;

#if UNITY_IOS
    private const string gameID = "6003730";
    private const string placementID = "Rewarded_iOS";
#elif UNITY_ANDROID
    private const string gameID = "6003731";
    private const string placementID = "Rewarded_Android";
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Init()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameID, Application.isEditor, instance);
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        }
    }

    /// <summary>
    /// 배너광고 출력 여부
    /// </summary>
    /// <param name="_enabled"></param>
    public static void ShowBanner(bool _enabled)
    {
        if (_enabled)
        {
            Advertisement.Banner.Show(placementID);
        }

        else
        {
            Advertisement.Banner.Hide();
            Advertisement.Banner.Load(placementID);
        }
    }

    /// <summary>
    /// 광고 출력
    /// </summary>
    public static void ShowVideo()
    {
        Advertisement.Show(placementID, instance);
    }

    #region Initialize
    public void OnInitializationComplete()
    {
        //Service.Log("광고 초기화 완료");
        Advertisement.Load(placementID, this);
        Advertisement.Banner.Load(placementID);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        //광고 초기화 실패
        Service.Log($"{error}/{message}");
    }
    #endregion

    #region Load
    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (!placementId.Equals(placementID)) return;
        Service.Log($"{placementID} 광고 준비 완료");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //네트워크 문제, 콘텐츠 없음, 또는 기타
        Service.Log($"{error}/{message}");
    }
    #endregion

    #region Show
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //로드된 광고 표시 과정에서 오류 발생
        Service.Log($"{error}/{message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Service.Log($"광고 출력 시작");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Service.Log($"광고를 클릭하여 스토어로 이동 했음");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Service.Log($"광고 시청 후 광고를 닫았음");

        Json.SetAdvertPass(0);
        Advertisement.Load(placementID, this);
        UiManager.Get<FadeUi>().FadeOut(0.5f);
    }
    #endregion
}

#else
public class AdvertisementComponent : MonoBehaviour
{
}
#endif