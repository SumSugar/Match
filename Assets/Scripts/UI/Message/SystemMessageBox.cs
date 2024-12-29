using TMPro;
using UnityEngine;

public class SystemMessageBox : BaseMessageBox
{
    public TextMeshProUGUI tmpSyetemText;

    public override void DisplayMessage(BaseMessage entry)
    {
        // 覆写显示方法，添加系统信息
        tmpSyetemText.text = entry.content;
 

    }
    public void SetMessage(string attackUser, string attackTarget)
    {

    }
}

