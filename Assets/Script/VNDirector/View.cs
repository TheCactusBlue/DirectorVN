using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class View : MonoBehaviour {

    public GameObject textBoxCollection;

    public Text nameBox;
    public Text dialogueBox;
    private bool _isDialogueBoxVisible = false;

    public string scrollText = "";
    public Queue<char> textToScroll;
    // Use this for initialization
    void Start() {
        textBoxCollection.SetActive(false);
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetDialogue(string text, bool isScrolled = true) {
        textBoxCollection.SetActive(true);
        dialogueBox.text = Manager.jsEngine.FormatString(text);
    }
}
