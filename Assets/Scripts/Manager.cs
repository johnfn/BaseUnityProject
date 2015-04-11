using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
    public GameObject Tile;

    public GameObject Root;

    public static Manager Instance;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Creates a generic game object. Use this instead of Initialize.
    /// </summary>
    /// <returns>The game object.</returns>
    /// <param name="position">The position of the object.</param>
    /// <param name="baseObject">Base object.</param>
    private static GameObject CreateGameObject(Vector3 position, GameObject baseObject)
    {
        var result = (GameObject)Instantiate(baseObject, position, Quaternion.identity);
        result.transform.parent = Instance.Root.transform;

        return result;
    }

    public static Tile CreateTile()
    {
        var cell = CreateGameObject(Vector3.zero, Instance.Tile);

        return cell.GetComponent<Tile>();
    }
}
