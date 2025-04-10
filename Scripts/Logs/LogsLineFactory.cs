using System;
using CustomAttributes;
using UnityEngine;

namespace Logs
{
    public class LogsLineFactory : MonoBehaviour
    {
        public static LogsLineFactory Instance { get; private set; }
        public Vector2 _connectionMinMaxValues;
        [SerializeField, NotNull] private GameObject _arrow;
        
        private Color[] _colorPalette = new Color[]
        {
            new Color32(17, 95, 154, 50),   // #115f9a
            new Color32(25, 132, 197, 255),  // #1984c5
            new Color32(34, 167, 240, 255),  // #22a7f0
            new Color32(72, 181, 196, 255),  // #48b5c4
            //new Color32(118, 198, 143, 255), // #76c68f
            new Color32(166, 215, 91, 255),  // #a6d75b
            new Color32(201, 229, 47, 255),  // #c9e52f
            new Color32(208, 238, 17, 255),  // #d0ee11
            new Color32(208, 244, 0, 255)    // #d0f400
        };

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void InitializeFactory(Vector2 connectionMinMaxValues)
        {
            _connectionMinMaxValues = connectionMinMaxValues;
        }

        public GameObject CreateLine(LogsDataStructures.ObjectsConnection connection)
        {
            GameObject line = new GameObject();

            line.name = connection._firstObjName + "-" + connection._secondObjName;
            
            line.AddComponent<LineRenderer>();
            
            var lineRenderer = line.GetComponent<LineRenderer>();
            
            //Color of line
            Material thisLineMaterial = new Material(Shader.Find("Sprites/Default"));
            thisLineMaterial.SetColor("_Color", GetColorFromPalette(connection._howManyTimes));
            lineRenderer.material = thisLineMaterial;
            
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            
            lineRenderer.positionCount = 3;
            
            Vector3 middlePoint = (connection._firstObjPosition + connection._secondObjPosition) / 2;
            
            lineRenderer.SetPosition(0, connection._firstObjPosition);
            lineRenderer.SetPosition(1, middlePoint);
            lineRenderer.SetPosition(2, connection._secondObjPosition);
            
            Vector3 direction = (connection._secondObjPosition - connection._firstObjPosition).normalized;
            
            var arrow = CreateArrow(middlePoint, direction, thisLineMaterial);
            
            Material arrowMaterial = new Material(Shader.Find("Sprites/Default"));
            
            Color originalColor = GetColorFromPalette(connection._howManyTimes);
            
            Color newColor = new Color(1f, 0f, 0f, originalColor.a); // Czerwony z oryginalną przezroczystością

            arrowMaterial.SetColor("_Color", newColor);
            arrow.GetComponent<Renderer>().material = arrowMaterial;

            
            return line;
        }

        private GameObject CreateArrow(Vector3 position, Vector3 direction, Material material)
        {
            var arrow = Instantiate(_arrow, position, Quaternion.identity);
            arrow.transform.rotation = Quaternion.LookRotation(direction);
            var arrowRenderer = arrow.GetComponent<Renderer>();
            if (arrowRenderer != null)
            {
                //arrowRenderer.material = material;
            }

            return arrow;
        }
        
        private Color GetColorFromPalette(float value)
        {
            float t = Mathf.InverseLerp(_connectionMinMaxValues.x, _connectionMinMaxValues.y, value);
            int index = Mathf.FloorToInt(t * (_colorPalette.Length - 1));
            return _colorPalette[index];
        }
    }
}
