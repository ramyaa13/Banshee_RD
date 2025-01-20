using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Head Config", menuName = "Head Configuration")]
public class HeadConfigData : ScriptableObject
{
   public List<HeadConfig> HeadConfigs;
}

[Serializable]
public class HeadConfig
{
    public int GlassesIndex;
    public bool IsMask;
    public int hairIndex;
    public bool IsCrown;
    public bool IsHolo;
    public int eyeIndex;
}