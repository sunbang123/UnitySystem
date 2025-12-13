using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    // 데이터 테이터 파일들이 들어있는 경로를 string값으로 설정
    private const string DATA_PATH = "DataTable";

    // SingletonBehaviour의 Init 함수 오버라이딩
    protected override void Init()
    {
        base.Init();

        LoadChapterDataTable();
        LoadItemDataTable();
        //아이템 데이터 컨테이너에서 특정 아이템 아이디에 데이터를 찾는 함수
    }

    #region CHAPTER_DATA
    // 챕터 데이터 테이블 파일명을 갖는 String 변수
    private const string CHAPTER_DATA_TABLE = "ChapterDataTable";
    // 모든 챕터 데이터를 저장할 수 있는 컨테이너 즉, 자료구조를 선언
    private List<ChapterData> ChapterDataTable = new List<ChapterData>();

    // 챕터 데이터 테이블을 로드하는 함수
    private void LoadChapterDataTable()
    {
        // public static List<Dictionary<string, object>> Read(string file)
        // return list;
        // CSVReader.Read()는 리스트를 반환하는 함수임.
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE}");
        // var타입으로 한 이유는 복잡한 타입을 신경쓰지 않으려고. 선언과 동시에 초기화해야함.

        // 테이블을 순회하면서 각 데이터를 ChapterData인스턴스로 만들어서 ChapterDataTable 컨테이너에 넣어줌.
        foreach (var data in parsedDataTable)
        {
            var chapterData = new ChapterData
            {
                // 오브젝트 타입이라 지정된 개체 값을 32비트 부호 있는 정수로 변환
                ChapterNo = Convert.ToInt32(data["chapter_no"]),
                TotalStages = Convert.ToInt32(data["total_stages"]),
                ChapterRewardGem = Convert.ToInt32(data["chapter_reward_gem"]),
                ChapterRewardGold = Convert.ToInt32(data["chapter_reward_gold"]),
            };
            ChapterDataTable.Add(chapterData);
        }
    }

    // 이렇게 로드한 ChapterDataTable에서 찾고자 하는 ChapterData만 가져오는 함수
    public ChapterData GetChapterData(int chapterNo)
    {
        // 특정 챕터 넘버로 챕터 데이터 테이블을 검색해서
        // 그 챕터 넘버에 해당하는 데이터를 반환하는 함수
        // 링큐 사용 -> 링큐 : 검색, 변경을 좀 더 용이하게 해주는 기능
        // 만약 링큐를 사용하지 않는다면
        /*
           
         */
        return ChapterDataTable.Where(item => item.ChapterNo == chapterNo).FirstOrDefault();
    }
    #endregion

    #region ITEM_DATA

    // 데이터 테이블 명을 가진 변수를 선언
    private const string ITEM_DATA_TABLE = "ItemDataTable";
    // 아이템 데이터를 담을 컨테이너를 선언
    private List<ItemData> ItemDataTable = new List<ItemData>();

    //아이템 데이터를 로드하는 함수
    private void LoadItemDataTable()
    {
        // csv 파일을 읽어옴
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{ITEM_DATA_TABLE}");

        //데이터를 참고해서 아이템 데이터를 만들어줌.
        foreach(var data in parsedDataTable)
        {
            var itemData = new ItemData
            {
                ItemId = Convert.ToInt32(data["item_id"]),
                ItemName = data["item_name"].ToString(),
                AttackPower = Convert.ToInt32(data["attack_power"]),
                Defense = Convert.ToInt32(data["defense"]),
            };

            ItemDataTable.Add(itemData);
        }
    }

    //아이템 데이터 컨테이너에서 특정 아이템 아이디에 데이터를 찾는 함수
    public ItemData GetItemData(int itemId)
    {
        return ItemDataTable.Where(item => item.ItemId == itemId).FirstOrDefault();
    }
    #endregion
}

// 챕터 데이터의 각 값을 저장할 수 있도록 만들어야 하는 클래스
public class ChapterData
{
    // ChapterDataTable를 그대로 가져왔어요.
    public int ChapterNo; // 챕터 넘버
    public int TotalStages; // 챕터 내 스테이지 개수
    public int ChapterRewardGem; // 챕터를 클리어 했을 시 받게 되는 보석
    public int ChapterRewardGold; // 챕터를 클리어 했을 시 받게 되는 골드
}

public class ItemData
{
    public int ItemId;
    public string ItemName;
    public int AttackPower;
    public int Defense;
}

public enum ItemType
{
    Weapon = 1,
    Shield,
    ChestArmor,
    Gloves,
    Boots,
    Accessory,
}

public enum ItemGrade
{
    Common = 1,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}