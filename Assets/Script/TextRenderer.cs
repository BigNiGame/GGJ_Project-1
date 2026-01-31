using UnityEngine;
using System.Collections;
using TMPro;

public class TextRenderer : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.03f;
    bool isTyping;
    public bool canClose;
    
    public void StartTyping(string fullText)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(fullText));
    }
    IEnumerator TypeText(string fullText)
    {
        isTyping = true;
        canClose = false;
        MainStall.instance.currentPatient.patientState = PatientState.Talking;
        dialogueText.text = fullText;
        dialogueText.maxVisibleCharacters = 0;
        int charIndex = 0;
        while (charIndex < fullText.Length)
        {
            dialogueText.maxVisibleCharacters++;
            char currentChar = fullText[charIndex];

            float delay = typingSpeed;

            // punctuation pauses
            if (currentChar == ',' || currentChar == '，')
                delay *= 3f;
            else if (currentChar == '.' || currentChar == '!' || currentChar == '?'|| currentChar == '—')
                delay *= 6f;

            yield return new WaitForSeconds(delay);

            charIndex++;
        }
        while (dialogueText.maxVisibleCharacters < fullText.Length)
        {
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        MainStall.instance.currentPatient.patientState = PatientState.Idle;
        yield return new WaitForSeconds(2);
        canClose = true;
    }

    public void SkipTyping()
    {
        if (isTyping)
            dialogueText.maxVisibleCharacters = dialogueText.text.Length;
    }
}
