using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class BaseModel
{
    private bool _Dirty = true;
    public bool Dirty
    {
        get
        {
            if (_Dirty)
            {
                _Dirty = false;
                return true;
            }

            return false;
        }
        set
        {
            _Dirty = value;
        }
    }
}

public abstract class BaseBehavior<T> : MonoBehaviour where T: BaseModel
{
    public T Model { get; set; }

    [UsedImplicitly]
    public virtual void Update()
    {
        if (Model.Dirty)
        {
            DirtyUpdate();
        }
    }

    protected abstract void DirtyUpdate();
}
