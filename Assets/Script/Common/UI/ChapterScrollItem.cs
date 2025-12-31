using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//스크롤뷰의 스크롤 아이템에 대해서는 해당 스크롤 아이템에 대한
//전용 데이터 클래스가 필요함.
public class ChapterScrollItemData : InfiniteScrollData
{
    public int ChapterNo;
}

public class ChapterScrollItem : InfiniteScrollItem
{
    public GameObject CurrChapter; //챕터오브젝트 변수 선언
    public RawImage CurrChapterBg; //챕터이미지 컴퍼넌트도 선언
    //챕터가 해금되지 않았을 때 표시해줄 UI컴포넌트들도 선언
    public Image Dim;
    public Image LockIcon;
    public Image Round;
    //아직 존재하지 않는 챕터(이후 업데이트에서 추가 될 챕터)에 대한 UI
    public ParticleSystem ComingSoonFX;
    public TextMeshProUGUI ComingSoonTxt;

    //ChapterScrollItemData를 받아서 담아둘 변수도 선언
    private ChapterScrollItemData m_ChapterScrollItemData;

    //UI 처리를 해주기 위해 호출 되는 UpdateData 함수 오버라이딩
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //매개변수로 받은 스크롤 데이터를 받아 줌
        m_ChapterScrollItemData = scrollData as ChapterScrollItemData;

        if (m_ChapterScrollItemData == null)
        {
            Logger.LogError("Invalid ChapterScrollItemData.");
            return;
        }
        //만약 표시해야 할 챕터 넘버가 글로벌 정의에 있는 MAX_CHAPTER의 값
        //즉, 게임 내 존재하는 최대 챕터보다 크다면
        if (m_ChapterScrollItemData.ChapterNo > GlobalDefine.MAX_CHAPTER)
        {
            //챕터포시UI는 비활성화 처리하고 
            // ComingSoon 활성화
            CurrChapter.SetActive(false);
            ComingSoonFX.gameObject.SetActive(true);
            ComingSoonTxt.gameObject.SetActive(true);
        }
        else //그렇지 않으면 본격적으로 챕터UI표시
        {
            CurrChapter.SetActive(true);
            ComingSoonFX.gameObject.SetActive(false);
            ComingSoonTxt.gameObject.SetActive(false);
            //유저플레이데이터를 가져옮
            var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
            if (userPlayData != null)
            {
                //현재 최대로 클리어한 챕터와 비교하여 챕터의 해금 여부를 판단
                var isLocked = m_ChapterScrollItemData.ChapterNo > userPlayData.MaxClearedChapter + 1;
                //그리고 해금 여부에 따라 이미지 컴포넌들을 처리해 줌 
                Dim.gameObject.SetActive(isLocked);//(디밍(광원의 밝기조정)은 해금되 않았으면 활성화)
                LockIcon.gameObject.SetActive(isLocked); //잠금 아이콘도 해금되 않았으면 활성화
                //테두리 이미지는 해금 되었으면 밝게 그렇지 않으면 어둡게 처리
                Round.color = isLocked ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;
            }
            //해당 챕터 넘버에 맞는 배경 이미지를 로딩하여 세팅해 줌.
            var bgTexture = Resources.Load
            ($"ChapterBG/Background_{m_ChapterScrollItemData.ChapterNo.ToString("D3")}") as Texture2D;
            if (bgTexture != null)
            {
                CurrChapterBg.texture = bgTexture;
            }
        }
    }
}