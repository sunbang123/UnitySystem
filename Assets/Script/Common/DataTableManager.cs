using System;
using System.Collections.Generic;
using System.Linq;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    private const string DATA_PATH = "DataTable";

    protected override void Init()
    {
        base.Init();

        LoadChapterDataTable();
        LoadItemDataTable();
        //아이템 데이터 컨테이너에서 특정 아이템 아이디에 데이터를 찾는 함수
    }

    #region CHAPTER_DATA
    private const string CHAPTER_DATA_TABLE = "ChapterDataTable";
    private List<ChapterData> ChapterDataTable = new List<ChapterData>();

    private void LoadChapterDataTable()
    {
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE}");

        foreach (var data in parsedDataTable)
        {
            var chapterData = new ChapterData
            {
                ChapterNo = Convert.ToInt32(data["chapter_no"]),
                ChapterName = data["chapter_name"].ToString(),
                TotalStage = Convert.ToInt32(data["total_stages"]),
                ChapterRewardGem = Convert.ToInt32(data["chapter_reward_gem"]),
                ChapterRewardGold = Convert.ToInt32(data["chapter_reward_gold"]),
            };

            ChapterDataTable.Add(chapterData);
        }
    }

    public ChapterData GetChapterData(int chapterNo)
    {
        return ChapterDataTable.Where(item => item.ChapterNo == chapterNo).FirstOrDefault();
    }
    #endregion

    #region ITEM_DATA

    //데이터테이블 명을 가진 변수를 선언
    private const string ITEM_DATA_TABLE = "ItemDataTable";
    //아이템테이터를 담을 컨테이너를 선언
    private List<ItemData> ItemDataTable = new List<ItemData>();

    //아이템데이터를 로드하는 함수 
    private void LoadItemDataTable()
    {
        //csv파일을 읽어옮
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{ITEM_DATA_TABLE}");

        //데이터를 참고해서 아이템 데이터를 만들어 주자..
        foreach (var data in parsedDataTable)
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

public class ChapterData
{
    public int ChapterNo;
    public string ChapterName;
    public int TotalStage;
    public int ChapterRewardGem;
    public int ChapterRewardGold;
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
    Accessory
}

public enum ItemGrade
{
    Common = 1,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}
