using System;
using UnityEngine;
public class LobbyUIController : MonoBehaviour
{
    public void Init()
    {

    }

    //로비씬에서 설정 버튼을 누르면 SettingsUI가 열리는 함수
    // 설정 버튼을 눌렀을 때 처리
    public void OnClickSettingsBtn()
    {
        Logger.Log($"{GetType()}::OnClickSettingsBtn");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }

    public void OnClickProfileBtn()
    {
        Logger.Log($"{GetType()}::OnClickProfileBtn");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }
}
