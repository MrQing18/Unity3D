using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningWindow : MonoBehaviour {

    // Use this for initialization
    public Text text;
    public void active(string value)
    {
        text.text = value;
        gameObject.SetActive(true);
    }
    public void close()
    {
        gameObject.SetActive(false);
    }

}
