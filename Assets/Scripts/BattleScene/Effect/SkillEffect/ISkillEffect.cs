using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectObject
{
    public void SetPosition(Vector3 pos);
    public void DestroyEffect();
}
