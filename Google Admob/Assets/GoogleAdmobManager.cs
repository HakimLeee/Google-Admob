using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAdmobManager : MonoBehaviour
{
    private static GoogleAdmobManager instance;
    public static GoogleAdmobManager Instance
    {
        get
        {
            return instance;
        }
    }

    public AdPosition m_bannerPosition = AdPosition.Bottom;

    public string m_bannerID = "";

    public string m_interstitialID = "";

    public string m_rewardVedioID = "";

    private BannerView m_bannerView = null;

    private int m_failedLoadBannerViewCount = 0;

    private InterstitialAd m_interstitial = null;

    public int m_probabilityShowInterstitial = 75;

    private int m_failedLoadInterstitialCount = 0;

    private int m_failedLoadRewardVedioCount = 0;

    public int m_maxFailedLoadCount = 3;

    public bool m_bTest = false;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (!string.IsNullOrEmpty(m_bannerID))
        {
            LoadBanner();
        }

        if (!string.IsNullOrEmpty(m_interstitialID))
        {
            LoadInterstitial();
        }

        if (!string.IsNullOrEmpty(m_rewardVedioID))
        {
            LoadRewadVedio();
        }
    }

    private void LoadBanner()
    {
        DestroyBanner();

        // Create a 320x50 banner at the top of the screen.
        m_bannerView = new BannerView(m_bannerID, AdSize.Banner, m_bannerPosition);
        // Create an empty ad request.
        AdRequest request = null;
        if (m_bTest)
        {
            request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        }
        else
        {
            request = new AdRequest.Builder().Build();
        }

        m_bannerView.OnAdLoaded += OnBannerViewLoad;

        m_bannerView.OnAdFailedToLoad += OnBannerFailedToLoad;

        m_bannerView.OnAdClosed += OnBannerClosed;

        // Load the banner with the request.
        m_bannerView.LoadAd(request);

        Debug.Log("Start Load Banner View.");
    }

    public void ShowBanner()
    {
        if (m_bannerView != null)
        {
            m_bannerView.Show();

            Debug.Log("Show Banner View.");
        }
    }

    public void HideBanner()
    {
        if (m_bannerView != null)
        {
            m_bannerView.Hide();

            Debug.Log("Hide Banner View.");
        }
    }

    public void DestroyBanner()
    {
        if (m_bannerView != null)
        {
            m_bannerView.Destroy();
            m_bannerView = null;

            Debug.Log("Destroy Banner View.");
        }
    }

    private void OnBannerViewLoad(object sender, EventArgs args)
    {
        Debug.Log("Load Banner View Finished.");
        m_failedLoadBannerViewCount = 0;
    }

    private void OnBannerFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        m_failedLoadBannerViewCount = m_failedLoadBannerViewCount + 1;
        Debug.Log("Load Banner View Failed. message = " + args.Message + " m_failedLoadBannerViewCount = " + m_failedLoadBannerViewCount);
        if (m_failedLoadBannerViewCount < m_maxFailedLoadCount)
        {
            LoadBanner();
        }
    }

    private void OnBannerClosed(object sender, EventArgs args)
    {
        Debug.Log("Closed Banner View.");
        DestroyBanner();
        LoadBanner();
    }

    private void LoadInterstitial()
    {
        DestroyInterstitial();

        // Initialize an InterstitialAd.
        m_interstitial = new InterstitialAd(m_interstitialID);
        // Create an empty ad request.
        AdRequest request = null;
        if (m_bTest)
        {
            request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        }
        else
        {
            request = new AdRequest.Builder().Build();
        }

        m_interstitial.OnAdLoaded += OnInterstitialLoad;

        m_interstitial.OnAdFailedToLoad += OnInterstitialFailedToLoad;

        m_interstitial.OnAdClosed += OnInterstitialClosed;

        // Load the interstitial with the request.
        m_interstitial.LoadAd(request);

        Debug.Log("Start Load Interstitial.");
    }

    public void ShowInterstitial()
    {
        if (m_interstitial != null && m_interstitial.IsLoaded() && CanShowInterstitial())
        {
            m_interstitial.Show();

            Debug.Log("Show Interstitial.");
        }
    }

    private void DestroyInterstitial()
    {
        if (m_interstitial != null)
        {
            m_interstitial.Destroy();
            m_interstitial = null;

            Debug.Log("Destroy Interstitial.");
        }
    }

    public bool CanShowInterstitial()
    {
        int index = UnityEngine.Random.Range(0, 100);

        return index <= m_probabilityShowInterstitial;
    }

    private void OnInterstitialLoad(object sender, EventArgs args)
    {
        Debug.Log("Load OnInterstitialLoad Finished.");
        m_failedLoadBannerViewCount = 0;
    }

    private void OnInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        m_failedLoadInterstitialCount = m_failedLoadInterstitialCount + 1;
        Debug.Log("Load OnInterstitialLoad Failed. message = " + args.Message + " m_failedLoadInterstitialCount = " + m_failedLoadInterstitialCount);
        if (m_failedLoadInterstitialCount < m_maxFailedLoadCount)
        {
            LoadInterstitial();
        }
    }

    private void OnInterstitialClosed(object sender, EventArgs args)
    {
        Debug.Log("Closed OnInterstitialLoad.");
        DestroyInterstitial();
        LoadInterstitial();
    }

    private void LoadRewadVedio()
    {
        // Create an empty ad request.
        AdRequest request = null;
        if (m_bTest)
        {
            request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        }
        else
        {
            request = new AdRequest.Builder().Build();
        }

        RewardBasedVideoAd.Instance.OnAdLoaded -= OnRewardVideoLoad;

        RewardBasedVideoAd.Instance.OnAdFailedToLoad -= OnRewardVideoFailedToLoad;

        RewardBasedVideoAd.Instance.OnAdClosed -= OnRewardVideoClosed;

        RewardBasedVideoAd.Instance.OnAdRewarded -= OnRewardVideoRewarded;

        RewardBasedVideoAd.Instance.OnAdLoaded += OnRewardVideoLoad;

        RewardBasedVideoAd.Instance.OnAdFailedToLoad += OnRewardVideoFailedToLoad;

        RewardBasedVideoAd.Instance.OnAdClosed += OnRewardVideoClosed;

        RewardBasedVideoAd.Instance.OnAdRewarded += OnRewardVideoRewarded;

        // Initialize an RewardBasedVideoAd.
        RewardBasedVideoAd.Instance.LoadAd(request, m_rewardVedioID);

        Debug.Log("Start Load Reward Vedio.");
    }

    public void ShowRewardVedio()
    {
        if (RewardBasedVideoAd.Instance.IsLoaded())
        {
            RewardBasedVideoAd.Instance.Show();

            Debug.Log("Show Reward Vedio");
        }
    }

    private void OnRewardVideoLoad(object sender, EventArgs args)
    {
        Debug.Log("Load RewardVideo Finished.");
        m_failedLoadBannerViewCount = 0;
    }

    private void OnRewardVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        m_failedLoadRewardVedioCount = m_failedLoadRewardVedioCount + 1;
        Debug.Log("Load RewardVideo Failed. message = " + args.Message + " m_failedLoadRewardVedioCount = " + m_failedLoadRewardVedioCount);
        if (m_failedLoadRewardVedioCount < m_maxFailedLoadCount)
        {
            LoadRewadVedio();
        }
    }

    private void OnRewardVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("Closed RewardVideo.");
        LoadRewadVedio();
    }

    private void OnRewardVideoRewarded(object sender, Reward args)
    {
        Debug.Log("Rewarded. Type = " + args.Type + " Amount = " + args.Amount);
    }

    private void OnDestroy()
    {
        DestroyBanner();

        DestroyInterstitial();

        Debug.Log("On Game Destroy.");
    }
}