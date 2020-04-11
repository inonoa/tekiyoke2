using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReusable
{
    void Activate(string paramsStr);
    bool InUse{ get; }
}
