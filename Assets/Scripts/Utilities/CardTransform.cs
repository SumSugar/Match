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
