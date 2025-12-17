using UnityEngine;
using Gpm.Ui;
using TMPro;

public class InventoryUI : BaseUI
{
    public InfiniteScroll InventoryScrollList;
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SetInventory();
    }

    private void SetInventory()
    {
        InventoryScrollList.Clear();

        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();

        if(userInventoryData != null)
        {
            foreach(var itemData in userInventoryData.InventoryDataList)
            {
                var itemSlotData = new InventoryItemSlotData();
                itemSlotData.SerialNumber = itemData.SerialNumber;
                itemSlotData.ItemId = itemData.ItemId;
                InventoryScrollList.InsertData(itemSlotData);
            }
        }
    }
}
