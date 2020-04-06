using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReusable
{
    void Activate();
    bool InUse{ get; }
}
