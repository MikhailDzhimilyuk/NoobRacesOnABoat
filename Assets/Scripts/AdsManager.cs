using UnityEngine;
using System.Collections;
using YG;
public class AdsManager : MonoBehaviour
{
    [SerializeField] private Saving_Manager_YSDK saving_Manager_YSDK;
    [SerializeField] private ShopController shopController;
    private bool canReward = false;
    public bool canShowFullscreenAd = false;

    private void OnEnable() 
    {
        YandexGame.OpenVideoEvent += CheckAd;
        YandexGame.CloseVideoEvent += Rewarded;
        YandexGame.CloseFullAdEvent += SetFalseCanShowFullscreenAd;
        YandexGame.ErrorFullAdEvent += SetFalseCanShowFullscreenAd;
    }

    private void OnDisable() 
    {
        YandexGame.OpenVideoEvent -= CheckAd;
        YandexGame.CloseVideoEvent -= Rewarded;
        YandexGame.CloseFullAdEvent -= SetFalseCanShowFullscreenAd;
        YandexGame.ErrorFullAdEvent -= SetFalseCanShowFullscreenAd;
    }

    public void Rewarded()
    {
        if (canReward)
        {
            shopController.ChangeBalance(200);
            saving_Manager_YSDK.SaveData();
        }

        canReward = false;
        StopAllCoroutines();
    }

    public void OpenRewardAd()
    {
        YandexGame.RewVideoShow(0);
    }

    public void ShowFullscreenAd()
    {
        if (canShowFullscreenAd)
        {
            YandexGame.FullscreenShow();
        }
    }

    public void SetFalseCanShowFullscreenAd()
    {
        canShowFullscreenAd = false;
    }

    public void SetCanShowFullscreenAd()
    {
        canShowFullscreenAd = true;
    }

    private void CheckAd()
    {
        StartCoroutine(CheckAdWasShowed());
    }

    private IEnumerator CheckAdWasShowed()
    {
        yield return new WaitForSecondsRealtime(10);
        canReward = true;
    }
}