using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BreakerView : MonoBehaviour
{
    [Header(header: "组件")]
    public Image cardImage;

    public TextMeshProUGUI costText, descriptionText, typeText, breakerName, amountText;

    public Button actionButton; // 卡牌的交互按钮

    public BreakerDataSO breakerData;

    public void SetBreakerData(BreakerDataSO data, int amount)
    {
        breakerData = data;
        cardImage.sprite = data.Icon;
        amountText.text = $"x{amount}";
        //costText.text = data.cooldown.ToString();
        //descriptionText.text = data.description;
        //breakerName.text = data.breakerName;
        //typeText.text = data.breakerType switch
        //{
        //    BreakerType.Attack => "Attack",
        //    BreakerType.Abilities => "Ability",
        //    _ => throw new System.NotImplementedException(),
        //};

        // 如果数量为零，禁用按钮并改变外观
        if (amount <= 0)
        {
            actionButton.interactable = false;
            // 可以在这里改变外观，例如使图片灰化
            cardImage.color = Color.gray;
        }
        else
        {
            actionButton.interactable = true;
            cardImage.color = Color.white;
        }
    }
}
