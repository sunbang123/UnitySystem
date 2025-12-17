using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserItemData
{
    public long SerialNumber;

    //스택을 표시를 해줄 예정
    public int ItemID;

    //생성자
    public UserItemData(long serialNumber, int itemID)
    {
        SerialNumber = serialNumber;

        ItemID = itemID;
    }
}

[Serializable]
public class UserInventoryItemDataListWrapper
{
    public List<UserItemData> InventoryItemDataList;
}

public class UserInventoryData : IUserData
{
    public List<UserItemData> InventoryDataList { get; set; } = new List<UserItemData>();

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");

        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11001));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11002));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 21001));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 21002));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 31001));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 31002));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 41001));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 41002));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 51001));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 51002));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 61001));
        InventoryDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")
            + UnityEngine.Random.Range(0, 9999).ToString("D4")), 61002));
    }

    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;

        try
        {
            string inventoryItemDataListJson = PlayerPrefs.GetString("InventoryItemDataList");

            if (!string.IsNullOrEmpty(inventoryItemDataListJson))
            {
                UserInventoryItemDataListWrapper itemDataListWrapper =
                    //그 래퍼 클래스 내에 있는 InventoryItemDataList에 있는 데이터를 UserInventoryData의 InventroyItemDataList 변수에 대입
                    JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataListJson);
                InventoryDataList = itemDataListWrapper.InventoryItemDataList;

                Logger.Log("InventoryItemDataList");

                foreach (var item in InventoryDataList)
                {
                    Logger.Log($"SerialNumber:{item.SerialNumber} ItemID:{item.ItemID}");
                }
            }

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

            inventoryItemDataListWrapper.InventoryItemDataList = InventoryDataList;

            //이 데이터를 JsonUtility클래스를 이용해서 스트링으로 변환
            string inventoryItemDataListJson = JsonUtility.ToJson(inventoryItemDataListWrapper);

            //이 스트링 값을 플레이어 프리펩에 저장
            PlayerPrefs.SetString("InventoryItemDataList", inventoryItemDataListJson);

            Logger.Log("InventoryItemDataList");

            foreach (var item in InventoryDataList)
            {
                Logger.Log($"SerialNumber:{item.SerialNumber} ItemID:{item.ItemID}");
            }

            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }

        return result;
    }

}
