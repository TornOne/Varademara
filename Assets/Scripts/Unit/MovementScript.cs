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


    private Tile gizmos_current_tile;
    public List<Tile> gizmos_possible_tiles;

    //public static Map tileMap;

    /*
    //---PLACEHOLDERS
    //map tile placeholder
    public class TileMapPlaceholder
    {
        public int xSize;
        public int ySize;
        public TilePlaceholder[,] tiles;
        public TileMapPlaceholder(int X, int Y)
        {
            xSize = X;
            ySize = Y;
            tiles = new TilePlaceholder[xSize, ySize];
            for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    tiles[x, y] = new TilePlaceholder(x, y);
                }
            }

            foreach (TilePlaceholder tile in tiles) tile.neighbours = GetNeighbours(tile.x, tile.y);
        }

        private List<TilePlaceholder> GetNeighbours(int x, int y)
        {
            List<TilePlaceholder> neighbours = new List<TilePlaceholder>();
            if (x > 0) neighbours.Add(tiles[x - 1, y]);
            if (x < xSize - 1) neighbours.Add(tiles[x + 1, y]);
            if (y > 0) neighbours.Add(tiles[x, y - 1]);
            if (y < xSize - 1) neighbours.Add(tiles[x, y + 1]);

            return neighbours;
        }
    }

    //map tile placeholder
    public class TilePlaceholder
    {
        public int x;
        public int y;
        public int difficulty = 1;
        public List<TilePlaceholder> neighbours;
        public Unit unit;

        public TilePlaceholder(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public Vector3 Position()
        {
            return new Vector3(x-0.5f,y-0.5f,0);
        }

        public float DistanceTo(TilePlaceholder target)
        {
            if (target.unit != null) return float.PositiveInfinity;
            return difficulty;
        }

    }*/
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
            if (Map.map == null) return 1;

            //TODO: bad assumption here
            if (Map.map.tiles[n.y].Length <= n.x) return float.MaxValue;

            return Map.map.tiles[y][x].DistanceTo(Map.map.tiles[n.y][n.x]);
        }
    }


    //run at init once BuildPathGraph(TileMapPlaceholder tileMap);
    //build graph from map tiles
    public static void BuildPathGraph() //hex column zig-zag
    {
        Map.map = Map.map;
        //initialize graph nodes
        graph = new Node[Map.map.width, Map.map.height];
        for (int x = 0; x < Map.map.width; x++){
            for (int y = 0; y < Map.map.height; y++){
                graph[x, y] = new Node(x, y);
            }
        }

        //initialize graph relations
        foreach (Tile[] row in Map.map.tiles)
        {
            foreach (Tile tile in row)
            {
                List<Node> node_neighbours = new List<Node>();
                foreach (Tile neightbour in tile.neighbors) node_neighbours.Add(graph[neightbour.x, neightbour.y]);
                graph[tile.x, tile.y].neighbours = node_neighbours;
            }
        }
    }

    //Calculates movement distances from current location and resturns all possible target tiles
    public List<Tile> CalculateMovement(Tile currentTile, int moveDist)
    {
        Dijkstra(graph, currentTile.x, currentTile.y, moveDist);

        List<Tile> possible_tiles = new List<Tile>();
        foreach (Node node in graph)
        {
            if (dist[node] <= moveDist) possible_tiles.Add(Map.map.tiles[node.y][node.x]);
        }

        gizmos_possible_tiles = possible_tiles;
        gizmos_current_tile = currentTile;

        return possible_tiles;
    }

    //Find tile based path to target location
    public List<Tile> FindPathTo(Tile targetTile)
    {
        Node targetNode = graph[targetTile.x, targetTile.y];
        //Debug.Log(new Vector2(targetNode.x, targetNode.y));
        List<Node> path = NodePathfind(targetNode);
        //print(path.Count);
        List<Tile> tilePath = new List<Tile>();
        foreach (Node node in path){
            tilePath.Add(Map.map.tiles[node.y][node.x]);
        }
        //tilePath.Reverse();
        return tilePath;
    }

    //Move unit to location
    public void MoveToTile(List<Tile> travelPath, Unit unit)
    {
        //unit.Move(travelPath);
    }

    //traverse through dijkstra tree to target location
    private List<Node> NodePathfind(Node targetNode)
    {
        List<Node> path = new List<Node>();
        path.Add(targetNode);
        while (true){
            targetNode = prev[targetNode];
            if (targetNode == prev[targetNode]) break;
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
        prev[source] = source;

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

                    if (alt <= moveDist) prev[vertex] = u;

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

    void OnDrawGizmos()
    {
        if (gizmos_possible_tiles == null) return;
        if (gizmos_current_tile == null) return;
        Gizmos.color = new Color(0, 0, 1, 0.1f);
        foreach (Tile tile in gizmos_possible_tiles) Gizmos.DrawLine(gizmos_current_tile.transform.position, tile.transform.position);
    }



}
