using UnityEngine;

public class DestinyTokenView : MonoBehaviour
{
    public Destiny destiny;
    

    public void RegisterDestiny(Destiny destiny)
    {
        this.destiny = destiny;
        destiny.OnDestoryAction = OnDestinyDestroy;
    }

    public void OnDestinyDestroy()
    {
        gameObject.SetActive(false);
    }
}
