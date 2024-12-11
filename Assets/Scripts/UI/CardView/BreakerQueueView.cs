using UnityEngine;
using UnityEngine.UI;

public class BreakerQueueView : MonoBehaviour
{
    public BreakerDataSO breakerData;
    public Image icon;
    public void SetBreakerData(BreakerDataSO data)
    {
        breakerData = data;
        icon.sprite = data.breakerIcon;
    }
}
