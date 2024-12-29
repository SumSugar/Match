using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "DestinyLibrarySO", menuName = "Destiny/DestinyLibrarySO")]

public class DestinyLibrarySO : ScriptableObject
{
    public List<DestinyLibraryEntry> destinyLibraryList;
}

[System.Serializable]
public struct DestinyLibraryEntry
{
    public DestinyDataSO destinyData;
    public int amount;
}
