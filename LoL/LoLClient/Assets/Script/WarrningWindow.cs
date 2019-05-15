using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarrningWindow : MonoBehaviour {

    // Use this for initialization
    public Text text;
    WarrningResult result;
    public void active(WarrningModel value)
    {
        text.text = value.value;
        this.result = value.result;
        gameObject.SetActive(true);
    }
    public void close()
    {
        gameObject.SetActive(false);
        if(result != null)
        {
            result();
        }
    }

}
