using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tiled2Unity;
using UnityEditor;

[Tiled2Unity.CustomTiledImporter]
class CustomImporterAddComponent : Tiled2Unity.ICustomTiledImporter
{
    public void HandleCustomProperties(UnityEngine.GameObject gameObject,
        IDictionary<string, string> props)
    {
        // Simply add a component to our GameObject
        if (props.ContainsKey("PrefabName"))
        {
            Debug.Log("Wow");

            var prefabPath = string.Format("Assets/Prefabs/{0}.prefab", props["PrefabName"]);
            var spawn = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject));
            var spawnInstance = (GameObject)Object.Instantiate(spawn);
            spawnInstance.name = spawn.name;

            // Use the position of the game object we're attached to
            spawnInstance.transform.parent = gameObject.transform;
            spawnInstance.transform.localPosition = Vector3.zero;

            // gameObject.AddComponent(props["AddComp"]);
        }
    }

    public void CustomizePrefab(GameObject prefab)
    {
        // Do nothing
    }
}