using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PickCardPanel : MonoBehaviour
{
    public CardManager cardManager;
    private VisualElement rootElement;
    public VisualTreeAsset cardTemplate;
    private VisualElement cardContainer;

    private CardDataSO currentCardData;

    private Button confirmButton;

    private List<Button> cardButtons = new();

    [Header(header:"广播事件")]
    public ObjectEventSO finishPickCardEvent;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        cardContainer = rootElement.Q<VisualElement>("Container");
        confirmButton = rootElement.Q<Button>("ConfirmButton");

        confirmButton.clicked += OnConfirmButtonClicked;

        for (int i = 0; i < 3; i++)
        {
            var card = cardTemplate.Instantiate();
            var data = cardManager.GetNewCardData();
            //初始化
            InitCard(card, data);
            var cardButton = card.Q<Button>("Card");

            cardContainer.Add(card);
            cardButtons.Add(cardButton);
            cardButton.clicked += () => OnCardClicked(cardButton , data);
        }
    }

    private void OnConfirmButtonClicked()
    {
        cardManager.UnlockCard(currentCardData);
        finishPickCardEvent.RaiseEvent(null,this);
    }

    private void OnCardClicked(Button cardButton , CardDataSO data)
    {
        currentCardData = data;
        for (int i = 0; i < cardButtons.Count; i++)
        {
            if(cardButtons[i] == cardButton)
            {
                cardButtons[i].SetEnabled(false);
            }
            else
            {
                cardButtons[i].SetEnabled(true);
            }
        }
        //Debug.Log(currentCardData.cardName);
    }

    public void InitCard(VisualElement card , CardDataSO cardData)
    {
        card.dataSource = cardData;

        var cardSpirteElement = card.Q<VisualElement>("CardSprite");
        var cardCost = card.Q<Label>("EnergyCost");
        var cardDescription = card.Q<Label>("CardDescription");
        var cardType = card.Q<Label>("CardType");
        var cardName = card.Q<Label>("CardName");


        cardSpirteElement.style.backgroundImage = new StyleBackground(cardData.cardImage);
        cardName.text = cardData.cardName;
        cardCost.text = cardData.cost.ToString();
        cardDescription.text = cardData.description;
        cardType.text = cardData.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "技能",
            CardType.Abilities => "能力",
            _=>throw new System.NotImplementedException()
        };
    }
}
