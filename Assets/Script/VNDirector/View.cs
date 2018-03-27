using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class View : MonoBehaviour {

    public const float timePerChar = 0.2f;
    private float scrollTime = 0;

    public GameObject textBoxCollection;

    public Text nameBox;
    public Text dialogueBox;

    private Queue<char> textToScroll;
    public bool isTextScrolling = false;
    private string fullText;

    void Start() {
        textBoxCollection.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        scrollTime += Time.deltaTime;
        if ((scrollTime > timePerChar) && isTextScrolling) {
            dialogueBox.text += textToScroll.Dequeue();
            if (textToScroll.Count == 0) isTextScrolling = false;
        }
    }

    public void SetDialogue(string text, bool isScrolled = true) {
        fullText = Manager.jsEngine.FormatString(text);
        textToScroll = new Queue<char>(fullText);
        isTextScrolling = true;

        dialogueBox.text = "";

        textBoxCollection.SetActive(true);
    }

    public void EndScroll() {
        isTextScrolling = false;
        dialogueBox.text = fullText;
    }
}
