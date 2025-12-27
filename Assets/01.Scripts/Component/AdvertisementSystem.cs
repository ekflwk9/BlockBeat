using UnityEngine;
using UnityEngine.Advertisements;

#if UNITY_ANDROID || UNITY_IOS
public class AdvertisementSystem : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private const int maxPassCount = 3;
    private static bool isBanner;

#if UNITY_IOS
    private const string gameID = "6003730";
    private const string placementID = "Rewarded_iOS";
#elif UNITY_ANDROID
    private const string gameID = "6003731";
    private const string placementID = "Rewarded_Android";
#endif

#if UNITY_EDITOR
    private void Reset()
    {
        this.name = typeof(AdvertisementSystem).Name;
        this.transform.position = Vector3.zero;
    }
#endif

    void Awake()
    {
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Json.SetAdvertPass(0); //패스 횟수 초기화
            Advertisement.Initialize(gameID, Application.isEditor, this);
        }
    }

    private void Start()
    {
        ShowBanner();
        ShowVideo();
    }

    private void ShowBanner()
    {
        if ((Json.GetAdvertPass() < maxPassCount) &&
            (SceneChangeManager.currentScene == SceneChangeManager.SceneName.Result ||
             SceneChangeManager.currentScene == SceneChangeManager.SceneName.Rank))
        {
            isBanner = true;

            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
            Advertisement.Banner.Show(placementID);
        }

        else if (isBanner)
        {
            isBanner = false;

            Advertisement.Banner.Hide();
            Advertisement.Banner.Load(placementID);
        }
    }

    private void ShowVideo()
    {
        if (SceneChangeManager.currentScene != SceneChangeManager.SceneName.Result) return;
        var passCount = Json.GetAdvertPass();

        if (maxPassCount <= passCount)
        {
            Json.SetAdvertPass(0);
            Advertisement.Show(placementID, this);
        }

        else
        {
            Json.SetAdvertPass(passCount + 1);
        }
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
        //사운드 일시 정지
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Service.Log($"광고를 클릭하여 스토어로 이동 했음");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Service.Log($"광고 시청 후 광고를 닫았음");

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