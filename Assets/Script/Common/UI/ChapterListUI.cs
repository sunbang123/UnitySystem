using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterListUI : BaseUI
{
    public InfiniteScroll ChapterScrollList;

    public GameObject SelectedChapterName;
    public TextMeshProUGUI SelectedChapterNameTxt;
    public Button SelectBtn;

    private int SelectedChapter;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if (userPlayData == null)
        {
            return;
        }
        SelectedChapter = userPlayData.SelectedChapter;

        SetSelectedChapter();
        SetChapterScrollList();

        ChapterScrollList.MoveTo(SelectedChapter - 1, InfiniteScroll.MoveToType.MOVE_TO_CENTER);

        ChapterScrollList.OnSnap = (currentSnappedIndex) =>
        {
            var chapterListUI = UIManager.Instance.GetActiveUI<ChapterListUI>() as ChapterListUI;
            if (chapterListUI != null)
            {
                chapterListUI.OnSnap(currentSnappedIndex + 1);
            }
        };
    }

    private void SetSelectedChapter()
    {
        if (SelectedChapter <= GlobalDefine.MAX_CHAPTER)
        {
            SelectedChapterName.SetActive(true);
            SelectBtn.gameObject.SetActive(true);

            var itemData = DataTableManager.Instance.GetChapterData(SelectedChapter);
            if (itemData != null)
            {
                SelectedChapterNameTxt.text = itemData.ChapterName;
            }
        }
        else
        {
            SelectedChapterName.SetActive(false);
            SelectBtn.gameObject.SetActive(false);
        }
    }

    private void SetChapterScrollList()
    {
        ChapterScrollList.Clear();

        for (int i = 1; i <= GlobalDefine.MAX_CHAPTER + 1; i++)
        {
            var chapterItemData = new ChapterScrollItemData();
            chapterItemData.ChapterNo = i; //i +1
            ChapterScrollList.InsertData(chapterItemData);
        }
    }

    public void OnSnap(int selectedChapter)
    {
        //���� ������ é�͸� �Ű������� ���� é�ͷ� �������ְ� SetSelectedChapter()�Լ� �ٽ� ȣ��
        SelectedChapter = selectedChapter;
        SetSelectedChapter();
    }
    //�����ϱ� ��ư�� ������ �� ������ �ش� é�͸� ������ �����ϴ� ó���� �ϴ� �Լ�
    public void OnClickSelect()
    {

        //UserPlayData�� �����ͼ�
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if (userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }
        //������ ���� ������ é��(�ر��� é��)���
        if (SelectedChapter <= userPlayData.MaxClearedChapter + 1)
        {
            //�÷��� �����͸� �������ְ� 
            //�κ� �ִ� ���� ������ é��UI�� �������ݴϴ�.
            userPlayData.SelectedChapter = SelectedChapter;
            LobbyManager.Instance.LobbyUIController.SetCurrChapter();
            CloseUI(); //���ӻ� �ڿ������� �帧�� ���� �� ȭ���� �ݾ���.
        }
    }
}
