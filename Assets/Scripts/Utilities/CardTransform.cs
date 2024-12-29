using UnityEngine;

public struct CardTransform
{
    public Vector3 pos;
    public Quaternion rotation;

    public CardTransform(Vector3 vector3 , Quaternion quaternion)
    {
        pos = vector3;
        rotation = quaternion;
    }
}

public struct RectTransformData
{
    public Vector2 anchoredPosition;      // 2D 平面的位置
    public Quaternion rotation; // 旋转

    public RectTransformData(Vector2 position, Quaternion rot)
    {
        anchoredPosition = position;
        rotation = rot;
    }
}

public class TransformPropertyChange
{
    public Vector2 OldPosition { get; set; }
    public Vector2 NewPosition { get; set; }
    public float OldAlpha { get; set; }
    public float NewAlpha { get; set; }
    public Vector3 OldScale { get; set; }
    public Vector3 NewScale { get; set; }
}



public struct CameraFloat
{
    public float intensity;
    public float duration;

    public CameraFloat(float intensity, float duration)
    {
        this.intensity = intensity;
        this.duration = duration;
    }
}
