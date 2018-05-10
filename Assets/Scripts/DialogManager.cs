using System;
using UnityEngine;

public class DialogManager : MonoBehaviour {
    public DialogBox dialog;

    public bool isDialogOpen = false;

    // Event sent when the dialog is closing
    public EventHandler dialogClosing;

    public static DialogManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void StartDialog(DialogLine[] dialogLines)
    {
        if (!isDialogOpen)
        {
            DialogBox currentDialog = Instantiate(dialog);
            currentDialog.Init(dialogLines);
            isDialogOpen = true;
            currentDialog.dialogClosing += DialogClosed;
        }
    }

    private void DialogClosed(object sender, EventArgs args)
    {
        DialogBox dialog = (DialogBox)sender;
        dialog.dialogClosing -= DialogClosed;
        isDialogOpen = false;
        EventHandler handler = dialogClosing;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
        DestroyObject(dialog.gameObject);
    }
}
