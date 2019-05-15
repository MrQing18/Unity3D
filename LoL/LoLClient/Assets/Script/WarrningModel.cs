using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void WarrningResult();
public class WarrningModel
{
    public WarrningResult result;
    public string value;
    public WarrningModel(string value, WarrningResult result = null)
    {
        this.value = value;
        this.result = result;
    }
}

