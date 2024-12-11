using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float offsetrSpeed = 0.1f;

    private void Update()
    {
        if (lineRenderer != null)
        {
            //获取当前纹理偏移
            var offset = lineRenderer.material.mainTextureOffset;

            offset.x += offsetrSpeed * Time.deltaTime;

            lineRenderer.material.mainTextureOffset = offset;
        }
    }
}
