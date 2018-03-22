using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;

public class View : MonoBehaviour {

    public Text nameBox;
    public Text dialogueBox;

    public string scrollText = "";
    public Queue<char> textToScroll;

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    void PrintDialogue(string dialogue) {
        textToScroll = new Queue<char>(dialogue);
    }
}
