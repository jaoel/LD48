using UnityEngine;

namespace LD48 {
    [CreateAssetMenu(fileName = "New Terrain Segment", menuName = "LD48/Terrain Segment")]
    public class TerrainSegmentAsset : ScriptableObject {
        public TileSegment tilePrefab = null;
        public ShaftSegment shaftPrefab = null;
        public Color startColor;
        public Color endColor;
    }
}
