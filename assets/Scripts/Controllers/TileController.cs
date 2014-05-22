using UnityEngine;
using System.Collections;

/*
 * Odyssey Game
 * 
 * Tile Controller
 * Version: 0.12
 * Author: Thomas Auberson
 * 
 * This script handles the world tiles and their generation.
 * 
 * It stores a list of potential tiles that can be generated. 
 * These must be stored as WorldTile# in /WorldTiles/Resources/ (v0.12)
 * 
 * Current functionality of Tile Controller allows it to detect existence (or not) of current tiles
 * and randomly generate new ones. (v0.12)
 * 
 * Existing tiles are stored in 4 seperate 2D arrays each representing a quadrant of the coordinate system (NE, SE, SW, NW)
 * This is to allow tiles to be stored and indexed with negative coordinates allowing player to move in any direction. 
 */

public class TileController : MonoBehaviour {
	
	// CONSTANTS
	public int GENERATION_RADIUS = 2; // Sets radius of tiles around current tile that will be generated
	public int INITIAL_MAP_DIMENSIONS = 4; // Sets starting size of map dimensions. 	
	// Initial Map array will be sqaure with dimensions double this value. e.g. value of 4 => 8x8 map
	public int BLANK_MAP_TILE_TYPE_NUM = 0; // The Tile Type Number for a blank tile of water (Will be used for starting tile)
	public int SCALE = 50; // Size of tiles
	public int NUM_TILE_TYPES = 5; // Current Number of Possible Tile Types to Choose From
	
	// VARIABLES
	private Map map;
	private System.Random random;
	private GameObject[] tileTypes;	// List of Different Tile Types Available Indexed by their Tile Type Number
	// NOTE: Index=0 should be the type of tile Player starts on (a Blank Tile??)
	
	
	// Use this for initialization
	void Start () {
		// Initialize Tile Types
		tileTypes = new GameObject[NUM_TILE_TYPES];
		for (int i = 0; i<NUM_TILE_TYPES; i++) {
			tileTypes[i]=(GameObject)(Resources.Load("WorldTile"+i)); // All the Tiles Named WorldTile# from the WorldTiles/Resources Folder will be added to list
			
		}
		random = new System.Random ();
		
		// Initialize Map
		map = new Map (INITIAL_MAP_DIMENSIONS);
		map.AddTileToMap (0, 0, BLANK_MAP_TILE_TYPE_NUM); // Set starting tile
		
		// Generate Immediate Neighbours
		PlaceNeighbours (0, 0, GENERATION_RADIUS);		
	}
	
	public int GenerateTileType(){
		return random.Next (NUM_TILE_TYPES);
	}
	
	public GameObject GetTileByTypeNum(int typeNum){
		return tileTypes [typeNum];
	}
	
	public void TileEntryAt(float px, float py) { 
		//Debug.Log ("Tile Entry At: "+px + " , " + py);
		px = px / SCALE;
		py = py / SCALE;
		int x = (int)px;
		int y = (int)py;
		PlaceNeighbours (x, y, GENERATION_RADIUS);
	}
	
	private void PlaceNeighbours(int x, int y, int radiusDepth) { // Triggered Upon Player entering tile at coord (x,y)
		//Debug.Log ("Tile Entered At: "+x + " , " + y);
		// Check neighbouring tiles in all directions within the radiusDepth
		// Generate and add new tiles
		
		for (int i = x-radiusDepth; i <= x+radiusDepth; i++) {
			for (int j = y-radiusDepth; j <= y+radiusDepth; j++) {
				int tileType = GenerateTileType();
				MapNode mapNode = map.AddTileToMap (i, j, tileType);
				if(mapNode != null)	// Check Tile. If no tile is present add a new one
					mapNode.SetObject(PlaceTileInWorld(i, j, mapNode.GetTileNum())); 	// Place the new tile on physical map
			}
		}
		
		// Remove past neighbours from gameworld
		for (int i = x-radiusDepth-1; i <= x+radiusDepth+1; i++) {
			MapNode mp = map.GetTileAt(i,y-radiusDepth-1);
			if(mp!=null){
				Destroy (mp.GetObject());
				mp.ClearObject();
			}
			mp = map.GetTileAt(i,y+radiusDepth+1);
			if(mp!=null){
				Destroy (mp.GetObject());
				mp.ClearObject();
			}
		}
		for (int j = y-radiusDepth; j <= y+radiusDepth; j++) {
			MapNode mp = map.GetTileAt(x-radiusDepth-1,j);
			if(mp!=null){
				Destroy (mp.GetObject());
				mp.ClearObject();
			}
			mp = map.GetTileAt(x+radiusDepth+1,j);
			if(mp!=null){
				Destroy (mp.GetObject());
				mp.ClearObject();
			}
		}
		
	}
	
	public GameObject PlaceTileInWorld(int x, int y, int tileType) {
		//Debug.Log ("Placed At: "+x + " , " + y);
		return (GameObject)Instantiate (GetTileByTypeNum(tileType), new Vector3(SCALE*x, 0, SCALE*y), Quaternion.identity);
	}
	
	// Clears a tile from game world. NOTE: The tile will still be stored in map
	public void RemoveTileFromWorld(int x, int y){
		MapNode mapNode = map.GetTileAt (x, y);
		mapNode.ClearObject ();
	}
}

public class Map {
	// This Clas Holds a map of MapNodes
	// The map can take positive and negative coordinates
	// This is done by subdividing map into 4 quadrants NE, SE, NW, SW
	// Positive X values are treated as E values
	// Positive Y values are treated as N values
	// Negative X values are treated as W values
	// Negative Y values are treated as S values
	
	// NOTE: Coordinate values of 0 are treated as positive. e.g. coord (0,0) will go into NE quadrant E=0, N=0 
	
	// Divide Map into 4 Quadrants and store in 4 arrays 
	private MapNode[,] mapNE;	// Coord (+,+) indexed as mapNE[E][N]
	private MapNode[,] mapSE;	// Coord (+,-) indexed as mapSE[E][S]
	private MapNode[,] mapNW;	// Coord (-,+) indexed as mapNW[W][N]
	private MapNode[,] mapSW;	// Coord (-,-) indexed as mapSW[W][S]
	
	// Map Dimensions - Map expands like an ArrayList
	private int sizeN;	// Coord (_,+)
	private int sizeS;	// Coord (_,-)
	private int sizeE;	// Coord (+,_)
	private int sizeW;	// Coord (-,_)
	
	
	public Map(int initialSize){
		// Set Initial Map Dimensions
		sizeN = initialSize;
		sizeS = initialSize;
		sizeE = initialSize;
		sizeW = initialSize;
		
		// Initialise Quadrant Arrays
		mapNE = new MapNode[sizeE,sizeN];
		mapSE = new MapNode[sizeE,sizeS];
		mapNW = new MapNode[sizeW,sizeN];
		mapSW = new MapNode[sizeW,sizeS];
	}
	
	// CHECK WHETHER TILE AT COORD (x,y) exists
	public bool DoesTileExist(int x, int y){
		// If coord (+,+) Check NE Quadrant
		if (x >= 0 & y >= 0) { 
			int e = x; // Convert x coord to east index
			int n = y; // Convert y coord to north index
			if (e >= sizeE || n >= sizeN)
				return false;
			if (mapNE[e,n] == null)
				return false;
			return true;
		}
		
		// If coord (+,-) Check SE Quadrant
		if (x >= 0 & y < 0) {
			int e = x; // Convert x coord to east index
			int s = 0 - y; // Convert y coord to south index
			if (e >= sizeE || s >= sizeS)
				return false;
			if (mapSE[e,s] == null)
				return false;
			return true;
		}
		
		// If coord (-,+) Check NW Quadrant
		if (x < 0 & y >= 0) { 
			int w = 0 - x; // Convert x coord to west index
			int n = y; // Convert y coord to north index
			if (w >= sizeW || n >= sizeN)
				return false;
			if (mapNW[w,n] == null)
				return false;
			return true;
		}
		
		// If coord (-,-) Check SW Quadrant
		if (x < 0 & y < 0) { 
			int w = 0 - x; // Convert x coord to west index
			int s = 0 - y; // Convert y coord to south index
			if (w >= sizeW || s >= sizeS)
				return false;
			if (mapSW[w,s] == null)
				return false;
			return true;
		}
		
		// Invalid Input - Return False
		return false;
	}
	
	// Returns the MapNode for the tile at given coord. Rturns null if does not exist yet
	public MapNode GetTileAt(int x, int y){
		if (!DoesTileExist (x,y))
			return null;
		
		if (x >= 0 & y >= 0) { 	// Tile in NE Quadrant
			int e = x; // Convert x coord to east index
			int n = y; // Convert y coord to north index	
			return mapNE[e,n];
		}
		if (x >= 0 & y < 0) { 	// Tile in SE Quadrant
			int e = x; // Convert x coord to east index
			int s = 0 - y; // Convert y coord to south index	
			return mapSE[e,s];
		}
		if (x < 0 & y >= 0) { // Tile in NW Quadrant	
			int w = 0 - x; // Convert x coord to west index
			int n = y; // Convert y coord to north index
			return mapNW[w,n];
		}
		if (x < 0 & y < 0) { // Tile in SW Quadrant	
			int w = 0 - x; // Convert x coord to west index
			int s = 0 - y; // Convert y coord to south index
			return mapSW[w,s];
		}
		return null;
	}
	
	// ADD A TILE OF TILE TYPE NUMBER tileNum AT COORD (x,y)
	// Return MapNode if successful, Return null and reject addition if tile already present at (x,y)
	public MapNode AddTileToMap(int x, int y, int tileNum){
		//Debug.Log ("Add Tile At: "+x + " , " + y+ "???");
		// First Check whether or not tile already exists
		if (DoesTileExist (x, y)) {
			return RegenerateTile(x,y);
		}
		
		// Make sure map is large enough - Expand if necessary
		if (x >= sizeE) {
			ExpandEast();
		}
		if (y >= sizeN) {
			ExpandNorth();
		}
		if ((0-x) >= sizeW ) { // Compare -x to sizeW
			ExpandWest();
		}
		if ((0-y) >= sizeS) { // Compare -y to sizeS
			ExpandSouth();
		}
		
		// Add Tile to relvant quadrant
		if (x >= 0 & y >= 0) { 	// Tile in NE Quadrant
			int e = x; // Convert x coord to east index
			int n = y; // Convert y coord to north index		
			mapNE[e,n] = new MapNode(tileNum);
			return mapNE[e,n];
		}
		if (x >= 0 & y < 0) { 	// Tile in SE Quadrant
			int e = x; // Convert x coord to east index
			int s = 0 - y; // Convert y coord to south index			
			mapSE[e,s] = new MapNode(tileNum);
			return mapSE[e,s];
		}
		if (x < 0 & y >= 0) { // Tile in NW Quadrant	
			int w = 0 - x; // Convert x coord to west index
			int n = y; // Convert y coord to north index			
			mapNW[w,n] = new MapNode(tileNum);
			return mapNW[w,n];
		}
		if (x < 0 & y < 0) { // Tile in SW Quadrant	
			int w = 0 - x; // Convert x coord to west index
			int s = 0 - y; // Convert y coord to south index			
			mapSW[w,s] = new MapNode(tileNum);
			return mapSW[w,s];
		}
		
		return null;
	}
	
	public MapNode RegenerateTile(int x, int y){
		if (x >= 0 & y >= 0) { 	// Tile in NE Quadrant
			int e = x; // Convert x coord to east index
			int n = y; // Convert y coord to north index		
			if(!mapNE[e,n].IsActive())
				return mapNE[e,n];
		}
		if (x >= 0 & y < 0) { 	// Tile in SE Quadrant
			int e = x; // Convert x coord to east index
			int s = 0 - y; // Convert y coord to south index			
			if(!mapSE[e,s].IsActive())
				return mapSE[e,s];
		}
		if (x < 0 & y >= 0) { // Tile in NW Quadrant	
			int w = 0 - x; // Convert x coord to west index
			int n = y; // Convert y coord to north index			
			if(!mapNW[w,n].IsActive())
				return mapNW[w,n];
		}
		if (x < 0 & y < 0) { // Tile in SW Quadrant	
			int w = 0 - x; // Convert x coord to west index
			int s = 0 - y; // Convert y coord to south index			
			if(!mapSW[w,s].IsActive())
				return mapSW[w,s];
		}
		return null;
	}
	
	// EXPAND MAP DIMENSIONS IN DIRECTION (NORTH, SOUTH, EAST, WEST)
	public void ExpandEast(){
		//Debug.Log ("EXPAND EAST");
		int oldSizeE = sizeE;
		sizeE = sizeE * 2;
		ReloadNE (oldSizeE, sizeN);
		ReloadSE (oldSizeE, sizeS);
	}
	
	public void ExpandWest(){
		//Debug.Log ("EXPAND WEST");
		int oldSizeW = sizeW;
		sizeW = sizeW * 2;
		ReloadNW (oldSizeW, sizeN);
		ReloadSW (oldSizeW, sizeS);
	}
	
	public void ExpandNorth(){
		//Debug.Log ("EXPAND NORTH");
		int oldSizeN = sizeN;
		sizeN = sizeN * 2;
		ReloadNE (sizeE, oldSizeN);
		ReloadNW (sizeW, oldSizeN);
	}
	
	public void ExpandSouth(){
		//Debug.Log ("EXPAND SOUTH");
		int oldSizeS = sizeS;
		sizeS = sizeS * 2;
		ReloadSE (sizeE, oldSizeS);
		ReloadSW (sizeW, oldSizeS);
	}
	
	
	// RELOAD MAP QUADRANTS WITH NEW SIZES
	public void ReloadNE(int oldSizeE, int oldSizeN){
		MapNode[,] newMapNE = new MapNode[sizeE,sizeN];
		
		// Copy old map to new one
		for(int x = 0; x < oldSizeE; x++){
			for(int y = 0; y < oldSizeN; y++){
				newMapNE[x,y] = mapNE[x,y];
			}
		}
		
		// Set new Map
		mapNE = newMapNE;	
	}
	
	public void ReloadSE(int oldSizeE, int oldSizeS){
		MapNode[,] newMapSE = new MapNode[sizeE,sizeS];
		
		// Copy old map to new one
		for(int x = 0; x < oldSizeE ; x++){
			for(int y = 0; y < oldSizeS; y++){
				newMapSE[x,y] = mapSE[x,y];
			}
		}
		
		// Set new Map
		mapSE = newMapSE;	
	}
	
	public void ReloadNW(int oldSizeW, int oldSizeN){
		MapNode[,] newMapNW = new MapNode[sizeW,sizeN];
		
		// Copy old map to new one
		for(int x = 0; x < oldSizeW; x++){
			for(int y = 0; y < oldSizeN; y++){
				newMapNW[x,y] = mapNW[x,y];
			}
		}
		
		// Set new Map
		mapNW = newMapNW;	
	}
	
	public void ReloadSW(int oldSizeW, int oldSizeS){
		MapNode[,] newMapSW = new MapNode[sizeW,sizeS];
		
		// Copy old map to new one
		for(int x = 0; x < oldSizeW ; x++){
			for(int y = 0; y < oldSizeS; y++){
				newMapSW[x,y] = mapSW[x,y];
			}
		}
		
		// Set new Map
		mapSW = newMapSW;	
	}
}

// MAPNODE CLASS STORES RELEVANT INFORMATION ABOUT TILES
public class MapNode {
	
	public int tileNum;	// Type Number of Tile
	GameObject gameObject = null;
	
	public MapNode(int tileNum){
		this.tileNum = tileNum;
	}
	
	//	public MapNode(int tileNum,GameObject gameObject ){
	//		this.tileNum = tileNum;
	//		this.gameObject = gameObject;
	//	}
	
	public void SetObject(GameObject gameObject){
		this.gameObject = gameObject;
	}
	
	public void ClearObject(){
		this.gameObject = null;
	}
	
	public GameObject GetObject(){
		return gameObject;
	}
	
	public int GetTileNum(){
		return tileNum;
	}
	
	// Return true if tile currently exists in game world
	public bool IsActive(){
		return gameObject != null;
	}
}
