using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchWay : MonoBehaviour
{
    [SerializeField]
    GameObject sphere1;
    [SerializeField]
    GameObject sphere2;
    [SerializeField]
    GameObject sphere3;
    [SerializeField]
    GameObject sphere4;
    [SerializeField]
    GameObject sphere5;
    [SerializeField]
    GameObject sphere6;

    GameObject cube;

    Vector3 sphere1Position;
    Vector3 sphere2Position;
    Vector3 sphere3Position;
    Vector3 sphere4Position;
    Vector3 sphere5Position;
    Vector3 sphere6Position;

    Vector3 rotationAxisV;
    Vector3 directionV;
    Vector3 cylDefaultOrientation = new Vector3(0, 1, 0);

    float dist;

    // Start is called before the first frame update
    void Start()
    {
        sphere1 = GameObject.Find("Sphere1");
        sphere2 = GameObject.Find("Sphere2");
        sphere3 = GameObject.Find("Sphere3");
        sphere4 = GameObject.Find("Sphere4");
        sphere5 = GameObject.Find("Sphere5");
        sphere6 = GameObject.Find("Sphere6");

        sphere1Position = sphere1.transform.position;
        sphere2Position = sphere2.transform.position;
        sphere3Position = sphere3.transform.position;
        sphere4Position = sphere4.transform.position;
        sphere5Position = sphere5.transform.position;
        sphere6Position = sphere6.transform.position;

        CreateAMine(sphere1Position, sphere2Position);
        CreateAMine(sphere2Position, sphere6Position);
        CreateAMine(sphere2Position, sphere3Position);
        CreateAMine(sphere2Position, sphere4Position);
        CreateAMine(sphere3Position, sphere4Position);
        CreateAMine(sphere3Position, sphere5Position);
        CreateAMine(sphere4Position, sphere5Position);
        CreateAMine(sphere4Position, sphere6Position);
        CreateAMine(sphere5Position, sphere6Position);

        var g = new Graph();

        //добавление вершин
        g.AddVertex("Sphere1");
        g.AddVertex("Sphere2");
        g.AddVertex("Sphere3");
        g.AddVertex("Sphere4");
        g.AddVertex("Sphere5");
        g.AddVertex("Sphere6");

        //добавление ребер
        g.AddEdge("Sphere1", "Sphere2", Convert.ToInt32(Vector3.Distance(sphere1Position, sphere2Position)));
        g.AddEdge("Sphere2", "Sphere6", Convert.ToInt32(Vector3.Distance(sphere2Position, sphere6Position)));
        g.AddEdge("Sphere2", "Sphere3", Convert.ToInt32(Vector3.Distance(sphere2Position, sphere3Position)));
        g.AddEdge("Sphere2", "Sphere4", Convert.ToInt32(Vector3.Distance(sphere2Position, sphere4Position)));
        g.AddEdge("Sphere3", "Sphere4", Convert.ToInt32(Vector3.Distance(sphere3Position, sphere4Position)));
        g.AddEdge("Sphere3", "Sphere5", Convert.ToInt32(Vector3.Distance(sphere3Position, sphere5Position)));
        g.AddEdge("Sphere4", "Sphere5", Convert.ToInt32(Vector3.Distance(sphere4Position, sphere5Position)));
        g.AddEdge("Sphere4", "Sphere6", Convert.ToInt32(Vector3.Distance(sphere4Position, sphere6Position)));
        g.AddEdge("Sphere5", "Sphere6", Convert.ToInt32(Vector3.Distance(sphere5Position, sphere6Position)));

        var dijkstra = new Dijkstra(g);
        var path = dijkstra.FindShortestPath("Sphere1", "Sphere6");
        Debug.Log(path);
    }


    public void CreateAMine(Vector3 pointEnd, Vector3 pointStart)
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // ѕомещаем куб посередине (между двух точек (в нашем случае, двух сфер)).
        cube.transform.position = (pointEnd + pointStart) / 2.0F;

        // –ассчитываем нормализованный вектор.
        /* Vector3.Normalize Ч ¬озвращает вектор с тем же направлением, 
         * что и заданный вектор, но с длиной равной единице. */
        directionV = Vector3.Normalize(pointEnd - pointStart);

        // ќсь вращени€.
        rotationAxisV = directionV + cylDefaultOrientation;

        rotationAxisV = Vector3.Normalize(rotationAxisV);

        // Ќаходим угол поворота.
        cube.transform.rotation = new Quaternion(rotationAxisV.x, rotationAxisV.y, rotationAxisV.z, 0);

        // Scale        
        dist = Vector3.Distance(pointEnd, pointStart);

        // «адаЄм размер кубу.
        cube.transform.localScale = new Vector3(0.5f, dist, 0.5f);
    }
}