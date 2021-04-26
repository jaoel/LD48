using System.Collections.Generic;
using UnityEngine;

namespace LD48 {
    public class TerrainManager : MonoBehaviour {
        public static TerrainManager Instance { get; private set; } = null;

        [System.Serializable]
        public class SegmentDefinition {
            public TerrainSegmentAsset segment = null;
            public int repeatCountMin = 1;
            public int repeatCountMax = 1;
        }

        public class Segment {
            public GameObject parent = null;
            public TileSegment tileSegment = null;
            public ShaftSegment shaftSegment = null;
            public Color startColor;
            public Color endColor;
            public bool unminable = false;
        }

        private const float tileHeight = 100f;
        private const float tileWidth = 20f;
        private const float drillHeight = 16f;

        private static MaterialPropertyBlock mpb = null;

        public List<SegmentDefinition> segmentDefinitions = new List<SegmentDefinition>();
        public Color startColor = Color.white;
        public ShaftSegment drillSegment = null;
        public Transform depthObject = null;
        public bool debugControls = false;

        private float currentDepth = -drillHeight;
        private float dugDepth = -drillHeight;
        private List<Segment> segments = new List<Segment>();

        private void Awake() {
            Instance = this;
            if (mpb == null) {
                mpb = new MaterialPropertyBlock();
            }
        }

        private void Start() {
            GenerateSegments(100);
        }

        private void Update() {
            if (debugControls) {
                if (Input.GetKey(KeyCode.UpArrow)) {
                    depthObject.transform.position -= Vector3.up * 50f * Time.deltaTime;
                }

                if (Input.GetKey(KeyCode.DownArrow)) {
                    depthObject.transform.position += Vector3.up * 50f * Time.deltaTime;
                }
            }

            currentDepth = depthObject.position.y;
            dugDepth = Mathf.Max(currentDepth, dugDepth);

            Shader.SetGlobalFloat("_MineshaftDepth", -(dugDepth - currentDepth));

            transform.position = Vector3.up * currentDepth;

            UpdateDrillSegment();

            DoCulling();
        }

        private void DoCulling() {
            int currentDepthIndex = (int)(currentDepth / tileHeight);
            for (int i = 0; i < segments.Count; i++) {
                Segment segment = segments[i];
                bool segmentActive = Mathf.Abs(i - currentDepthIndex) <= 1;
                if (segment.parent.activeSelf != segmentActive) {
                    segment.parent.SetActive(segmentActive);
                }
            }
        }

        private void UpdateDrillSegment() {
            int currentDepthIndex = (int)(dugDepth / tileHeight);
            if (currentDepthIndex >= 0 && currentDepthIndex < segments.Count) {
                Segment currentSegment = segments[currentDepthIndex];
                float localDepth = Mathf.Repeat(dugDepth, tileHeight) / tileHeight;
                float localDrillHeight = drillHeight / tileHeight;
                mpb.SetColor("_StartColor", currentDepthIndex == 0 ? currentSegment.endColor : Color.Lerp(currentSegment.startColor, currentSegment.endColor, localDepth));
                mpb.SetColor("_EndColor", Color.Lerp(currentSegment.startColor, currentSegment.endColor, localDepth + localDrillHeight));
                drillSegment.dynamicRenderer.SetPropertyBlock(mpb);
            }

            drillSegment.transform.localPosition = -Vector3.up * dugDepth;
        }

        public bool TryGetCurrentSegment(out Segment currentSegment) {
            int currentDepthIndex = (int)(currentDepth / tileHeight);
            if (currentDepthIndex >= 0 && currentDepthIndex < segments.Count) {
                currentSegment = segments[currentDepthIndex];
                return true;
            }

            currentSegment = null;
            return false;
        }

        private void GenerateSegments(int maxDepth = 100) {
            int depth = 0;
            Color previousColor = startColor;

            bool AddSegment(TerrainSegmentAsset terrainSegmentAsset) {
                Segment segment = InstantiateSegment(terrainSegmentAsset, depth, previousColor);
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

                    previousColor = segmentDefinition.segment.color;
                }
            }
        }

        private Segment InstantiateSegment(TerrainSegmentAsset segmentAsset, int depthIndex, Color previousColor) {
            GameObject parentObject = new GameObject($"Segment - {depthIndex}");

            parentObject.transform.SetParent(transform);
            parentObject.transform.position = new Vector3(0f, -tileHeight * (depthIndex + 1), 0f);

            if (segmentAsset.availableResources != null && segmentAsset.availableResources.Count > 0) {

                int orePerSide = Random.Range(5, 20);

                for (int i = 0; i < orePerSide; i++) {
                    GameObject go = Instantiate(segmentAsset.availableResources[Random.Range(0, segmentAsset.availableResources.Count)].gameObject, parentObject.transform);
                    go.transform.localPosition = new Vector3(12 + Random.Range(0, 20), 0 + Random.Range(0.0f, 100.0f), 1.5f);
                    go.transform.localRotation = Random.rotation;
                }

                for (int i = 0; i < orePerSide; i++) {
                    GameObject go = Instantiate(segmentAsset.availableResources[Random.Range(0, segmentAsset.availableResources.Count)].gameObject, parentObject.transform);
                    go.transform.localPosition = new Vector3(-12 - Random.Range(0, 20), 0 + Random.Range(0.0f, 100.0f), 1.5f);
                    go.transform.localRotation = Random.rotation;
                }
            }


            mpb.SetColor("_StartColor", previousColor);
            mpb.SetColor("_EndColor", segmentAsset.color);

            TileSegment tileSegment = Instantiate(segmentAsset.tilePrefab, parentObject.transform);
            tileSegment.transform.localPosition = Vector3.zero;
            tileSegment.transform.localRotation = Quaternion.identity;
            tileSegment.transform.localScale = Vector3.one;
            tileSegment.dynamicRenderer.SetPropertyBlock(mpb);

            ShaftSegment shaftSegment = Instantiate(segmentAsset.shaftPrefab, parentObject.transform);
            shaftSegment.transform.localPosition = Vector3.zero;
            shaftSegment.transform.localRotation = Quaternion.identity;
            shaftSegment.transform.localScale = Vector3.one;
            shaftSegment.dynamicRenderer.SetPropertyBlock(mpb);

            Segment segment = new Segment();
            segment.parent = parentObject;
            segment.tileSegment = tileSegment;
            segment.shaftSegment = shaftSegment;
            segment.startColor = previousColor;
            segment.endColor = segmentAsset.color;
            segment.unminable = segmentAsset.unminable;
            return segment;
        }
    }
}