using System.Collections.Generic;
using UnityEngine;

public class BreakerQueueLayout: MonoBehaviour
{

    public bool isVertical;
    public float maxWidth = 7f;
    public float breakerSpacing = 2f;

    [Header(header: "å å`éQêî")]
    public float angleBwtweenBreakers = 7f;

    public float radius = 17f;

    public Vector2 centerPoint;



    [SerializeField] private List<Vector2> breakerLoaclPositions = new();

    private List<Quaternion> breakerRotations = new();


    public void Awake()
    {
        //centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -20.5f;
        if (!isVertical)
        {
            centerPoint.x -= radius;
        }
    }

    public RectTransformData GetRectTransform(int index, int totalBreakers)
    {
        CaculatePosition(totalBreakers, isVertical);

        return new RectTransformData(breakerLoaclPositions[index], breakerRotations[index]);
        
    }

    private void CaculatePosition(int numberOfBreakers, bool horizontal)
    {
        breakerLoaclPositions.Clear();
        breakerRotations.Clear();

        if (isVertical)
        {
            float currentWidth = breakerSpacing * (numberOfBreakers - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfBreakers - 1) : 0;

            for (int i = 0; i < numberOfBreakers; i++)
            {
                float yPos = 0 - (totalWidth / 2) + (i * currentSpacing);

                var pos = new Vector2(0, yPos);
                var rot = Quaternion.identity;

                breakerLoaclPositions.Add(pos);
                breakerRotations.Add(rot);
            }

        }
        else
        {
            float breakerAngle = (numberOfBreakers - 1) * angleBwtweenBreakers / 2;

            for (int i = 0; i < numberOfBreakers; i++)
            {
                var pos = FanBreakerPosition(breakerAngle - i * angleBwtweenBreakers);

                var rot = Quaternion.Euler(0, 0, breakerAngle - i * angleBwtweenBreakers);

                breakerLoaclPositions.Add(pos);
                breakerRotations.Add(rot);
            }
        }
    }

    private Vector2 FanBreakerPosition(float angle)
    {
        return new Vector2(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius
        );
    }
}