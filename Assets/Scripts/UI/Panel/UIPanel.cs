using UnityEngine;

public class UIPanel : MonoBehaviour
{
    // 面板名称
    public string panelName;
    public UIPanelState state;

    // 显示面板
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    // 隐藏面板
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
