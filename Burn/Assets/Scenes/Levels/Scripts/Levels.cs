using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum LevelNames
{
    undefined,
    Level_01,
    Level_02
}

public class Levels : MonoBehaviour
{
    private Dictionary<LevelNames, CellManager> levelsDictionary;
    private CellManager currentLevel;

    private void OnEnable()
    {
        levelsDictionary = new Dictionary<LevelNames, CellManager>();
        CoreConnector.Levels = this;
    }

    public void RegisterLevel(CellManager level)
    {
        var levelKey = level.GetID();
        // store the level in a dictionary
        if (levelsDictionary.ContainsKey(levelKey))
        {
            // already have a reference
        }
        else
        {
            levelsDictionary.Add(levelKey, level);
        }
    }

    public void UnRegisterLevel(CellManager level)
    {
        var levelKey = level.GetID();
        if (!levelsDictionary.ContainsKey(levelKey))
        {
            return;
        }

        levelsDictionary.Remove(levelKey);
        Debug.Log("UnRegisterLevel successful :" + levelKey);
    }

    public void HideAllLevels()
    {
        foreach (var cellManager in levelsDictionary)
        {
            var level = cellManager.Value;
            level.DisableRenderers();
            level.DisableColliders();
        }
    }

    public void ShowLevel(LevelNames levelName)
    {
        HideAllLevels();
        if (levelsDictionary.TryGetValue(levelName, out var foundLevel))
        {
            currentLevel = foundLevel;
            currentLevel.Reset();
            currentLevel.EnableColliders();

            currentLevel.RegisterShipMarkers();
            CoreConnector.CoreGameControl.ResetGame();
        }
        else
        {
            // couldn't find level
            Debug.Log("ERROR! Can't find it");
        }
    }

    public Vector3 GetFinishLineStopPosition()
    {
        // find the current level
        return currentLevel.GetFinishLineStopPosition();
    }

    public Vector3 GetLevelEndLocation()
    {
        // find the current level
        return currentLevel.GetLevelEndLocation();
    }

    public void UpdateLoop()
    {
        currentLevel.UpdateLoop();
    }

    public void Reset()
    {
        currentLevel.Reset();
    }

    private void SetupLevel()
    {
        currentLevel.SetupLevel();
    }

    public EndCell GetEndCell()
    {
        return currentLevel.GetEndCell();
    }

    public void EnableRenderers()
    {
        currentLevel.EnableRenderers();
    }

    public void ResetForNewGame()
    {
        Reset();
        SetupLevel();
        UpdateLoop();
    }
}