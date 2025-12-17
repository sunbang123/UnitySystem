using Gpm.Ui;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

//InventoryItemSlot 인스턴스 생성을 위해 필요한 데이터 클래스
//InfiniteScrollData 
//상속 : 인피니티 스클롤 컴포넌트를 사용하여 스크롤 아이템을 생성하기 위해
//(그냥 써보자)
public class InventoryItemSlotData : InfiniteScrollData
{
    //필요한 데이터는 유저 아이템 데이터와 동일하게 시리얼 넘버와 아이템 아이디
    public long SerialNumber;
    public int ItemId;
}

//InfiniteScrollItem 상속 : 
//InfiniteScroll클래스에서 스크롤 데이터와 동일하게 
//스크롤 아이템 들이 내부적으로 InfiniteScrollItem인스턴스로 관리되기 때문
//(그냥 써보자)
public class InventoryItemSlot : InfiniteScrollItem
{
    public Image ItemGradeBg;
    public Image itemicon;
    private InventoryItemSlotData m_InventoryItemSlotData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);

                            // infiniteScrollData를 InventoryItemSlotData 형식으로 형변환한것
        m_InventoryItemSlotData = scrollData as InventoryItemSlotData;

        if (m_InventoryItemSlotData == null)
        {
            Logger.LogError("m_InventoryItemSlotData is invalid.");
            return;
        }

        var itemGrade = (ItemGrade)((m_InventoryItemSlotData.ItemId / 1000) % 10);

        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");

        if(gradeBgTexture != null)
        {
            ItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }

        StringBuilder sb = new StringBuilder(m_InventoryItemSlotData.ItemId.ToString());
        sb[1] = '1';
        var itemIconName = sb.ToString();

        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        if(itemIconTexture != null)
        {
            itemicon.sprite =
                Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }
}