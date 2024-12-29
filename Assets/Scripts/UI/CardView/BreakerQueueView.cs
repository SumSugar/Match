using UnityEngine;
using UnityEngine.UI;

public class BreakerQueueView : MonoBehaviour
{
    public Breaker breaker;
    public Transform continer;
    public Image breakerImage;
    public Image userIcon;
    public void SetBreakerData(Breaker breaker)
    {
        this.breaker = breaker;
        breakerImage.sprite = breaker.data.Icon;

        userIcon.sprite = breaker.ownerCharacter.characterData.Icon;
    }
}
