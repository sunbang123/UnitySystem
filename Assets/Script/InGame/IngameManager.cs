using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController InGameUIController { get; private set; }

    protected override void Init()
    {
        m_IsDestroyOnLoad = true; //인게임매니저는 인게임 씬을 벗어나면 삭제되어야하므로 true

        base.Init();

    }

    private void Start()
    {
        //씬 내에서 InGameUIController 스크립트를 가지고 있는 오브젝트를 찾아서 대입 
        InGameUIController = FindObjectOfType<InGameUIController>();
        if (!InGameUIController)
        {
            Logger.LogError("InGameUIController does not exist.");
            return;
        }

        InGameUIController.Init();
    }
}