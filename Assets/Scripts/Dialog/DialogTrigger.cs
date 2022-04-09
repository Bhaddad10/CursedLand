using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    public DialogManager dialogManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerDialog()
    {
        Debug.Log("calling dialog manager");
        // dialogManager.StartDialog(dialog);
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }
}
