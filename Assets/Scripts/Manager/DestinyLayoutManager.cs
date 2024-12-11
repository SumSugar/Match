using System.Collections.Generic;
using UnityEngine;

public class DestinyLayoutManager : MonoBehaviour
{
    
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float destinySpacing = 2f;

    [Header(header:"å å`éQêî")]
    public float angleBwtweenDestinys = 7f;

    public float radius = 17f;

    public Vector3 centerPoint;

    [SerializeField] private List<Vector3> destinyLoaclPositions = new();

    private List<Quaternion> destinyRotations = new();


    private void Awake()
    {
        //centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -20.5f;
        if (!isHorizontal)
        {
            centerPoint.y -= radius;
        }
    }

    public CardTransform GetDestinyTransform(int index , int totalDestinys)
    {
        CaculatePosition(totalDestinys , isHorizontal) ;

        return new CardTransform(destinyLoaclPositions[index], destinyRotations[index]);
    }

    private void CaculatePosition(int numberOfDestinys , bool horizontal)
    {
        destinyLoaclPositions.Clear();
        destinyRotations.Clear();

        if (horizontal)
        {
            float currentWidth = destinySpacing * (numberOfDestinys - 1);
            float totalWidth = Mathf.Min(currentWidth, maxWidth);

            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfDestinys - 1) : 0;

            for (int i = 0; i < numberOfDestinys; i++) 
            {
                float xPos = 0 - (totalWidth / 2) + (i * currentSpacing);

                var pos = new Vector3 (xPos, 0, 0);
                var rot = Quaternion.identity;

                destinyLoaclPositions.Add (pos);
                destinyRotations.Add (rot);
            }

        }
        else
        {
            float destinyAngle = (numberOfDestinys - 1) * angleBwtweenDestinys / 2;

            for (int i = 0; i < numberOfDestinys; i++)
            {
                var pos = FanDestinyPosition (destinyAngle - i * angleBwtweenDestinys);

                var rot = Quaternion.Euler(0, 0, destinyAngle - i * angleBwtweenDestinys);

                destinyLoaclPositions.Add(pos);
                destinyRotations.Add(rot);
            }
        }
    }
    
    private Vector3 FanDestinyPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - Mathf.Sin(Mathf.Deg2Rad * angle) * radius,
            centerPoint.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
            0
        ); 
    }
}
