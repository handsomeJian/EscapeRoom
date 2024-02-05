using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelObjectManager : MonoBehaviour
{
    public LevelObjectComponent[] LevelObjectPrefabList;

    private Dictionary<LevelObjectComponent, Guid> _objectToIdDict;

    public List<LevelObjectComponent> LevelObjectList;

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
        var idList = new List<Guid>();
        foreach (var obj in LevelObjectPrefabList)
        {
            var keyName = "ObjName:" + obj.name;
            if (PlayerPrefs.HasKey(keyName))
            {
                var newId = new Guid(PlayerPrefs.GetString(keyName));
                _objectToIdDict[obj] = newId;
                idList.Add(newId);
            }
            else
            {
                var levelObj = Instantiate(obj);
                levelObj.hasAnchor = false;
                LevelObjectList.Add(levelObj);
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

        var obj = GetLevelObjectWithId(unboundAnchor.Uuid);
        if (obj == null)
        {
            Log($"{unboundAnchor} does not have related level obj!");
            return;
        }

        var pose = unboundAnchor.Pose;
        var levelObj = Instantiate(obj, pose.position, pose.rotation);

        levelObj.hasAnchor = true;
        levelObj.AnchorID = unboundAnchor.Uuid;

        var newAnchor = levelObj.gameObject.AddComponent<OVRSpatialAnchor>();
        unboundAnchor.BindTo(newAnchor);

        LevelObjectList.Add(levelObj);
    }

    private static void Log(string message) => Debug.Log($"[SpatialAnchorsUnity]: {message}");
}
