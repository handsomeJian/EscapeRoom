using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelObjectManager : MonoBehaviour
{
    //public LevelObjectComponent[] LevelObjectPrefabList;

    public LevelObjectComponent[] LevelObjectList;

    private Dictionary<LevelObjectComponent, Guid> _objectToIdDict = new Dictionary<LevelObjectComponent, Guid>();

    public static LevelObjectManager Instance;

    private SpatialAnchorLoader _anchorLoader;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        _anchorLoader = FindAnyObjectByType<SpatialAnchorLoader>();
        _anchorLoader._onLoadAnchor = OnAnchorLoad;

        LoadLevelObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevelObjects()
    {
        LevelObjectList = FindObjectsOfType<LevelObjectComponent>();

        var idList = new List<Guid>();
        foreach (var obj in LevelObjectList)
        {
            var keyName = "ObjName:" + obj.name;
            if (PlayerPrefs.HasKey(keyName))
            {
                var existingId = new Guid(PlayerPrefs.GetString(keyName));
                _objectToIdDict[obj] = existingId;
                idList.Add(existingId);
            }
        }
        _anchorLoader.LoadAnchorsByUuid(idList.ToArray());
    }

    public LevelObjectComponent GetLevelObjectWithId(Guid anchorId)
    {
        
        foreach (var key in _objectToIdDict.Keys)
        {
            if (_objectToIdDict[key] == anchorId)
            {
                return key;
            }
        }
        return null;
    }

    public void OnAnchorLoad(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success)
    {
        if (!success)
        {
            Log($"{unboundAnchor} Localization failed!");
            return;
        }

        var levelObj = GetLevelObjectWithId(unboundAnchor.Uuid);
        if (levelObj == null)
        {
            Log($"{unboundAnchor} does not have related level obj!");
            return;
        }

        levelObj.hasAnchor = true;
        levelObj.AnchorID = unboundAnchor.Uuid;

        var newAnchor = levelObj.gameObject.AddComponent<OVRSpatialAnchor>();
        unboundAnchor.BindTo(newAnchor);
    }

    public void RemoveSpatialAnchor(LevelObjectComponent levelObj)
    {
        if (!levelObj.hasAnchor) return;

        var objAnchor = levelObj.GetComponent<OVRSpatialAnchor>();
        if (!objAnchor) return;

        objAnchor.Erase((anchor, success) =>
        {
            if (success)
            {
                levelObj.hasAnchor = false;
                Destroy(anchor);
            }
        });
    }

    public void CreateSpatialAnchor(LevelObjectComponent levelObj)
    {
        if (levelObj.hasAnchor || levelObj.GetComponent<OVRSpatialAnchor>() != null) return;

        OVRSpatialAnchor newAnchor = levelObj.gameObject.AddComponent<OVRSpatialAnchor>();
        StartCoroutine(anchorCreated(newAnchor));
        levelObj.hasAnchor = true;
    }

    public IEnumerator anchorCreated(OVRSpatialAnchor anchor)
    {
        // keep checking for a valid and localized spatial anchor state
        while (!anchor.Created && !anchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }

        //when ready, save the spatial anchor using OVRSpatialAnchor.Save()
        anchor.Save((anchor, success) =>
        {
            if (!success) return;

            SaveUuidToPlayerPrefs(anchor);
        });
    }

    void SaveUuidToPlayerPrefs(OVRSpatialAnchor anchor)
    {
        var levelObjComp = anchor.GetComponent<LevelObjectComponent>();
        var keyName = "ObjName:" + levelObjComp.name;

        PlayerPrefs.SetString(keyName, anchor.Uuid.ToString());
    }

    private static void Log(string message) => Debug.Log($"[SpatialAnchorsUnity]: {message}");
}
