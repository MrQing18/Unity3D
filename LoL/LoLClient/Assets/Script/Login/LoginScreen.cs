using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScreen : MonoBehaviour {
    #region 登陆面板部分
    public InputField accountInput;
    public InputField passwordInput;
    #endregion
    public Button loginBtn;

    public GameObject regPanel;  // 注册界面 
    #region 注册面板部分
    public InputField regAccountInput;
    public InputField regpwInput;
    public InputField regpw1Input;
    #endregion
    public void loginOnClick()
    {
        if (accountInput.text.Length == 0 || accountInput.text.Length > 6)
        {
            WarrningManager.errors.Add(new WarrningModel("账号不合法", delegate { Debug.Log("回调测试"); } ) );
            return;
        }
        if (passwordInput.text.Length == 0 || passwordInput.text.Length > 6)
        {
            WarrningManager.errors.Add(new WarrningModel("密码不合法"));
            return;
        }
        Debug.Log("申请登陆");
        loginBtn.interactable = false;
    }

    public void regClick()
    {
        regPanel.SetActive(true);
    }
    public void regClose()
    {
        regPanel.SetActive(false);
    }
    public void regpanelregClick()
    {
        if (regAccountInput.text.Length == 0 || regAccountInput.text.Length > 6)
        {
            WarrningManager.errors.Add(new WarrningModel("账号不合法"));
            return;
        }
        if (regpwInput.text.Length == 0 || regpwInput.text.Length > 6)
        {
            WarrningManager.errors.Add(new WarrningModel("密码不合法"));
            return;
        }
        if (!regpwInput.text.Equals(regpw1Input.text))
        {
            WarrningManager.errors.Add(new WarrningModel("两次密码不一致"));
            return;
        }
        Debug.Log("申请注册");
        regPanel.SetActive(false);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
