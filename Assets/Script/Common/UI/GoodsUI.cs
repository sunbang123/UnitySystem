using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoodsUI : MonoBehaviour
{
    public TextMeshProUGUI GoldAmountText;
    public TextMeshProUGUI GemAmountText;

    public void SetValues()
    {
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();

        if (userGoodData == null)
        {
            Logger.Log("No user goods data");
            return;
        }

        //재화데이터가 정상이면 보석과 골드 수량표시(N0; 1000단위 쉼표)
        GoldAmountText.text = userGoodData.Gold.ToString("N0");
        GemAmountText.text = userGoodData.Gem.ToString("N0");
    }

}