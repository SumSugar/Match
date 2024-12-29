using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RestRoomPanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Button restButton , backToMapButton;

    public Effect restEffcet;
    public ObjectEventSO backToMapEvent;

    private CharacterBase player;

    private void OnEnable()
    {
        rootElement = GetComponent<UIDocument>().rootVisualElement;
        restButton = rootElement.Q<Button>("RestButton");
        backToMapButton = rootElement.Q<Button>("BackToMapButton");

        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include);

        restButton.clicked += OnRestButtonClicked;

        backToMapButton.clicked += OnBackToMapButtonClicked;
    }

    private void OnBackToMapButtonClicked()
    {
        backToMapEvent.RaiseEvent(null, this);
    }

    private void OnRestButtonClicked()
    {
        restEffcet.Excute(player, null);
        restButton.SetEnabled(false);
    }
}
