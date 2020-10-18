using UnityEngine;

public class RightWorldSidePiece : WorldSidePieceBase
{
	[SerializeField]
	protected NonCollisionObjectBase[] rubblePrefabs;

	protected override NonCollisionObjectBase GetPrefabToSpawn()
	{
		var randomChoice = Random.Range(0, rubblePrefabs.Length);
		var prefab = rubblePrefabs[randomChoice];
		return prefab;
	}

	public override void Randomize()
	{

	}
}
