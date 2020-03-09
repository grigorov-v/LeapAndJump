using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public LevelBlock LevelBlockPrototype = null;
    public int        CountBlocks         = 4;

    LevelBlock _lastLevelBlock = null;

    void Start() {
        _lastLevelBlock = LevelBlockPrototype;
        for ( var i = 0; i < CountBlocks; i++ ) {
            var newBlock = Instantiate(LevelBlockPrototype);
            newBlock.transform.position = _lastLevelBlock.EndPoint.position - newBlock.BeginPoint.localPosition;
            _lastLevelBlock = newBlock;
        }
    }
}