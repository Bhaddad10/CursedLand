using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    public DialogManager dialogManager;

    public void TriggerDialog()
    {
        Debug.Log("calling dialog manager");
        // dialogManager.StartDialog(dialog);
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }
}
