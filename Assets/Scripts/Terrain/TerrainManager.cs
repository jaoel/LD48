using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class TerrainManager : MonoBehaviour {
        [System.Serializable]
        public class SegmentDefinition {
            public TerrainSegmentAsset segment = null;
            public TerrainSegmentAsset transitionSegment = null;
            public int repeatCountMin = 1;
            public int repeatCountMax = 1;
        }

        public class Segment {
            public GameObject parent = null;
            public TileSegment tileSegment = null;
            public ShaftSegment shaftSegment = null;
        }

        private const float tileHeight = 100f;

        public List<SegmentDefinition> segmentDefinitions = new List<SegmentDefinition>();

        private float currentDepth = 0f;
        private float dugDepth = 0f;
        private List<Segment> segments = new List<Segment>();

        private void Start() {
            GenerateSegments(10);
        }

        private void Update() {
            if (Input.GetKey(KeyCode.UpArrow)) {
                currentDepth -= 50f * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.DownArrow)) {
                currentDepth += 50f * Time.deltaTime;
            }

            dugDepth = Mathf.Max(currentDepth, dugDepth);
            Shader.SetGlobalFloat("_MineshaftDepth", -(dugDepth - currentDepth));

            transform.position = Vector3.up * currentDepth;

            DoCulling();
        }

        private void DoCulling() {
            int currentDepthIndex = (int)(currentDepth / tileHeight) + 1;
            for (int i = 0; i < segments.Count; i++) {
                Segment segment = segments[i];
                bool segmentActive = Mathf.Abs(i - currentDepthIndex) <= 1;
                if (segment.parent.activeSelf != segmentActive) {
                    segment.parent.SetActive(segmentActive);
                }
            }
        }

        private void GenerateSegments(int maxDepth = 100) {
            int depth = 0;

            bool AddSegment(TerrainSegmentAsset terrainSegmentAsset) {
                Segment segment = InstantiateSegment(terrainSegmentAsset, depth);
                segments.Add(segment);
                depth++;

                // Early out if too deep
                if (depth > maxDepth) {
                    return true;
                }

                return false;
            }

            foreach (SegmentDefinition segmentDefinition in segmentDefinitions) {
                int count = Mathf.Max(1, Random.Range(segmentDefinition.repeatCountMin, segmentDefinition.repeatCountMax + 1));
                for (int i = 0; i < count; i++) {
                    if (AddSegment(segmentDefinition.segment)) {
                        return;
                    }
                }

                if (segmentDefinition.transitionSegment != null) {
                    if (AddSegment(segmentDefinition.transitionSegment)) {
                        return;
                    }
                }
            }
        }

        private Segment InstantiateSegment(TerrainSegmentAsset segmentAsset, int depthIndex) {
            GameObject parentObject = new GameObject($"Segment - {depthIndex}");
            parentObject.transform.SetParent(transform);
            parentObject.transform.position = new Vector3(0f, -tileHeight * depthIndex, 0f);
            
            TileSegment tileSegment = Instantiate(segmentAsset.tilePrefab, parentObject.transform);
            tileSegment.transform.localPosition = Vector3.zero;
            tileSegment.transform.localRotation = Quaternion.identity;
            tileSegment.transform.localScale = Vector3.one;

            ShaftSegment shaftSegment = Instantiate(segmentAsset.shaftPrefab, parentObject.transform);
            shaftSegment.transform.localPosition = Vector3.zero;
            shaftSegment.transform.localRotation = Quaternion.identity;
            shaftSegment.transform.localScale = Vector3.one;

            Segment segment = new Segment();
            segment.parent = parentObject;
            segment.tileSegment = tileSegment;
            segment.shaftSegment = shaftSegment;
            return segment;
        }
    }
}