using UnityEngine;

public class AdForCoinsButton : MonoBehaviour {

    private AdManager m_AdManager;
    public int amountOfCoins;

    private void Awake()
    {
        m_AdManager = FindObjectOfType<AdManager>();
    }

    public void ShowAd()
    {
        m_AdManager.rewardToGive = "coins";
        m_AdManager.amountToGive = amountOfCoins;

        m_AdManager.ShowRewardedAd();
    }

}
