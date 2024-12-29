using System.Collections.Generic;
using UnityEngine;

public class BreakerLayoutManager:  MonoBehaviour
{
    
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float breakerSpacing = 2f;

    [Header(header:"弧形参数")]
    public float angleBwtweenBreakers = 7f;

    public float radius = 17f;

    public Vector3 centerPoint;



    [SerializeField] private List<Vector3> breakerLoaclPositions = new();

    private List<Quaternion> breakerRotations = new();


    public void Awake()
    {
        //centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -20.5f;
        if (!isHorizontal)
        {
            centerPoint.y -= radius;
        }
    }

    public CardTransform GetBreakerTransform(int index , int totalBreakers)
    {
        CaculatePosition(totalBreakers , isHorizontal) ;

        return new CardTransform(breakerLoaclPositions[index], breakerRotations[index]);
    }

    private void CaculatePosition(int numberOfBreakers , bool horizontal)
    {
        breakerLoaclPositions.Clear();
        breakerRotations.Clear();

        if (horizontal)
        {
            float currentWidth = breakerSpacing * (numberOfBreakers - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfBreakers - 1) : 0;

            for (int i = 0; i < numberOfBreakers; i++) 
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);

                var pos =new Vector3 (xPos, transform.localPosition.y, 0);
                var rot = Quaternion.identity;

                breakerLoaclPositions.Add (pos);
                breakerRotations.Add (rot);
            }

        }
        else
        {
            float breakerAngle = (numberOfBreakers - 1) * angleBwtweenBreakers / 2;

            for (int i = 0; i < numberOfBreakers; i++)
            {
                var pos = FanBreakerPosition (breakerAngle - i * angleBwtweenBreakers);

                var rot = Quaternion.Euler(0, 0, breakerAngle - i * angleBwtweenBreakers);

                breakerLoaclPositions.Add(pos);
                breakerRotations.Add(rot);
            }
        }
    }
    
    private Vector3 FanBreakerPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
        ); 
    }
}
