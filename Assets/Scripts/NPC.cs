using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    
    public int dialogueIndex;
    private bool _isTyping, _isDialogueActive;

    public bool CanInteract()
    {
        return !_isDialogueActive;
    }

    public void Interact()
    {
        if (dialogueData == null)
        {
            return;
        }

        if (_isDialogueActive)
        {
            
        }
        else
        {
            
        }
        
        void StartDialogue()
        {
            _isDialogueActive = true;
            dialogueIndex = 0;
            
            nameText.SetText(dialogueData.name);
            portraitImage.sprite = dialogueData.npcPortrait;
        }
    }
}
