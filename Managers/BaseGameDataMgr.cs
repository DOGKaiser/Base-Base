using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameDataMgr {


    // ------------------------------------------------------

    public virtual void Init() {
        DataHolderGameData.Instance.LoadDataHolderGameData();


    }
}
