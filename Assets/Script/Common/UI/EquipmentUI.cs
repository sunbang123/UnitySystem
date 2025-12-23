using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUIData : BaseUIData
{
    public long SerialNumber;
    public int ItemId;
    public bool IsEquipped;
}
public class EquipmentUI : BaseUI
{
    public Image ItemGradeBg;
    public Image ItemIcon;
    public TextMeshProUGUI ItemGradeTxt;
    public TextMeshProUGUI ItemNameTxt;
    public TextMeshProUGUI AttackPowerAmountTxt;
    public TextMeshProUGUI DefenseAmountTxt;

    private EquipmentUIData m_EquipmentUIData;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        m_EquipmentUIData = uiData as EquipmentUIData;
        if(m_EquipmentUIData == null)
        {
            Logger.LogError("m_EquipmentUIData is Invalid.");
            return;
        }

        var itemData = DataTableManager.Instance.GetItemData(m_EquipmentUIData.ItemId);
        if(itemData == null)
        {
            Logger.LogError($"Item data is invalid. ItemId:{m_EquipmentUIData.ItemId}");
            return;
        }

        var itemGrade = (ItemGrade)((m_EquipmentUIData.ItemId / 1000) % 10);
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");

        if(gradeBgTexture != null)
        {
            ItemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }

        ItemGradeTxt.text = itemGrade.ToString();
        var hexColor = string.Empty;
        switch(itemGrade)
        {
            case ItemGrade.Common:
                hexColor = "#1AB3FF";
                break;
            case ItemGrade.Uncommon:
                hexColor = "#51C52C";
                break;
            case ItemGrade.Rare:
                hexColor = "#EA5AFF";
                break;
            case ItemGrade.Epic:
                hexColor = "#FF9900";
                break;
            case ItemGrade.Legendary:
                hexColor = "#F24949";
                break;
            default:
                break;
        }

        Color color;

        if (ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            ItemGradeTxt.color = color;
        }

        StringBuilder sb = new StringBuilder(m_EquipmentUIData.ItemId.ToString());
        sb[1] = '1';
        var itemIconName = sb.ToString();

        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        if (itemIconTexture != null)
        {
            ItemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }

        ItemNameTxt.text = itemData.ItemName;
        AttackPowerAmountTxt.text = $"+{itemData.AttackPower}";
        DefenseAmountTxt.text = $"+{itemData.Defense}";
    }
}