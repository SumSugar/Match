using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header(header:"ñ î¬")]
    public GameObject gameplayPanel;
    public GameObject gameWinPanel;
    public GameObject gameOverPanel;
    public GameObject pickCardPanel;
    public GameObject restRoomPanel;
    public UIPanel MapPanel;
    public UIPanel currentPanel;

    // íçôePanel
    public void RegisterPanel(UIPanel panel)
    {
        currentPanel = panel;
        Debug.Log($"Panel {panel.panelName} íçôeìû UIManager");
    }

    public void OnLoadRoomEvent(object data)
    {
        Room currentRoom = (Room)data;

        switch (currentRoom.roomData.roomType)
        {
            case RoomType.MinorEnemy:
            case RoomType.ELiteEnemy:
            case RoomType.Boss:
                //gameplayPanel.SetActive(true);
                break;
            case RoomType.Shop:
                break;
            case RoomType.Treasure:
                break;
            case RoomType.RestRoom:
                //restRoomPanel.SetActive(true);
                break;
        }
    }


    /// <summary>
    /// loadMapEvent / loadMenu
    /// </summary>
    public void HideALlPanels()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restRoomPanel.SetActive(false);
    }

    public void OnGameWinEvent()
    {
        gameplayPanel.SetActive(false);
        gameWinPanel.SetActive(true);
    }

    public void OnGameOverEvent()
    {
        gameplayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void OnPickCardEvent()
    {
        pickCardPanel.SetActive(true);
    }

    public void OnFinishPickCardEvent()
    {
        pickCardPanel.SetActive(false);
    }
}
