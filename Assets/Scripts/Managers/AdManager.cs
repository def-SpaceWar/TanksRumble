using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

        private static AdManager Instance;

        [Header("Unity Ad Settings")]
        public string gameId; // This will be specifically the Android ID as this is targeted towards Android!
        public bool testMode;

        public string rewardToGive;
        public int amountToGive;

        public void OnInitializationComplete()
        {
                Debug.Log("Ads Are Initialized!");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
                Debug.LogError("Ads Failed to Initialize");
                Debug.LogError(message);
        }

        private void Start()
        {
                if (Instance == null)
                {
                        Advertisement.Initialize(gameId, testMode, this);
                        DontDestroyOnLoad(gameObject);
                        Instance = this;
                }
                else
                {
                        Destroy(gameObject);
                        Destroy(this);
                }
        }

        public void ShowInterstitialAd() => LoadAd("video");
        public void ShowRewardedAd() => LoadAd("rewardedVideo");

        private void LoadAd(string placementId)
        {
                Advertisement.Load(placementId, this);
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
                Advertisement.Show(placementId, this);
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
                Debug.LogError("Ad Failed to Load!");
                Debug.LogError(message);
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
                Debug.LogError("Ad Failed to Show!");
                Debug.LogError(message);
        }

        public void OnUnityAdsShowStart(string placementId)
        {
                //throw new System.NotImplementedException();
                return;
        }

        public void OnUnityAdsShowClick(string placementId)
        {
                //throw new System.NotImplementedException();
                return;
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
                if (placementId != "rewardedVideo") return;

                switch (showCompletionState)
                {
                        case UnityAdsShowCompletionState.COMPLETED:
                                if (rewardToGive == "coins")
                                {
                                        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins", 0) + amountToGive);
                                }

                                break;
                        case UnityAdsShowCompletionState.SKIPPED:
                                Debug.Log("Ad was skipped!");
                                break;
                        case UnityAdsShowCompletionState.UNKNOWN:
                                Debug.Log("Something happened to the Ad...");
                                break;
                }

                rewardToGive = null;
                amountToGive = 0;
        }
}
