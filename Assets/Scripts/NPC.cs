using System.Collections;
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
        if (dialogueData == null /* || (PauseController.IsGamePaused && !isDialogueActive) */)
        {
            return;
        }

        if (_isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
        
    }
    void StartDialogue()
    {
        _isDialogueActive = true;
        dialogueIndex = 0;
            
        nameText.SetText(dialogueData.name);
        portraitImage.sprite = dialogueData.npcPortrait;
            
        dialoguePanel.SetActive(true);
        //PauseController.SetPause(true);

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        _isTyping = true;
        dialogueText.SetText("");
            
        // so it goes letter by letter
        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed); 
        }
            
        _isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    void NextLine()
    {
        if (_isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            _isTyping = false;
        }
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        _isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        //PauseController.SetPause(false);
    }
}
