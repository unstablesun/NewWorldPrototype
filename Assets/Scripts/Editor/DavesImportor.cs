using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiled2Unity;


[Tiled2Unity.CustomTiledImporter]
class CustomImporterAddComponent : Tiled2Unity.ICustomTiledImporter
{
	public void HandleCustomProperties(UnityEngine.GameObject gameObject,
		IDictionary<string, string> props)
	{
		// Simply add a component to our GameObject
		if (props.ContainsKey("IsTrigger"))
		{
			Debug.Log("Handle custom properties from Tiled map");

			//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(gameObject, "Assets/Scripts/Editor/DavesImportor.cs (16,4)", props["IsTrigger"]);
		}
	}

	public void CustomizePrefab(GameObject prefab)
	{
		// Do nothing
		Debug.Log("Customize prefab = " + prefab.name);

		var boxColliders2D = prefab.GetComponentsInChildren<BoxCollider2D>();
		if (boxColliders2D == null) {
			Debug.Log ("no box colliders");
			return;
		}

		foreach (var box in boxColliders2D) {

			Debug.Log("setting box collider istrigger = true for " + box.name);
			box.enabled = true;
			box.isTrigger = true;


		}
	}
}
