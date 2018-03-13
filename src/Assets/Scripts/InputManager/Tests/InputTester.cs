using System.Collections;
using AnotherRTS.Management.RemappableInput;
using UnityEngine;

public class InputTester : MonoBehaviour {

    [SerializeField]
    TestKey[] keys;

    [SerializeField]
    InputManager manager;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].Init(manager);
        }
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].Update(manager);
        }
    }

    [System.Serializable]
    public class TestKey
    {
        [SerializeField]
        string name;
        int id;

        public void Init(InputManager manager)
        {
            id = manager.GetKeyID(name);
            Debug.Log(name + " has ID: " + id);
        }

        public void Update(InputManager manager)
        {
            if (manager.GetKeyDown(id))
                Debug.Log(name + " Pressed!");
            if (manager.GetKey(id))
                Debug.Log(name + " Held!");
            if (manager.GetKeyUp(id))
                Debug.Log(name + " Released!");
        }
    }
}
