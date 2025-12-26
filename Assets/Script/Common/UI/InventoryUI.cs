using UnityEngine;
using Gpm.Ui;
using TMPro;

public enum InventorySortType
{
    ItemGrade, // 등급
    ItemType, // 종류
}

public class InventoryUI : BaseUI
{
    public EquippedItemSlot WeaponSlot;
    public EquippedItemSlot ShieldSlot;
    public EquippedItemSlot ChestArmorSlot;
    public EquippedItemSlot BootsSlot;
    public EquippedItemSlot GlovesSlot;
    public EquippedItemSlot AccessorySlot;
    public EquippedItemSlot EquippedGloves;

    public InfiniteScroll InventoryScrollList;
    public TextMeshProUGUI SortBtnTxt;

    public TextMeshProUGUI AttackPowerAmountTxt;
    public TextMeshProUGUI DefenseAmountTxt;

    private InventorySortType m_InventorySortType = InventorySortType.ItemGrade;
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SetUserStats();
        SetEquippedItems();
        SetInventory();
        SortInventory();
    }
    
    private void SetUserStats()
    {
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist.");
            return;
        }

        var userTotalItemStats = userInventoryData.GetUserTotalItemStats();
        AttackPowerAmountTxt.text = $"+{userTotalItemStats.AttackPower.ToString("N0")}";
        DefenseAmountTxt.text = $"+{userTotalItemStats.Defense.ToString("N0")}";
    }

    private void SetEquippedItems()
    {
        //UserInventoryData를 가져온다
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        //데이터가 null이면 에러로그
        if (userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist.");
            return;
        }

        //null 아니면 SetItem, null 이면 ClearItem 실행
        if (userInventoryData.EquippedWeaponData != null)
        {
            WeaponSlot.SetItem(userInventoryData.EquippedWeaponData);
        }
        else
        {
            WeaponSlot.ClearItem();
        }

        if (userInventoryData.EquippedShieldData != null)
        {
            ShieldSlot.SetItem(userInventoryData.EquippedShieldData);
        }
        else
        {
            ShieldSlot.ClearItem();
        }

        if (userInventoryData.EquippedChestArmorData != null)
        {
            ChestArmorSlot.SetItem(userInventoryData.EquippedChestArmorData);
        }
        else
        {
            ChestArmorSlot.ClearItem();
        }

        if (userInventoryData.EquippedBootsData != null)
        {
            BootsSlot.SetItem(userInventoryData.EquippedBootsData);
        }
        else
        {
            BootsSlot.ClearItem();
        }

        if (userInventoryData.EquippedGlovesData != null)
        {
            GlovesSlot.SetItem(userInventoryData.EquippedGlovesData);
        }
        else
        {
            GlovesSlot.ClearItem();
        }

        if (userInventoryData.EquippedAccessoryData != null)
        {
            AccessorySlot.SetItem(userInventoryData.EquippedAccessoryData);
        }
        else
        {
            AccessorySlot.ClearItem();
        }
    }

    private void SetInventory()
    {
        InventoryScrollList.Clear();

        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();

        if(userInventoryData != null)
        {
            foreach(var itemData in userInventoryData.InventoryItemDataList)
            {
                if(userInventoryData.IsEquipped(itemData.SerialNumber))
                {
                    continue;
                }

                var itemSlotData = new InventoryItemSlotData();
                itemSlotData.SerialNumber = itemData.SerialNumber;
                itemSlotData.ItemId = itemData.ItemId;
                InventoryScrollList.InsertData(itemSlotData);
            }
        }
    }

    private void SortInventory()
    {
        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:

                SortBtnTxt.text = "GRADE";

                InventoryScrollList.SortDataList((a, b) =>
                {
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;

                    int compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);

                    if (compareResult == 0)
                    {
                        var itemAIdStr = itemA.ItemId.ToString();
                        var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                        var itemBIdStr = itemB.ItemId.ToString();
                        var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);

                        compareResult = itemAComp.CompareTo(itemBComp);
                    }
                    return compareResult;
                });
                break;

            case InventorySortType.ItemType:

                SortBtnTxt.text = "TYPE";

                InventoryScrollList.SortDataList((a, b) =>
                {
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;

                    var itemAIdStr = itemA.ItemId.ToString();
                    var itemAComp = itemAIdStr.Substring(0, 1) + itemAIdStr.Substring(2, 3);

                    var itemBIdStr = itemB.ItemId.ToString();
                    var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3);

                    int compareResult = itemAComp.CompareTo(itemBComp);

                    if (compareResult == 0)
                    {
                        compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    }
                    return compareResult;
                });
                break;
            default:
                break;
        }
        InventoryScrollList.UpdateAllData();
    }

    // 인벤토리 정렬조건을 다른 정렬조건으로 정렬해주는 기능~
    public void OnClickSortBtn()
    {
        switch(m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                m_InventorySortType = InventorySortType.ItemType;
                break;
            case InventorySortType.ItemType:
                m_InventorySortType = InventorySortType.ItemGrade;
                break;
            default:
                break;
        }

        SortInventory();
    }
    //아이템 장착을 한 후 UI처리에 대한 함수를 먼저 작성하겠음.
    public void OnEquipItem(int itemId)
    {
        //UserInventoryData를 가져옮
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();

        if (userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist.");
            return;
        }
        //아이템 종류에 따른 분기 처리
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            case ItemType.Weapon:
                //무기를 장착하는 상황이라면 무기 슬룻을 세팅해 줌
                WeaponSlot.SetItem(userInventoryData.EquippedWeaponData);
                break;
            case ItemType.Shield:
                ShieldSlot.SetItem(userInventoryData.EquippedShieldData);
                break;
            case ItemType.ChestArmor:
                ChestArmorSlot.SetItem(userInventoryData.EquippedChestArmorData);
                break;
            case ItemType.Gloves:
                GlovesSlot.SetItem(userInventoryData.EquippedGlovesData);
                break;
            case ItemType.Boots:
                BootsSlot.SetItem(userInventoryData.EquippedBootsData);
                break;
            case ItemType.Accessory:
                AccessorySlot.SetItem(userInventoryData.EquippedAccessoryData);
                break;
            default:
                break;
        }
        SetUserStats();
        SetInventory(); //인벤토리를 다시 세팅하고
        SortInventory(); //정렬까지 다시 해주겠음.
    }

    //탈착 후 UI 처리에 대한 함수도 작성
    public void OnUnequipItem(int itemId)
    {
        //아이템 종류 추출하고
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            //해당 슬롯 초기화
            case ItemType.Weapon:
                WeaponSlot.ClearItem();
                break;
            case ItemType.Shield:
                ShieldSlot.ClearItem();
                break;
            case ItemType.ChestArmor:
                ChestArmorSlot.ClearItem();
                break;
            case ItemType.Gloves:
                GlovesSlot.ClearItem();
                break;
            case ItemType.Boots:
                BootsSlot.ClearItem();
                break;
            case ItemType.Accessory:
                AccessorySlot.ClearItem();
                break;
            default:
                break;
        }
        SetUserStats();
        SetInventory();
        SortInventory();
    }
}