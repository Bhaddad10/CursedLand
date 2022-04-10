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
        if (!dialogManager)
        {
            Debug.LogWarning("LogManager Instance not found.");
        }
    }

    public void TriggerDialog()
    {
        Debug.Log("calling dialog manager");
        // dialogManager.StartDialog(dialog);
        dialogManager.StartDialog(dialog);
    }
}
