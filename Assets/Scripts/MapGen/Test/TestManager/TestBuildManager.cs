using System;
using System.Collections;
using System.Collections.Generic;
using Save;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBuildManager : MonoBehaviour {
    public static TestBuildManager Instance = null;
    
    public string filePath;
    
    public TestBuildManagerWindow window;

    [Header("Map Data")] 
    [SerializeField] private Vector2Int _mapSize;
    [SerializeField] private float _mapHeight;
    public GameObject _squarePrefab;

    [Header("Progression System")] 
    [SerializeField] private List<ObjectWrapper> _allMapableObjects = new();
    
    [Header("Build System")]
    private Transform _buildParents;

    [Header("Map Generation System")] 
    [SerializeField] private List<PreGenObjectWrapper> _objectsToPregen = new ();
    
    public SaveHandler saveHandler;
    public MapData mapData;
    public TestBuildProgression progressionSystem;
    public BuildingSystem buildingSystem;
    public MapGenerationSystem mapGenerationSystem;

    private void Start() {
        if (Instance != null) return;
        Instance = this;
        
        _buildParents = new GameObject("BuildingParents").transform;
        
        saveHandler = new SaveHandler();
        saveHandler.LoadData(filePath, out MapSave _mapSave);
        
        mapData = new MapData(_mapSize.x, _mapSize.y, _mapHeight, _buildParents);
        progressionSystem = new TestBuildProgression(_allMapableObjects);
        buildingSystem = new BuildingSystem(ref mapData, ref  progressionSystem, ref _buildParents);
        mapGenerationSystem = new MapGenerationSystem(ref buildingSystem, _mapSize);

        string generationCallback = "Map Generation is ";
        if (_mapSave != null) {
            generationCallback += mapGenerationSystem.SavedMapGeneration(_mapSave.SavedObjects).ToString();
        } else {
            generationCallback += mapGenerationSystem.RandomMapGeneration(_objectsToPregen).ToString();
        }
        
        Debug.Log(generationCallback);
    }
    
    public Coroutine StartChildCoroutine(IEnumerator _coroutineMethod) {
        return StartCoroutine(_coroutineMethod);
    }
    
    public void StopChildCoroutine(IEnumerator _coroutineMethod) {
        StopCoroutine(_coroutineMethod);
    }

    public void GenerateMap() {
        mapData.DeleteMap();
        mapGenerationSystem.RandomMapGeneration(_objectsToPregen);
    }

    public void OnAttack(InputAction.CallbackContext _context) {
        if (_context.started) {
            MapSave _save = new MapSave(mapData);
            Debug.Log(_save._savedObjects.Count);
            saveHandler.Save(filePath, _save);
            
            Debug.Log("Clicked");
            // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // RaycastHit hit;
            //
            // if (Physics.Raycast(ray, out hit)) {
            //     Vector2Int coordOnMap = new Vector2Int((int)hit.collider.transform.position.x, (int)hit.collider.transform.position.z);
            //     mapData.TryDeleteCell(coordOnMap.x, coordOnMap.y);
            //     Debug.Log("Deleted : " +  coordOnMap);
            // }
        }
    }

    void OnDrawGizmos()
    {
        if (mapData == null) return;
        mapData.OnDrawGizmos();
    }
}