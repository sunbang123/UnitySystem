using Gpm.Ui;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;


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


}