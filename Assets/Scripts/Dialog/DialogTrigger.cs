using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    private DialogManager dialogManager;

    void Start()
    {
        dialogManager = DialogManager.Instance;
    }

    public void TriggerDialog()
    {
        dialogManager.StartDialog(dialog);
    }
}
