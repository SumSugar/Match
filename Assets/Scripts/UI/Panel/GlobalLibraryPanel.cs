using System.Collections.Generic;
using UnityEngine;

public class GlobalLibraryPanel : MonoBehaviour
{
    public GameObject cardUIPrefab;
    public GameObject draggableCardPrefab;
    public Transform cardContainer;
    public BreakerLibrarySO breakerLibrarySO;

    private Dictionary<BreakerDataSO, int> globalLibrary = new Dictionary<BreakerDataSO, int>();
    private Dictionary<BreakerDataSO, GameObject> cardUIInstances = new Dictionary<BreakerDataSO, GameObject>();

    private void Start()
    {
        LoadLibraryFromSO();
        DisplayLibrary();
    }

    private void LoadLibraryFromSO()
    {
        globalLibrary.Clear();
        foreach (var entry in breakerLibrarySO.breakerLibraryList)
        {
            if (entry.breakerData != null && entry.amount > 0)
            {
                globalLibrary[entry.breakerData] = entry.amount;
            }
        }
    }

    public void DisplayLibrary()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        cardUIInstances.Clear();

        foreach (var kvp in globalLibrary)
        {
            CreateCardUI(kvp.Key, kvp.Value);
        }
    }

    private void CreateCardUI(BreakerDataSO card, int amount)
    {
        GameObject cardUI = Instantiate(cardUIPrefab, cardContainer);
        var breakerView = cardUI.GetComponent<BreakerView>();
        breakerView.SetBreakerData(card, amount);

        var cardController = cardUI.GetComponent<CardController>();
        cardController.Initialize(card, this);

        cardUIInstances[card] = cardUI;
    }

    public void ConfirmRemoveCard(BreakerDataSO card)
    {
        if (globalLibrary.ContainsKey(card) && globalLibrary[card] > 0)
        {
            globalLibrary[card]--;

            if (globalLibrary[card] <= 0)
            {
                Destroy(cardUIInstances[card]);
                cardUIInstances.Remove(card);
            }
            else
            {
                UpdateCardUI(card);
            }
        }
    }

    public void AddCard(BreakerDataSO card)
    {
        if (globalLibrary.ContainsKey(card))
        {
            globalLibrary[card]++;
        }
        else
        {
            globalLibrary[card] = 1;
        }

        UpdateCardUI(card);
    }

    private void UpdateCardUI(BreakerDataSO card)
    {
        if (cardUIInstances.TryGetValue(card, out GameObject cardUI))
        {
            var breakerView = cardUI.GetComponent<BreakerView>();
            breakerView.SetBreakerData(card, globalLibrary[card]);
        }
    }
}



