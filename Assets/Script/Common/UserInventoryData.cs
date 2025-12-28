using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserItemData
{
    public long SerialNumber;

    //스택을 표시를 해줄 예정
    public int ItemId;

    //생성자
    public UserItemData(long serialNumber, int itemId)
    {
        SerialNumber = serialNumber;

        ItemId = itemId;
    }
}

[Serializable]
public class UserInventoryItemDataListWrapper
{
    public List<UserItemData> InventoryItemDataList;
}
public class UserItemStats
{
    public int AttackPower;
    public int Defense;
    public UserItemStats(int attackPower, int defense)
    {
        AttackPower = attackPower;
        Defense = defense;
    }
}
public class UserInventoryData : IUserData
{
    public UserItemData EquippedWeaponData { get; set; }
    public UserItemData EquippedShieldData { get; set; }
    public UserItemData EquippedChestArmorData { get; set; }
    public UserItemData EquippedBootsData { get; set; }
    public UserItemData EquippedGlovesData { get; set; }
    public UserItemData EquippedAccessoryData { get; set; }

    public List<UserItemData> InventoryItemDataList { get; set; } = new List<UserItemData>();
    public Dictionary<long, UserItemStats> EquippedItemDic { get; set; } = new Dictionary<long, UserItemStats>();

    // 시리얼 번호 생성기 (18자리로 오버플로우 방지)
    private long GenerateSN()
    {
        string timePart = DateTime.Now.ToString("yyyyMMddHHmmss");
        string randomPart = UnityEngine.Random.Range(0, 10000).ToString("D4");
        return long.Parse(timePart + randomPart);
    }

    // 효율적인 검색 함수 (타입, 등급 입력)
    private UserItemData GetItem(int type, int grade)
    {
        return InventoryItemDataList.Find(item =>
            (item.ItemId / 10000 == type) &&
            ((item.ItemId / 1000) % 10 == grade));
    }

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");
        InventoryItemDataList.Clear();

        // 1. 종류(Type) 우선 루프
        for (int type = 1; type <= 6; type++)
        {
            for (int grade = 1; grade <= 4; grade++)
            {
                // 검의 종류
                for (int num = 1; num <= 2; num++)
                {
                    int id = (type * 10000) + (grade * 1000) + num;
                    InventoryItemDataList.Add(new UserItemData(GenerateSN(), id));
                }
            }
        }

        // 2. 검색 함수를 사용하여 장착 (인덱스 숫자 몰라도 됨!)
        EquippedWeaponData = GetItem(1, 1); // 1번타입 1등급 (무기)
        EquippedShieldData = GetItem(2, 2); // 2번타입 2등급 (방패)
        EquippedChestArmorData = GetItem(3, 3); // 2번타입 2등급 (방패)
        EquippedGlovesData = GetItem(4, 1); // 번타입 2등급 (방패)
        EquippedBootsData = GetItem(5, 4); // 2번타입 2등급 (방패)
        EquippedAccessoryData = GetItem(6, 1); // 6번타입 3등급 (장신구)
        // 나머지 부위도 기본값 필요하면 여기서 GetItem(타입, 등급)으로 호출

        SetEquippedItemDic();
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            string weaponJson = PlayerPrefs.GetString("EquippedWeaponData");

            if (!string.IsNullOrEmpty(weaponJson))
            {
                EquippedWeaponData = JsonUtility.FromJson<UserItemData>(weaponJson);
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber}ItemId:{ EquippedWeaponData.ItemId}");
            }

            string shieldJson = PlayerPrefs.GetString("EquippedShieldData");
            if (!string.IsNullOrEmpty(shieldJson))
            {
                EquippedShieldData = JsonUtility.FromJson<UserItemData>(shieldJson);
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber}ItemId:{ EquippedShieldData.ItemId}");
            }

            string chestArmorJson = PlayerPrefs.GetString("EquippedChestArmorData");
            if (!string.IsNullOrEmpty(chestArmorJson))
            {
                EquippedChestArmorData = JsonUtility.FromJson<UserItemData>(chestArmorJson);
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber}ItemId:{ EquippedChestArmorData.ItemId}");
            }

            string glovesJson = PlayerPrefs.GetString("EquippedGlovesData");
            if (!string.IsNullOrEmpty(glovesJson))
            {
                EquippedGlovesData = JsonUtility.FromJson<UserItemData>(glovesJson);
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber}ItemId:{ EquippedGlovesData.ItemId}");
            }

            string bootsJson = PlayerPrefs.GetString("EquippedBootsData");
            if (!string.IsNullOrEmpty(bootsJson))
            {
                EquippedBootsData = JsonUtility.FromJson<UserItemData>(bootsJson);
                Logger.Log($"EquippedBootsArmorData: SN:{EquippedBootsData.SerialNumber}ItemId:{EquippedBootsData.ItemId}");
            }

            string accessoryJson = PlayerPrefs.GetString("EquippedAccessoryData");
            if (!string.IsNullOrEmpty(accessoryJson))
            {
                EquippedAccessoryData = JsonUtility.FromJson<UserItemData>(accessoryJson);
                Logger.Log($"EquippedChestArmorData: SN:{EquippedAccessoryData.SerialNumber}ItemId:{ EquippedAccessoryData.ItemId}");
            }

            string inventoryItemDataListJson = PlayerPrefs.GetString("InventoryItemDataList");

            if (!string.IsNullOrEmpty(inventoryItemDataListJson))
            {
                UserInventoryItemDataListWrapper itemDataListWrapper =
                    //그 래퍼 클래스 내에 있는 InventoryItemDataList에 있는 데이터를 UserInventoryData의 InventroyItemDataList 변수에 대입
                    JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataListJson);
                InventoryItemDataList = itemDataListWrapper.InventoryItemDataList;

                Logger.Log("InventoryItemDataList");

                foreach (var item in InventoryItemDataList)
                {
                    Logger.Log($"SerialNumber:{item.SerialNumber} ItemID:{item.ItemId}");
                }
            }
            SetEquippedItemDic();

            result = true;
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }

        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");

        bool result = false;

        try
        {
            UserInventoryItemDataListWrapper inventoryItemDataListWrapper = new UserInventoryItemDataListWrapper();

            inventoryItemDataListWrapper.InventoryItemDataList = InventoryItemDataList;

            //이 데이터를 JsonUtility클래스를 이용해서 스트링으로 변환
            string inventoryItemDataListJson = JsonUtility.ToJson(inventoryItemDataListWrapper);

            //이 스트링 값을 플레이어 프리펩에 저장
            PlayerPrefs.SetString("InventoryItemDataList", inventoryItemDataListJson);

            Logger.Log("InventoryItemDataList");

            foreach (var item in InventoryItemDataList)
            {
                Logger.Log($"SerialNumber:{item.SerialNumber} ItemID:{item.ItemId}");
            }

            // 2. ⭐ 장착 아이템 데이터 개별 저장 (추가된 부분)
            // 이 부분이 있어야 LoadData에서 개별적으로 부를 수 있습니다.
            if (EquippedWeaponData != null) PlayerPrefs.SetString("EquippedWeaponData", JsonUtility.ToJson(EquippedWeaponData));
            if (EquippedShieldData != null) PlayerPrefs.SetString("EquippedShieldData", JsonUtility.ToJson(EquippedShieldData));
            if (EquippedChestArmorData != null) PlayerPrefs.SetString("EquippedChestArmorData", JsonUtility.ToJson(EquippedChestArmorData));
            if (EquippedGlovesData != null) PlayerPrefs.SetString("EquippedGlovesData", JsonUtility.ToJson(EquippedGlovesData));
            if (EquippedBootsData != null) PlayerPrefs.SetString("EquippedBootsData", JsonUtility.ToJson(EquippedBootsData));
            if (EquippedAccessoryData != null) PlayerPrefs.SetString("EquippedAccessoryData", JsonUtility.ToJson(EquippedAccessoryData));

            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }

        return result;
    }

    public void SetEquippedItemDic()
    {
        if (EquippedWeaponData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedWeaponData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedWeaponData.SerialNumber,
                    new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedShieldData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedShieldData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedShieldData.SerialNumber,
                    new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedChestArmorData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedChestArmorData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedChestArmorData.SerialNumber,
                    new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedGlovesData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedGlovesData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedGlovesData.SerialNumber,
                    new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedBootsData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedBootsData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedBootsData.SerialNumber,
                    new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedAccessoryData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedAccessoryData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedAccessoryData.SerialNumber,
                    new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
    }

    public bool IsEquipped(long serialNumber)
    {
        return EquippedItemDic.ContainsKey(serialNumber);
    }

    public void EquipItem(long serialNumber, int itemId)
    {
        var itemData = DataTableManager.Instance.GetItemData(itemId);
        if(itemData == null)
        {
            Logger.LogError($"Item data does not exit, ItemId: {itemId}");
            return;
        }
        var itemType = (ItemType)(itemId / 10000);
        switch(itemType)
        {
            case ItemType.Weapon:
                if(EquippedWeaponData != null)
                {
                    EquippedItemDic.Remove(EquippedWeaponData.SerialNumber);
                    EquippedWeaponData = null;
                }
                EquippedWeaponData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Shield:
                if (EquippedShieldData != null)
                {
                    EquippedItemDic.Remove(EquippedShieldData.SerialNumber);
                    EquippedShieldData = null;
                }
                EquippedShieldData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.ChestArmor:
                if (EquippedChestArmorData != null)
                {
                    EquippedItemDic.Remove(EquippedChestArmorData.SerialNumber);
                    EquippedChestArmorData = null;
                }
                EquippedChestArmorData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Gloves:
                if (EquippedGlovesData != null)
                {
                    EquippedItemDic.Remove(EquippedGlovesData.SerialNumber);
                    EquippedGlovesData = null;
                }
                EquippedGlovesData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Boots:
                if (EquippedBootsData != null)
                {
                    EquippedItemDic.Remove(EquippedBootsData.SerialNumber);
                    EquippedBootsData = null;
                }
                EquippedBootsData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Accessory:
                if (EquippedAccessoryData != null)
                {
                    EquippedItemDic.Remove(EquippedAccessoryData.SerialNumber);
                    EquippedAccessoryData = null;
                }
                EquippedAccessoryData = new UserItemData(serialNumber, itemId);
                break;
            default:
                break;
        }

        EquippedItemDic.Add(serialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
    }

    //탈착함수
    public void UnequipItem(long serialNumber, int itemId)
    {
        //마찬가지로 아이템 종류값을 추출하고 
        var itemType = (ItemType)(itemId / 10000);
        //아이템 종류에 따라 해당 변수를 초기화해 줌.
        switch (itemType)
        {
            case ItemType.Weapon:
                EquippedWeaponData = null;
                break;
            case ItemType.Shield:
                EquippedShieldData = null;
                break;
            case ItemType.ChestArmor:
                EquippedChestArmorData = null;
                break;
            case ItemType.Gloves:
                EquippedGlovesData = null;
                break;
            case ItemType.Boots:
                EquippedBootsData = null;
                break;
            case ItemType.Accessory:
                EquippedAccessoryData = null;
                break;
            default:
                break;
        }
        //딕셔너리에서 삭제
        EquippedItemDic.Remove(serialNumber);
    }

    public UserItemStats GetUserTotalItemStats()
    {
        var totalAttackPower = 0;
        var totalDefense = 0;

        foreach(var item in EquippedItemDic)
        {
            totalAttackPower += item.Value.AttackPower;
            totalDefense += item.Value.Defense;
        }

        return new UserItemStats(totalAttackPower, totalDefense);
    }
}
