using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Linq;

public class MovementScript : MonoBehaviour {

    //map tile graph for distance calculations and pathfinding
    static private Node[,] graph;


    //---DIJKSTRA
    //node distances
    private Dictionary<Node, float> dist;
    //node parents
    private Dictionary<Node, Node> prev;
    //---DIJKSTRA


    //---PLACEHOLDERS
    //map tile placeholder
    public class TileMapPlaceholder
    {
        public int xSize;
        public int ySize;
        public TilePlaceholder[,] tiles;
        public TileMapPlaceholder()
        {
            xSize = 10;
            ySize = 10;
            tiles = new TilePlaceholder[xSize, ySize];
            for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    tiles[x, y] = new TilePlaceholder(x, y);
                }
            }
        }
    }

    //map tile placeholder
    public class TilePlaceholder
    {
        public int x;
        public int y;
        public int difficulty;

        public TilePlaceholder(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }
    //---PLACEHOLDERS



    //graph node object
    class Node
    {
        public int x;
        public int y;
        public List<Node> neighbours;

        public Node(int X, int Y)
        {
            x = X;
            y = Y;
            neighbours = new List<Node>();
        }

        //TODO: tilemap tile weight? distance measure?
        public float DistanceTo(Node n)
        {
            return 1;
        }
    }


    //run at init once BuildPathGraph(TileMapPlaceholder tileMap);
    //build graph from map tiles
    public void BuildPathGraph(TileMapPlaceholder tileMap) //hex column zig-zag
    {
        //initialize graph nodes
        graph = new Node[tileMap.xSize, tileMap.ySize];
        for (int x = 0; x < tileMap.xSize; x++){
            for (int y = 0; y < tileMap.ySize; y++){
                graph[x, y] = new Node(x, y);
            }
        }
        
        //initialize graph relations
        for (int x = 0; x < tileMap.xSize; x++){
            for (int y = 0; y < tileMap.ySize; y++){
                if (x > 0) graph[x, y].neighbours.Add(graph[x - 1, y]);
                if (x < tileMap.xSize - 1) graph[x, y].neighbours.Add(graph[x + 1, y]);
                if (y > 0) graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < tileMap.xSize - 1) graph[x, y].neighbours.Add(graph[x, y + 1]);
            }
        }
    }

    //Calculates movement distances from current location and resturns all possible target tiles
    public List<TilePlaceholder> CalculateMovement(TileMapPlaceholder tileMap, TilePlaceholder currentTile, int moveDist)
    {
        Dijkstra(graph, currentTile.x, currentTile.y, moveDist);

        List<TilePlaceholder> possible_tiles = new List<TilePlaceholder>();
        foreach (Node node in graph)
        {
            if (prev[node] != null) possible_tiles.Add(tileMap.tiles[node.x, node.y]);
        }

        return possible_tiles;
    }

    //Find tile based path to target location
    public List<TilePlaceholder> FindPathTo(TileMapPlaceholder tileMap, TilePlaceholder targetTile)
    {
        Node targetNode = graph[targetTile.x, targetTile.y];
        //Debug.Log(targetNode);
        List<Node> path = NodePathfind(targetNode);
        List<TilePlaceholder> tilePath = new List<TilePlaceholder>();
        foreach (Node node in path){
            tilePath.Add(tileMap.tiles[node.x,node.y]);
        }
        //tilePath.Reverse();
        return tilePath;
    }

    //Move unit to location
    public void MoveToTile(List<TilePlaceholder> travelPath, UnitScript unit)
    {
        List<Vector3> vectorPath = new List<Vector3>();
        foreach (TilePlaceholder tile in travelPath)
        {
            vectorPath.Add(new Vector3(tile.x, tile.y, 0));
        }

        unit.Move(vectorPath);
        //Vector3 Start = new Vector3(unit.xPos, unit.yPos,0);
        //Vector3 End = new Vector3(targetTile.x, targetTile.y,0);
        //float lerp = 0;
        //while (lerp < 1){
        //    Debug.Log(lerp);
        //    lerp += Time.deltaTime * unit.moveAnimationSpeed;
        //    unit.Move(new Vector3(targetTile.x, targetTile.y, 0));
        //}

    }

    //traverse through dijkstra tree to target location
    private List<Node> NodePathfind(Node targetNode)
    {
        List<Node> path = new List<Node>();
        while (true){
            targetNode = prev[targetNode];
            if (targetNode == null) break;
            path.Add(targetNode);
        }
        return path;
    }




    //create dijkstra tree from current position up to distance
    private void Dijkstra(Node[,] graph, int x, int y, int moveDist)
    {
        //node distances
        dist = new Dictionary<Node, float>();
        //node parents
        prev = new Dictionary<Node, Node>();

        //unvisited nodes
        List<Node> Q = new List<Node>();

        //starting node
        Node source = graph[x, y];
        dist[source] = 0;
        prev[source] = null;

        //initialize infinity distance for each node
        foreach (Node vertex in graph){
            if (vertex != source){
                dist[vertex] = Mathf.Infinity;
                prev[vertex] = null;
            }
            Q.Add(vertex);
        }

        //traverse Q and calculate distance for each node
        while (Q.Count > 0){
            //closest node
            //Node U = Q.OrderBy(n => dist[n]).First(); //slow
            Node u = Q[0];
            Q.Remove(u);

            //break algorithm if reached max movement distance
            if (dist[u] > moveDist && dist[u] < Mathf.Infinity) return;

            
            foreach (Node vertex in u.neighbours) {
                float alt = dist[u] + u.DistanceTo(vertex);
                if (alt < dist[vertex]) {
                    dist[vertex] = alt;
                    prev[vertex] = u;

                    Q.Remove(vertex);
                    for (int i = 0; i < Q.Count; i++) {
                        if (dist[Q[i]] > alt) {
                            Q.Insert(i, vertex);
                            break;
                        }
                    }

                }
            }
        }
    }



	
}
