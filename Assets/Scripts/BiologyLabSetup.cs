using UnityEngine;

public class BiologyLabSetup : MonoBehaviour
{
    public GameObject tablePrefab;
    public int numberOfTables = 4;
    public Vector3 tableSize = new Vector3(2, 1, 1);
    public Vector3 labSize = new Vector3(10, 3, 10);

    void Start()
    {
        CreateRoom();
        CreateTables();
    }

    void CreateRoom()
    {
        // Create floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.localScale = new Vector3(labSize.x / 10, 1, labSize.z / 10);
        floor.name = "Floor";

        // Create walls
        CreateWall(new Vector3(0, labSize.y / 2, labSize.z / 2), new Vector3(labSize.x, labSize.y, 0.1f)); // Back wall
        CreateWall(new Vector3(0, labSize.y / 2, -labSize.z / 2), new Vector3(labSize.x, labSize.y, 0.1f)); // Front wall
        CreateWall(new Vector3(labSize.x / 2, labSize.y / 2, 0), new Vector3(0.1f, labSize.y, labSize.z)); // Right wall
        CreateWall(new Vector3(-labSize.x / 2, labSize.y / 2, 0), new Vector3(0.1f, labSize.y, labSize.z)); // Left wall
    }

    void CreateWall(Vector3 position, Vector3 scale)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.name = "Wall";
    }

    void CreateTables()
    {
        for (int i = 0; i < numberOfTables; i++)
        {
            Vector3 tablePosition = new Vector3(
                (i % 2 == 0 ? -1 : 1) * (labSize.x / 4),
                tableSize.y / 2,
                (i / 2) * (labSize.z / (numberOfTables / 2)) - (labSize.z / 4)
            );
            GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cube);
            table.transform.position = tablePosition;
            table.transform.localScale = tableSize;
            table.name = $"Table_{i + 1}";
        }
    }
}
