using UnityEngine;
using UnityEditor;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "New Shop Items DB", menuName = "Create Shop Items DB", order = 0)]
    public class ShopItemsDB : ScriptableObject
    {
        [SerializeField] private ShopItemConfig[] _configs;
        
        public ShopItemConfig[] Configs => _configs;
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(ShopItemsDB))]
    public class ShopItemsDBEditor : Editor
    {
        private ShopItemsDB _database;
        
        private void OnEnable()
        {
            _database = (ShopItemsDB)target;
        }
        
        public override void OnInspectorGUI()
        {
            // Рисуем стандартный инспектор
            DrawDefaultInspector();
            
            // Добавляем отступ
            GUILayout.Space(10);
            
            // Рисуем кнопку
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Auto-Assign IDs to All Items", GUILayout.Height(30)))
            {
                AutoAssignIDs();
            }
            GUI.backgroundColor = Color.white;
            
            // Дополнительная кнопка для сброса ID (опционально)
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Reset All IDs to -1", GUILayout.Height(25)))
            {
                ResetAllIDs();
            }
            GUI.backgroundColor = Color.white;
            
            // Кнопка для проверки дубликатов
            GUI.backgroundColor = Color.cyan;
            if (GUILayout.Button("Check for Duplicate IDs", GUILayout.Height(25)))
            {
                CheckForDuplicateIDs();
            }
            GUI.backgroundColor = Color.white;
        }
        
        private void AutoAssignIDs()
        {
            if (_database.Configs == null)
            {
                Debug.LogWarning("ShopItemsDB: Configs array is null!");
                return;
            }
            
            int assignedCount = 0;
            Undo.RecordObject(_database, "Auto-Assign IDs");
            
            for (int i = 0; i < _database.Configs.Length; i++)
            {
                if (_database.Configs[i] != null)
                {
                    // Используем SerializedObject для правильного сохранения
                    SerializedObject so = new SerializedObject(_database.Configs[i]);
                    SerializedProperty idProperty = so.FindProperty("Id"); // Предполагается, что в ShopItemConfig есть приватное поле _id
                    
                    if (idProperty != null)
                    {
                        idProperty.intValue = i;
                        so.ApplyModifiedProperties();
                        assignedCount++;
                    }
                    else
                    {
                        Debug.LogWarning($"ShopItemsDB: Can't find _id field in {_database.Configs[i].name}");
                    }
                }
            }
            
            EditorUtility.SetDirty(_database);
            Debug.Log($"ShopItemsDB: Successfully assigned IDs to {assignedCount} items.");
        }
        
        private void ResetAllIDs()
        {
            if (_database.Configs == null) return;
            
            Undo.RecordObject(_database, "Reset All IDs");
            
            int resetCount = 0;
            for (int i = 0; i < _database.Configs.Length; i++)
            {
                if (_database.Configs[i] != null)
                {
                    SerializedObject so = new SerializedObject(_database.Configs[i]);
                    SerializedProperty idProperty = so.FindProperty("Id");
                    
                    if (idProperty != null)
                    {
                        idProperty.intValue = -1;
                        so.ApplyModifiedProperties();
                        resetCount++;
                    }
                }
            }
            
            EditorUtility.SetDirty(_database);
            Debug.Log($"ShopItemsDB: Reset IDs to -1 for {resetCount} items.");
        }
        
        private void CheckForDuplicateIDs()
        {
            if (_database.Configs == null)
            {
                Debug.LogWarning("Configs array is null!");
                return;
            }
            
            System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<string>> idMap = 
                new System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<string>>();
            
            foreach (var config in _database.Configs)
            {
                if (config != null)
                {
                    // Получаем ID через рефлексию или сериализацию
                    SerializedObject so = new SerializedObject(config);
                    SerializedProperty idProperty = so.FindProperty("Id");
                    
                    if (idProperty != null)
                    {
                        int id = idProperty.intValue;
                        if (id != -1) // Игнорируем неинициализированные ID
                        {
                            if (!idMap.ContainsKey(id))
                                idMap[id] = new System.Collections.Generic.List<string>();
                            
                            idMap[id].Add(config.name);
                        }
                    }
                }
            }
            
            bool hasDuplicates = false;
            foreach (var kvp in idMap)
            {
                if (kvp.Value.Count > 1)
                {
                    hasDuplicates = true;
                    Debug.LogError($"Duplicate ID {kvp.Key} found in items: {string.Join(", ", kvp.Value)}");
                }
            }
            
            if (!hasDuplicates)
            {
                Debug.Log("No duplicate IDs found!");
            }
        }
    }
    #endif
}