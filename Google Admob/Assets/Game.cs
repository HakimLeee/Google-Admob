using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public void OnBannerShowBtnClick()
    {
        GoogleAdmobManager.Instance.ShowBanner();
    }

    public void OnBannerHideBtnClick()
    {
        GoogleAdmobManager.Instance.HideBanner();
    }

    public void OnInterstitialBtnClick()
    {
        GoogleAdmobManager.Instance.ShowInterstitial();
    }

    public void OnRewardVedioBtnClick()
    {
        GoogleAdmobManager.Instance.ShowRewardVedio();
    }
}