using UnityEngine;
using UnityEngine.Advertisements;

public class AdvertisementComponent : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string iOSID = "6003551";
    private string androidID = "6003550";
    private string gameID;

    void Awake()
    {
        var testMode = false;

#if UNITY_IOS
        gameID = iOSID;
#elif UNITY_ANDROID
        gameID = androidID;
#elif UNITY_EDITOR
        gameID = androidID;
        testMode = true
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameID, testMode, this);
        }
    }

    #region Initialize
    public void OnInitializationComplete()
    {
        Service.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Service.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    #endregion

    #region Load
    public void OnUnityAdsAdLoaded(string placementId)
    {
        //광고 콘텐츠가 성공적으로 준비되어 표시할 준비가 완료되었음
        Service.Log($"Ad Loaded: {gameID}");
        if (!placementId.Equals(gameID)) return;

    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //네트워크 문제, 콘텐츠 없음, 또는 기타
    }
    #endregion

    #region Show
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //로드된 광고를 표시하는 과정에서 오류가 발생했을 때 발생합니다.
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //광고가 화면에 나타나기 시작했을 때 발생합니다.
        //사운드 일시 정지
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        //사용자가 광고를 클릭하여 스토어로 이동했을 때 발생합니다.
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //사용자가 광고를 끝까지 시청하거나 닫았을 때 발생합니다.
    }
    #endregion
}
