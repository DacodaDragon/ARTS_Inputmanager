using UnityEngine;
namespace AnotherRTS.Management.RemappableInput
{
    public class KeyGroup
    {
        private string m_name;
        private Key[] m_keys;
        private bool m_active = true;

        public string Name { get { return m_name; } }
        public Key[] Keys { get { return m_keys; } }
        public bool Active { get { return m_active; } set { m_active = value; } }

        public KeyGroup(string name, Key[] keys)
        {
            m_name = name;
            m_keys = keys;
        }

        public int[] GetAllKeyIDs()
        {
            int[] IDs = new int[m_keys.Length];
            for (int i = 0; i < m_keys.Length; i++)
            {
                IDs[i] = m_keys[i].ID;
            }
            return IDs;
        }

        public void KeyUp(KeyCode key)
        {
            for (int i = 0; i < m_keys.Length; i++)
            {
                m_keys[i].KeyDown(key);
            }
        }

        public void KeyDown(KeyCode key)
        {
            for (int i = 0; i < m_keys.Length; i++)
            {
                m_keys[i].KeyUp(key);
            }
        }

        public Key GetKey(int id)
        {
            for (int i = 0; i < m_keys.Length; i++)
            {
                if (m_keys[i].ID == id)
                    return m_keys[i];
            }
            return null;
        }

        public void Start()
        {
            for (int i = 0; i < m_keys.Length; i++)
            {
                m_keys[i].Start();
            }
        }

        public void SilentKill()
        {
            for (int i = 0; i < m_keys.Length; i++)
            {
                m_keys[i].SilentKill();
            }
        }
    }
}
