using System.Collections.Generic;
using UnityEngine;

using EventsHelper;
using NaughtyAttributes;

public class LevelGenerator : MonoBehaviour {
    public int                MinCountBlocks = 4;
    [ReorderableList]
    public List<LevelBlock>   Blocks         = new List<LevelBlock>();
    
    [ReorderableList][Space]
    public List<LevelElement> LevelElements = new List<LevelElement>();

    List<LevelBlock> _activeBlocks = new List<LevelBlock>();

    LevelBlock LastBlock {
        get {
            if ( _activeBlocks.Count == 0 ) {
                return null;
            }
            
            return _activeBlocks[_activeBlocks.Count - 1];
        }
    }

    void Awake() {
        for ( var i = 0; i < MinCountBlocks; i++ ) {
            CreateNewBlock();
        }

        EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
    }

    void OnDestroy() {
        EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.G) ) {
            Test();
        }
    }

    void CreateNewBlock() {
        var rand = Random.Range(0, Blocks.Count);
        
        var newBlock = Blocks[rand];
        newBlock = Instantiate(newBlock);
       
        var position = Vector2.zero;
        if ( LastBlock ) {
            position = LastBlock.EndPoint.position - newBlock.BeginPoint.localPosition;
        }

        newBlock.transform.position = position;
        newBlock.GenerateLevelElements(LevelElements);
        _activeBlocks.Add(newBlock);
    }

    void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e) {
        if ( LastBlock && (LastBlock == e.LevelBlock) ) {
            var firstBlock = _activeBlocks[0];
            _activeBlocks.Remove(firstBlock);
            Destroy(firstBlock.gameObject);

            CreateNewBlock();
        }
    }

    void Test() {
        var firstBlock = _activeBlocks[0];
        _activeBlocks.Remove(firstBlock);
        Destroy(firstBlock.gameObject);
        CreateNewBlock();
    }
}