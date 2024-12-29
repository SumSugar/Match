using TMPro;
using UnityEngine;

public class AttackMessageBox : BaseMessageBox
{
    public TextMeshProUGUI tmpAttackUserText;
    public TextMeshProUGUI tmpAttackTargetText;

    public override void DisplayMessage(BaseMessage entry)
    {
        // 覆写显示方法，添加系统信息
        CharacterBase character = entry.sender as CharacterBase;
        string attackUserColor = character.tag == "Ally" ? "#73CFF5" : "#EC6161";
        string attackTargetColor = character.currentTarget.tag == "Enemy" ? "#73CFF5" : "#EC6161";

        tmpAttackUserText.text = $"<color={attackUserColor}>{character.Name}</color>";
        tmpAttackTargetText.text = $"<color={attackTargetColor}>{character.currentTarget.Name}</color>";

    }
    public void SetMessage(string attackUser, string attackTarget)
    {
        tmpAttackUserText.text = attackUser;
        tmpAttackTargetText.text = attackTarget;
    }
}
