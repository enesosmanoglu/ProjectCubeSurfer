using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HmsPlugin;
using HuaweiMobileServices.Ads;

public class AdManager : MonoBehaviour
{
    private readonly string TAG = "[AdManager]";
    public bool adsRemoved = false;

    #region Singleton Bool
    public static AdManager Instance { get; private set; }
    private bool Singleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            Destroy(this);
            return false;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            return true;
        }
    }
    #endregion

    private void Awake()
    {
        if (!Singleton()) return;
        Debug.Log("### AWAKE ### AdManager");
    }

    private void Start()
    {
        adsRemoved = PlayerPrefs.GetInt("AdsRemoved", 0) == 1;
        Debug.Log($"{TAG} [PLAYER-PREFS] AdsRemoved: {adsRemoved}");

        HMSAdsKitManager.Instance.OnRewarded = OnRewarded;
        HMSAdsKitManager.Instance.OnInterstitialAdClosed = OnInterstitialAdClosed;

        HMSAdsKitManager.Instance.ConsentOnFail = OnConsentFail;
        HMSAdsKitManager.Instance.ConsentOnSuccess = OnConsentSuccess;
        HMSAdsKitManager.Instance.RequestConsentUpdate();

        //testAdStatusToggle = GameObject.FindGameObjectWithTag("Toggle").GetComponent<Toggle>();
        //testAdStatusToggle.isOn = HMSAdsKitSettings.Instance.Settings.GetBool(HMSAdsKitSettings.UseTestAds);


        #region SetNonPersonalizedAd , SetRequestLocation

        var builder = HwAds.RequestOptions.ToBuilder();

        builder
            .SetConsent("tcfString")
            .SetNonPersonalizedAd((int)NonPersonalizedAd.ALLOW_ALL)
            .Build();

        bool requestLocation = true;
        var requestOptions = builder.SetConsent("testConsent").SetRequestLocation(requestLocation).Build();

        Debug.Log($"{TAG} RequestOptions NonPersonalizedAds:  {requestOptions.NonPersonalizedAd}");
        Debug.Log($"{TAG} Consent: {requestOptions.Consent}");

        #endregion

    }

    public void RemoveAds()
    {
        adsRemoved = true;
        PlayerPrefs.SetInt("AdsRemoved", 1);
        PlayerPrefs.Save();
        HideBannerAd();
        Managers.Reference.removeAdsButton?.gameObject.SetActive(false);
    }

    private void OnConsentSuccess(ConsentStatus consentStatus, bool isNeedConsent, IList<AdProvider> adProviders)
    {
        Debug.Log($"{TAG} OnConsentSuccess consentStatus:{consentStatus} isNeedConsent:{isNeedConsent}");
        foreach (var AdProvider in adProviders)
        {
            Debug.Log($"{TAG} OnConsentSuccess adproviders: Id:{AdProvider.Id} Name:{AdProvider.Name} PrivacyPolicyUrl:{AdProvider.PrivacyPolicyUrl} ServiceArea:{AdProvider.ServiceArea}");
        }
    }

    private void OnConsentFail(string desc)
    {
        Debug.LogError($"{TAG} OnConsentFail:{desc}");
    }

    public void ShowBannerAd()
    {
        if (adsRemoved) return;
        Debug.Log($"{TAG} ShowBannerAd");

        HMSAdsKitManager.Instance.ShowBannerAd();
    }

    public void HideBannerAd()
    {
        Debug.Log($"{TAG} HideBannerAd");

        HMSAdsKitManager.Instance.HideBannerAd();
    }

    public void ShowRewardedAd()
    {
        if (adsRemoved) { OnRewarded(null); return; }
        Debug.Log($"{TAG} ShowRewardedAd");
        HMSAdsKitManager.Instance.ShowRewardedAd();
    }

    public void ShowInterstitialAd()
    {
        if (adsRemoved) return;
        Debug.Log($"{TAG} ShowInterstitialAd");
        HMSAdsKitManager.Instance.ShowInterstitialAd();
    }

    public void ShowSplashImage()
    {
        if (adsRemoved) return;
        Debug.Log($"{TAG} ShowSplashImage!");

        HMSAdsKitManager.Instance.LoadSplashAd("testq6zq98hecj", SplashAd.SplashAdOrientation.PORTRAIT);
    }

    public void ShowSplashVideo()
    {
        if (adsRemoved) return;
        Debug.Log($"{TAG} ShowSplashVideo!");

        HMSAdsKitManager.Instance.LoadSplashAd("testd7c5cewoj6", SplashAd.SplashAdOrientation.PORTRAIT);
    }

    public void OnRewarded(Reward reward)
    {
        Debug.Log($"{TAG} rewarded!");
        Managers.Game.CollectDiamond(10);
    }

    public void OnInterstitialAdClosed()
    {
        Debug.Log($"{TAG} interstitial ad closed");
    }

    public void SetTestAdStatus()
    {
        // HMSAdsKitManager.Instance.SetTestAdStatus(testAdStatusToggle.isOn);
        HMSAdsKitManager.Instance.SetTestAdStatus(HMSAdsKitSettings.Instance.Settings.GetBool(HMSAdsKitSettings.UseTestAds));
        HMSAdsKitManager.Instance.DestroyBannerAd();
        HMSAdsKitManager.Instance.LoadAllAds();
    }

}
