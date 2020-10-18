using UnityEngine;

[ExecuteInEditMode]
public class Level_02 : CellManager
{
    protected override void OnEnableFunction()
    {
        base.OnEnableFunction();

        var counter = 0;

        PositionCells(startCells, ref counter);
        PositionCells(randomizedCells, ref counter);

        // position the end cell, there is only one
        position.z = GameSettings.CellLength * counter;
        endCell.Position(position);
    }
    
    public override LevelNames GetID()
    {
        return LevelNames.Level_02;
    }
}