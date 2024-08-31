using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine.Purchasing;

#if ADS_ADMOB
using GoogleMobileAds.Api;
#endif
#if ADS_UNITY
using UnityEngine.Monetization;
using UnityEngine.Advertisements;
#endif

public class ServicesManager : MonoBehaviour {

    public static ServicesManager instance { get; set; }

    [HideInInspector] public int rewardedCoins;

    #region ADMOB
    [HideInInspector] public bool enableTestMode;
    [HideInInspector] public string appID;
    [HideInInspector] public string bannerID;
#if ADS_ADMOB
    [HideInInspector] public AdPosition bannerPosition;
#endif
    [HideInInspector] public string interstitialID;
    [HideInInspector] public string rewardedVideoAdsID;

#if ADS_ADMOB
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardVideoAd;
#endif

    #endregion
    #region UnityAds
    [HideInInspector] public bool testMode;
    [HideInInspector] public string gameID;
    [HideInInspector] public string bannerPlacementID;
    [HideInInspector] public string videoAdPlacementID;
    [HideInInspector] public string rewardedVideoAdPlacementID;
    #endregion
    #region IAP
    [HideInInspector] public string noAdsID;

    
    #endregion


    bool isRewardAdded;

    private void Awake()
    {
        instance = this;

    }
    // Use this for initialization
    void Start ()
    {
       DontDestroyOnLoad(this.gameObject);

        if (enableTestMode)
        {
            bannerID = "ca-app-pub-3940256099942544/6300978111";
            interstitialID = "ca-app-pub-3940256099942544/1033173712";
            rewardedVideoAdsID = "ca-app-pub-3940256099942544/5224354917";
        }

       InitializeAdmob();
       InitializeUnityAds();
	}
    #region Admob
    private void RequestBannerAdmob()
    {
#if ADS_ADMOB
        bannerView = new BannerView(bannerID,AdSize.Banner,AdPosition.Bottom);

        AdRequest request = new AdRequest();

        bannerView.LoadAd(request);
#endif
    }
    private void RequestInterstialAdmob()
    {
#if ADS_ADMOB

        var adRequest = new AdRequest();
        InterstitialAd.Load(interstitialID, adRequest,
    (InterstitialAd ad, LoadAdError error) =>
    {
        // if error is not null, the load request failed.
        if (error != null || ad == null)
        {
            Debug.LogError("interstitial ad failed to load an ad " +
                           "with error : " + error);
            return;
        }

        Debug.Log("Interstitial ad loaded with response : "
                  + ad.GetResponseInfo());

        interstitial = ad;
    });
#endif
    }
    private void RequestRewardedVideoAdAdmob()
    {
        isRewardAdded = false;

#if ADS_ADMOB
        
        if (rewardVideoAd != null)
        {
            rewardVideoAd.Destroy();
            rewardVideoAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardedVideoAdsID, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardVideoAd = ad;
            });
        
#endif
    }

    public void InitializeAdmob()
    {
#if ADS_ADMOB
        //MobileAds.Initialize(appID);
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
        this.RequestInterstialAdmob();
        this.RequestRewardedVideoAdAdmob();
#endif
    }
    public void InitializeBannerAdmob()
    {
#if ADS_ADMOB
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });
        //MobileAds.Initialize(appID);
#endif

        this.RequestBannerAdmob();
    }
    public void ShowBannerAdmob()
    {
        #if ADS_ADMOB
        if(this.bannerView != null)
          this.bannerView.Show();
#endif
    }
    public void DestroyBannerAdmob()
    {
#if ADS_ADMOB
        this.bannerView.Destroy();
#endif
    }
    public void ShowInterstitialAdmob()
    {
#if ADS_ADMOB
        if (interstitial != null && interstitial.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitial.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
        this.RequestInterstialAdmob();

#endif
    }
    public void ShowRewardedVideoAdAdmob()
    {
#if ADS_ADMOB
        //if (rewardVideoAd != null)
        //{
        //    Debug.Log("Rewarded was loaded succesfully!");

        //    rewardVideoAd.Show();
        //}

        const string rewardMsg =
       "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardVideoAd != null && rewardVideoAd.CanShowAd())
        {
            rewardVideoAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                //Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }

        rewardVideoAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");
            GameManager.Instance.ReviveSucceed(true);
            RequestRewardedVideoAdAdmob();

        };

        rewardVideoAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            RequestRewardedVideoAdAdmob();
        };
#endif
    }
#if ADS_ADMOB
    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");
            GameManager.Instance.ReviveSucceed(true);

            // Reload the ad so that we can show another as soon as possible.
            RequestRewardedVideoAdAdmob();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            RequestRewardedVideoAdAdmob();
        };
    }
#endif
    #endregion
    #region UnityAds
    public void InitializeUnityAds()
    {
#if ADS_UNITY
        Monetization.Initialize(gameID, testMode);
        Advertisement.Initialize(gameID, testMode);
#endif
    }
#if ADS_UNITY
    private IEnumerator RequestInterstialUnityAds()
    {
        while (!Monetization.IsReady(videoAdPlacementID))
        {
            yield return new WaitForSeconds(0.25f);
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(videoAdPlacementID) as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show();
        }
    }

    IEnumerator RequestRewardedVideoAdUnityAds()
    {
        while (!Monetization.IsReady(rewardedVideoAdPlacementID))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(rewardedVideoAdPlacementID) as ShowAdPlacementContent;

        if (ad != null)
        {
            ad.Show(AdFinished);
        }

    }
    IEnumerator RequestBannerUnityAds()
    {

        while (!Advertisement.IsReady("banner"))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(bannerPlacementID);

    }
    void AdFinished(UnityEngine.Monetization.ShowResult result)
    {
        if (result == UnityEngine.Monetization.ShowResult.Finished)
        {
            if (isRewardAdded == false)
            {

               int coins = PlayerPrefs.GetInt("Coins") + 100;

               PlayerPrefs.SetInt("Coins",coins);
               PlayerPref.Save();

               isRewardAdded = true;
            }
        }
    }
#endif
    public void ShowBannerUnityAds()
    {
#if ADS_UNITY
        StartCoroutine(RequestBannerUnityAds());
#endif
    }
    public void ShowInterstitialUnityAds()
    {
        #if ADS_UNITY
        StartCoroutine(RequestInterstialUnityAds());
#endif
    }
    public void ShowRewardedVideoUnityAds()
    {
        #if ADS_UNITY
        StartCoroutine(RequestRewardedVideoAdUnityAds());
#endif
    }
#endregion
}
