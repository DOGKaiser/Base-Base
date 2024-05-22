using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseGameDataMgr {
    protected static BaseGameDataMgr instance;
    
    public static BaseGameDataMgr Instance {
        get { return instance; }
    }
    
    
    public GameDataScriptableObject<StatConfig> Stats = new GameDataScriptableObject<StatConfig>();

    // ------------------------------------------------------

    protected BaseGameDataMgr() {
        LoadData();
    }
    
    public static void Init() {
        instance = new BaseGameDataMgr();
        
    }
    
    protected virtual void LoadData() {
        DataHolderGameData.Instance.LoadDataHolderGameData();
        
        Stats.Init("Stats");
    }
}
