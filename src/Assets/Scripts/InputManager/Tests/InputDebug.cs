using UnityEngine;
//using UnityEngine.UI;
using AnotherRTS.Management.RemappableInput;
using TMPro;

public class InputDebug : MonoBehaviour
{
    InputManager m_InputManager;

    [SerializeField]
    TMP_Text m_TextElement;

    Key[] keys;

    void Start()
    {
        m_InputManager = InputManager.Instance;
        keys = BuildKeys(m_InputManager.KeyNames);
    }

    // Update is called once per frame
    void Update()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int i = 0; i < keys.Length; i++)
        {
            sb.Append(keys[i].Update()).Append('\n');
        }

        m_TextElement.text = sb.ToString();
    }

    public Key[] BuildKeys(string[] names)
    {
        Key[] keys = new Key[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            keys[i] = new Key(names[i]);
        }
        return keys;
    }

    public class Key
    {
        private InputManager manager;
        private string name;
        private int id;

        public string Update()
        {
            if (manager.GetKeyUp(id))
                return "[" + name + "]\tpressed";
            if (manager.GetKey(id))
                return "[" + name + "]\theld";
            if (manager.GetKeyDown(id))
                return "[" + name + "]\treleased";
            return "[" + name + "]\tinactive";
        }

        public Key(string name)
        {
            manager = InputManager.Instance;
            id = manager.GetKeyID(name);
            this.name = name;
        }
    }
}
