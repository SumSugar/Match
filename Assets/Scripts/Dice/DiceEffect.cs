using UnityEngine;
using DG.Tweening;

public class DiceEffect : MonoBehaviour
{
    public SpriteRenderer diceSprite;  // 骰子的SpriteRenderer
    public Vector3 jumpHeight = new Vector3(0, 2f, 0);  // 骰子跳起的高度
    public float duration = 1f;        // 动画持续时间
    public Vector3 rotationAngle = new Vector3(0, 0, 360);  // 骰子旋转角度
    public Vector3 minScale = new Vector3(0.7f, 0.7f, 1);  // 缩小比例

    void Start()
    {
        PlayDiceAnimation();
    }

    public void PlayDiceAnimation()
    {

        // 抛起时的旋转和缩放效果
        diceSprite.transform.DOLocalJump(diceSprite.transform.localPosition, jumpHeight.y, 1, duration).SetEase(Ease.OutQuad);
        diceSprite.transform.DORotate(rotationAngle, duration, RotateMode.FastBeyond360).SetEase(Ease.OutQuad);
        diceSprite.transform.DOScale(minScale, duration / 2).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                diceSprite.transform.DOScale(Vector3.one, duration / 2);  // 落下时回到原始大小
                Debug.Log("骰子动画完成");
            });
    }
}