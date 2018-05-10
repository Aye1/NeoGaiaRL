using System;

public class PNJTest : Character, IInteractable {

    private Player _player;

    public void OnInteract(Player player)
    {
        _player = player;
        DialogLine[] texts = { new DialogLine("Bien ouej, tu es capable d'interagir avec un PNJ!", this)
                , new DialogLine("Je peux répondre !", _player)
                , new DialogLine("Et maintenant tu peux sauter 10 fois, parce que c'est comme ça", this)};
        DialogManager.instance.dialogClosing += OnDialogClosed;
        DialogManager.instance.StartDialog(texts);
    }

    private void OnDialogClosed(object sender, EventArgs args)
    {
        _player.has10Jumps = true;
        DialogManager.instance.dialogClosing -= OnDialogClosed;
    }
}
