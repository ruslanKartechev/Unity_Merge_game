using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if MADPIXEL_AMAZON_DROID && UNITY_ANDROID
using AmazonAds;
#endif
#if UNITY_IOS
using Unity.Advertisement.IosSupport; // NOTE: Import "com.unity.ads.ios-support" from Package Manager, if it's missing
#endif

namespace MAXHelper {
    public class AppLovinComp : MonoBehaviour {
        #region Fields
        private MaxSdkBase.AdInfo ShowedInfo;
        private MAXCustomSettings Settings;
        private string RewardedID = "empty";
        private string BannerID = "empty";
        private string InterstitialID = "empty";
        public bool bInitialized { get; private set; }
        [SerializeField] private bool bShowDebug;
        private List<string> Keywords;
        #endregion


        #region Events Declaration
        public UnityAction<bool> onFinishAdsEvent;
        public UnityAction<MaxSdkBase.AdInfo, MaxSdkBase.ErrorInfo, AdsManager.EAdType> onErrorEvent;
        public UnityAction onInterDismissedEvent;
        public UnityAction OnBannerInitialized;
        public UnityAction<bool> onAdLoadedEvent; // true = rewarded 

        public UnityAction<string, MaxSdkBase.AdInfo, MaxSdkBase.ErrorInfo> onBannerLoadedEvent;
        public UnityAction<string, MaxSdkBase.AdInfo> onBannerRevenueEvent;
        #endregion


        #region Initialization
        public void Init(MAXCustomSettings CustomSettings) {
            Settings = CustomSettings;
            if (string.IsNullOrEmpty(Settings.SDKKey)) {
                Debug.LogError("[MadPixel] Cant init SDK with a null SDK key!");
            }
            else {
                MaxSdkCallbacks.OnSdkInitializedEvent += OnAppLovinInitialized;
                InitSDK();
                GetTierKeywords();
                MaxSdk.TargetingData.Keywords = Keywords.ToArray();
            }
        }

        private void InitSDK() {
            MaxSdk.SetSdkKey(Settings.SDKKey);
            MaxSdk.InitializeSdk();
            MaxSdk.SetVerboseLogging(bShowDebug);
        }

        private void GetTierKeywords() {
            if (Keywords == null) {
                Keywords = new List<string>();
                bool bUpper = true;
                if (SystemInfo.deviceType == DeviceType.Handheld) {
                    bUpper = SystemInfo.systemMemorySize > 5000 &&
                             SystemInfo.graphicsMemorySize > 2000 &&
                             SystemInfo.processorCount >= 8 &&
                             Screen.currentResolution.height >= 1920 &&
                             Screen.currentResolution.width >= 1080;

                }

                Keywords.Add("tier:" + (bUpper ? "upper" : "lower"));
            }
        }


        private void OnAppLovinInitialized(MaxSdkBase.SdkConfiguration sdkConfiguration) {
            if (Settings.bShowMediationDebugger) {
                MaxSdk.ShowMediationDebugger();
            }

            if (Settings.bUseBanners) {
                InitializeBannerAds();
            }

            if (Settings.bUseRewardeds) {
                InitializeRewardedAds();
            }

            if (Settings.bUseInters) {
                InitializeInterstitialAds();
            }

            Debug.Log("[MadPixel] AppLovin is initialized");
            bInitialized = true;
        }

        #endregion

        #region Banners
        public void InitializeBannerAds() {
            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

#if UNITY_ANDROID
            if (!string.IsNullOrEmpty(Settings.BannerID)) {
                BannerID = Settings.BannerID;
            } else {
                Debug.LogError("[MadPixel] Banner ID in Settings is Empty!");
            }
#else
            if (!string.IsNullOrEmpty(Settings.BannerID_IOS)) {
                BannerID = Settings.BannerID_IOS;
            } else {
                Debug.LogError("Banner ID in Settings is Empty!");
            }
#endif

#if MADPIXEL_AMAZON_DROID && UNITY_ANDROID && !UNITY_EDITOR
            int width = 320;
            int height = 50;

            var apsBanner = new APSBannerAdRequest(width, height, Settings.AmazonBannerID);
            apsBanner.onSuccess += (adResponse) => {
                Debug.LogWarning($"[MadPixel] Banners. Amazon.onSuccess!");
                MaxSdk.SetBannerLocalExtraParameter(BannerID, "amazon_ad_response", adResponse.GetResponse());
                MaxSdk.CreateBanner(BannerID, MaxSdkBase.BannerPosition.BottomCenter);
                MaxSdk.SetBannerBackgroundColor(BannerID, Settings.BannerBackground);
            };
            apsBanner.onFailedWithError += (adError) => {
                Debug.LogWarning($"[MadPixel] Banners. Amazon.onFailedWithError!");
                MaxSdk.SetBannerLocalExtraParameter(BannerID, "amazon_ad_error", adError.GetAdError());
                MaxSdk.CreateBanner(BannerID, MaxSdkBase.BannerPosition.BottomCenter);
                MaxSdk.SetBannerBackgroundColor(BannerID, Settings.BannerBackground);
            };

            apsBanner.LoadAd();
#else
            MaxSdk.CreateBanner(BannerID, MaxSdkBase.BannerPosition.BottomCenter);
            MaxSdk.SetBannerBackgroundColor(BannerID, Settings.BannerBackground);
#endif

            OnBannerInitialized?.Invoke();
        }

        private void OnBannerAdLoadedEvent(string type, MaxSdkBase.AdInfo adInfo) {
            if (bShowDebug) {
                Debug.Log($"OnBannerAdLoadedEvent invoked. {type}, {adInfo}");
            }
            onBannerLoadedEvent?.Invoke(type, adInfo, null);
        }

        private void OnBannerAdLoadFailedEvent(string type, MaxSdkBase.ErrorInfo errorInfo) {
            if (bShowDebug) {
                Debug.Log($"OnBannerAdLoadFailedEvent invoked. {type}, {errorInfo}");
            }
            onBannerLoadedEvent?.Invoke(type, null, errorInfo);
        }

        private void OnBannerAdRevenuePaidEvent(string type, MaxSdkBase.AdInfo adInfo) {
            if (bShowDebug) {
                Debug.Log($"OnBannerAdRevenuePaidEvent invoked. {type}, {adInfo}");
            }
            onBannerRevenueEvent?.Invoke(type, adInfo);
        }

        public void ShowBanner(bool bShow, MaxSdkBase.BannerPosition NewPosition = MaxSdkBase.BannerPosition.BottomCenter) {
            if (bInitialized) {
                if (bShow) {
                    MaxSdk.UpdateBannerPosition(BannerID, NewPosition);
                    MaxSdk.ShowBanner(BannerID);
                }
                else {
                    MaxSdk.HideBanner(BannerID);
                }
            }
        }
        #endregion

        #region Interstitials
        public void InitializeInterstitialAds() {
            // Attach callback
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialFailedToDisplayEvent;

            // Load the first interstitial
            LoadInterstitial();
        }

        public void CancelInterAd() {
            onInterDismissedEvent?.Invoke();
        }

        private bool isFirstLoad_inter = true;
        private void LoadInterstitial() {
            if (isFirstLoad_inter) {
#if UNITY_ANDROID
                if (!string.IsNullOrEmpty(Settings.InterstitialID)) {
                    InterstitialID = Settings.InterstitialID;
                }
                else {
                    Debug.LogError("[MadPixel] Interstitial ID in Settings is Empty!");
                }
#else
                if (!string.IsNullOrEmpty(Settings.InterstitialID_IOS)) {
                    InterstitialID = Settings.InterstitialID_IOS;
                } else {
                    Debug.LogError("Interstitial ID in Settings is Empty!");
                }
#endif
#if MADPIXEL_AMAZON_DROID && UNITY_ANDROID && !UNITY_EDITOR
                // APS LoadAd only needs to be called once.
                isFirstLoad_inter = false;
                var interstitialVideoAdRequest = new APSVideoAdRequest(320, 480, Settings.AmazonInterstitialID);
                interstitialVideoAdRequest.onSuccess += (adResponse) => {
                    Debug.LogWarning($"[MadPixel] Inters. Amazon.onSuccess!");
                    MaxSdk.SetInterstitialLocalExtraParameter(InterstitialID, "amazon_ad_response", adResponse.GetResponse());
                    MaxSdk.LoadInterstitial(InterstitialID);
                };
                interstitialVideoAdRequest.onFailedWithError += (adError) => {
                    Debug.LogWarning($"[MadPixel] Inters. Amazon.onFailedWithError!");
                    MaxSdk.SetInterstitialLocalExtraParameter(InterstitialID, "amazon_ad_error", adError.GetAdError());
                    MaxSdk.LoadInterstitial(InterstitialID);
                };
                interstitialVideoAdRequest.LoadAd();
#else
                isFirstLoad_inter = false;
                MaxSdk.LoadInterstitial(InterstitialID);
#endif
            }
            else {
                MaxSdk.LoadInterstitial(InterstitialID);
            }
        }

        private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            onAdLoadedEvent?.Invoke(false);
        }

        private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
            // Interstitial ad failed to load. We recommend re-trying in 3 seconds.
            Invoke("LoadInterstitial", 3);
            Debug.LogWarning("OnInterstitialFailedEvent");
        }

        private void OnInterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) {
            if (bShowDebug) {
                Debug.Log("OnInterstitialFailedToDisplayEvent invoked");
            }
            LoadInterstitial();

            onErrorEvent?.Invoke(adInfo, errorInfo, AdsManager.EAdType.INTER);

            Debug.LogWarning("InterstitialFailedToDisplayEvent");
        }

        private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            if (bShowDebug) {
                Debug.Log("OnInterstitialDismissedEvent invoked");
            }

            LoadInterstitial();
            onInterDismissedEvent?.Invoke();
        }
        #endregion

        #region Rewarded
        public void InitializeRewardedAds() {
            // Attach callback
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

            // Load the first RewardedAd
            LoadRewardedAd();
        }

        public void CancelRewardedAd() {
            onFinishAdsEvent?.Invoke(false);
            ShowedInfo = null;
        }

        private bool isFirstLoad_rew = true;
        private void LoadRewardedAd() {
            if (isFirstLoad_rew) {
#if UNITY_ANDROID
                if (!string.IsNullOrEmpty(Settings.RewardedID)) {
                    RewardedID = Settings.RewardedID;
                }
                else {
                    Debug.LogError("[MadPixel] Rewarded ID in Settings is Empty!");
                }
#else
                if (!string.IsNullOrEmpty(Settings.RewardedID_IOS)) {
                    RewardedID = Settings.RewardedID_IOS;
                } else {
                    Debug.LogError("Rewarded ID in Settings is Empty!");
                }
#endif
#if MADPIXEL_AMAZON_DROID && UNITY_ANDROID && !UNITY_EDITOR
                // APS LoadAd only needs to be called once.
                isFirstLoad_rew = false;
                var rewardedVideoAdRequest = new APSVideoAdRequest(320, 480, Settings.AmazonRewardedID);
                rewardedVideoAdRequest.onSuccess += (adResponse) => {
                    Debug.LogWarning($"[MadPixel] Rewardeds. Amazon.onSuccess!");
                    MaxSdk.SetRewardedAdLocalExtraParameter(RewardedID, "amazon_ad_response", adResponse.GetResponse());
                    MaxSdk.LoadRewardedAd(RewardedID);
                };
                rewardedVideoAdRequest.onFailedWithError += (adError) => {
                    Debug.LogWarning($"[MadPixel] Rewardeds. Amazon.onFailedWithError!");
                    MaxSdk.SetRewardedAdLocalExtraParameter(RewardedID, "amazon_ad_error", adError.GetAdError());
                    MaxSdk.LoadRewardedAd(RewardedID);
                };
                rewardedVideoAdRequest.LoadAd();
#else
                isFirstLoad_rew = false;
                MaxSdk.LoadRewardedAd(RewardedID);
#endif
            }
            else {
                MaxSdk.LoadRewardedAd(RewardedID);
            }
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            if (bShowDebug) {
                Debug.Log("OnRewardedAdDisplayedEvent invoked");
            }
            ShowedInfo = adInfo;
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            onAdLoadedEvent?.Invoke(true);
            ShowedInfo = adInfo; 
            if (bShowDebug) {
                Debug.Log("OnRewardedAdLoadedEvent invoked");
            }
        }

        private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) {
            // Rewarded ad failed to load. We recommend re-trying in 3 seconds.
            Invoke("LoadRewardedAd", 3); 
            if (bShowDebug) {
                Debug.Log("OnRewardedAdLoadFailedEvent invoked");
            }
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) {
            // Rewarded ad failed to display. We recommend loading the next ad

            if (bShowDebug) {
                Debug.Log("OnRewardedAdFailedToDisplayEvent invoked");
            }

            OnError(adInfo, errorInfo);
            LoadRewardedAd();
        }

        private void OnError(MaxSdkBase.AdInfo adInfo, MaxSdkBase.ErrorInfo EInfo) {
            onErrorEvent?.Invoke(adInfo, EInfo, AdsManager.EAdType.REWARDED);
            ShowedInfo = null;
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
            if (bShowDebug) {
                Debug.Log("OnRewardedAdDismissedEvent invoked");
            }

            if (ShowedInfo != null) {
                onFinishAdsEvent?.Invoke(false);
            }
            
            ShowedInfo = null;
            LoadRewardedAd();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo) {
            if (bShowDebug) {
                Debug.Log("OnRewardedAdReceivedRewardEvent invoked");
            }

            onFinishAdsEvent?.Invoke(ShowedInfo != null);
            ShowedInfo = null;
        }

        #endregion

        #region Show Ads

        public bool ShowInterstitial() {
            if (bInitialized && MaxSdk.IsInterstitialReady(InterstitialID)) {
                MaxSdk.ShowInterstitial(InterstitialID);
                return true;
            }

            return false;
        }

        public void ShowRewarded() {
            if (bInitialized && MaxSdk.IsRewardedAdReady(RewardedID)) {
                MaxSdk.ShowRewardedAd(RewardedID);
            }
        }

        public bool IsReady(bool bIsRewarded) {
            if (bInitialized) {
                if (bIsRewarded) {
                    return MaxSdk.IsRewardedAdReady(RewardedID);
                }
                else {
                    return MaxSdk.IsInterstitialReady(InterstitialID);
                }
            }
            return false;
        }
        #endregion

        #region Unsubscribers

        void OnDestroy() {
            UnsubscribeAll();
        }
        public void UnsubscribeAll() {
            if (bInitialized) {
                MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoadedEvent;
                MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialFailedEvent;
                MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialDismissedEvent;
                MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialFailedToDisplayEvent;

                MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoadedEvent;
                MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedAdLoadFailedEvent;
                MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayedEvent;
                MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedAdDismissedEvent;
                MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedAdFailedToDisplayEvent;
                MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedRewardEvent;
            }
        }

        #endregion

        public void AddMediaSourceKeyword(string mediaSource) {
            MaxSdk.TargetingData.Keywords = null;

            mediaSource = MadPixel.ExtensionMethods.RemoveAllWhitespacesAndNewLines(mediaSource);

            GetTierKeywords(); 
            string msKeyword = "media_source:" + mediaSource;
            if (!Keywords.Contains(msKeyword)) {
                Keywords.Add(msKeyword);
            }

            Keywords.Add("app_version:" + Application.version.Replace(".", string.Empty));

            if (PlayerPrefs.GetInt("FirstPurchaseWas", 0) == 1) { // NOTE: was set for appsflyer in AnalyticsManager
                string purchKeyword = "purchase:purchase";
                if (!Keywords.Contains(purchKeyword)) {
                    Keywords.Add(purchKeyword);
                }
            }

            MaxSdk.TargetingData.Keywords = Keywords.ToArray();

            if (bShowDebug) {
                foreach (string key in Keywords) {
                    Debug.Log($"Keyword added: {key}");
                }
            }
        }

        public void AddPurchaseKeyword(string keyword) {
            if (Keywords != null && !Keywords.Contains(keyword)) {
                Keywords.Add(keyword);
                MaxSdk.TargetingData.Keywords = Keywords.ToArray();
            }

            if (bShowDebug) {
                foreach (string key in Keywords) {
                    Debug.Log($"Keyword added: {key}");
                }
            }
        }
    }
}