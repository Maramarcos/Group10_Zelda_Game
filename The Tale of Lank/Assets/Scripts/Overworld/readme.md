This is the documentation for the Map Editor and Map System.

Map System
----------
The map data is stored in a data specific class used for saving and loading. All world data is to be stored in the appropiate dataObject. If a data relates to the entire world then it is to be stored in WorldData, if it relates to a entire map then it goes in MapData, etc.
There are seperate gameobjects that display the data in the map data files. These gameobjects DO NOT have a copy of the data.
  they contain the indexes and ids that point towards the data. There is only one world instance that is a static variable


Basic Structure:
If you are familiar with minecraft's world system, this functions similarly.
    There is only one world, each world has a set of maps (like minecraft dimensions.). 
    Each map contains chunks. Each chunk contains tiles.     
    There are a unlimited amount of maps for a world
    There are a unlimited amount of chunks per map. The chunks start from (0,0) and extend to (+inf, +inf).
    Each chunk contains 16 by 11 tiles.    
    A Tile is collection of sprites, collision, and behavior. Most tiles have no behavior
    Chunks are loaded and unloaded based on how far away they are from the current player. This is to not cause lag by having the     
      entire map loaded at once.
    
General GameObject Logic:
A world object is initialized first. It loads the worldData file into the static worldData class.
The world object then creates a map gameobject for each map in the world file. It tells the object the map id.
The map objects then get the data from the static worldData class and creates the chunkObjects like how the world creates maps
The chunk object then behaves like map and world where it creates the proper tileObjects.
The tile object then sets its sprite.

MapEditor Logic:
The MapEditor Gameobject handles logic for selecting the pallet (what tile we want to place).
The Tile GameObject contains logic for knowing if it was clicked on so that it can change the data on itself and in the static worldData variable.

