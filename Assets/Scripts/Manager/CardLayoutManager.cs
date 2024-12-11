using System.Collections.Generic;
using UnityEngine;


public class CardLayoutManager : Singleton<CardLayoutManager>
{
    
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;

    [Header(header:"å å`éQêî")]
    public float angleBwtweenCards = 7f;

    public float radius = 17f;

    public Vector3 centerPoint;

    [SerializeField] private List<Vector3> cardPositions = new();
    private List<Quaternion> cardRotations = new();

    protected override void Awake()
    {
        base.Awake();
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -20.5f;
    }

    public CardTransform GetCardTransform(int index , int totalCards)
    {
        CaculatePosition(totalCards , isHorizontal) ;

        return new CardTransform(cardPositions[index], cardRotations[index]);
    }

    private void CaculatePosition(int numberOfCards , bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();

        if (horizontal)
        {
            float currentWidth = cardSpacing * (numberOfCards - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;

            for (int i = 0; i < numberOfCards; i++) 
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);

                var pos =new Vector3 (xPos, centerPoint.y, 0);
                var rot = Quaternion.identity;

                cardPositions.Add (pos);
                cardRotations.Add (rot);
            }

        }
        else
        {
            float cardAngle = (numberOfCards - 1) * angleBwtweenCards / 2;

            for (int i = 0; i < numberOfCards; i++)
            {
                var pos = FanCardPosition (cardAngle - i * angleBwtweenCards);

                var rot = Quaternion.Euler(0, 0, cardAngle - i * angleBwtweenCards);

                cardPositions.Add(pos);
                cardRotations.Add(rot);
            }
        }
    }
    
    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
        ); 
    }
}
