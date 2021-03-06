using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Dialog System inspired on https://www.youtube.com/watch?v=_nRzoTzeyxU,
 * customized for project characteristics
 */
public class DialogManager : MonoBehaviour
{
    public GameObject dialogBox;
    public Text nameText;
    public Text dialogText;

    private bool bDialogActive = false;

    private Queue<string> sentences;
    private NpcController npc;

    // Singleton
    private static DialogManager _uniqueInstance;
    public static DialogManager Instance
    {
        get { return _uniqueInstance; }
    }
    

    void Awake()
    {
        _uniqueInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialog(NpcController npcController)
    {
        npc = npcController;
        Dialog dialog = npcController.dialog;
        if (bDialogActive)
        {
            DisplayNextSentence();
            return;
        }

        bDialogActive = true;
        dialogBox.SetActive(bDialogActive);
        nameText.text = dialog.name;
        sentences.Clear();
        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogText.text = sentence;
    }

    void EndDialog()
    {
        bDialogActive = false;
        dialogBox.SetActive(bDialogActive);
        if (npc is MerchantNpcController)
        {
            ((MerchantNpcController) npc).actionAfterDialog();
            npc = null;
        }
    }

    public bool IsDialogActive()
    {
        return bDialogActive;
    }

    public void OnDestroy()
    {
        _uniqueInstance = null;
    }
}
