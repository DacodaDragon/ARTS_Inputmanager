using System.Collections.Generic;
using System;
using UnityEngine;

namespace AnotherRTS.Management.RemappableInput
{
    public class KeyBindingDatabase
    {
        private const string ERR_001 = "No KeyGroup containing keybinding [{0}] exists!";
        private const string ERR_002 = "Method [{0}] did not subscribe to [{1}] because the keybinding doesn't exist";
        private const string ERR_003 = "Keybinding with name [{0}] wasn't found";
        private const string ERR_004 = "KeyGroup with name [{0}] does not exist!";

        private ModifierKeyRegister m_ModifierRegister;
        private Dictionary<string, int> m_KeyNameToGroupID;
        private Dictionary<int, KeyGroup> m_GroupIDDict;
        private Dictionary<string, KeyGroup> m_GroupNameDict;
        private KeyGroup[] m_Groups;
        private List<KeyGroup> m_active = new List<KeyGroup>(10);
        private List<KeyGroup> m_inactive = new List<KeyGroup>(10);
        private string[] m_KeyNames;

        public string[] KeyNames { get { return m_KeyNames; } }

        private void PerformOnAll<t>(t[] array, Action<t> function)
        {
            for (int i = 0; i < array.Length; i++)
            {
                function(array[i]);
            }
        }

        private void PerformOnAll<t>(List<t> array, Action<t> function)
        {
            for (int i = 0; i < array.Count; i++)
            {
                function(array[i]);
            }
        }

        public KeyBindingDatabase(KeyGroup[] groups, Dictionary<string, int> m_KeyDict, ModifierKeyRegister register)
        {
            m_Groups = groups;

            for (int i = 0; i < m_Groups.Length; i++)
            {
                m_active.Add(m_Groups[i]);
            }

            m_GroupNameDict = new Dictionary<string, KeyGroup>(groups.Length);
            PerformOnAll(m_Groups, (KeyGroup x) => { m_GroupNameDict.Add(x.Name, x); });

            m_KeyNameToGroupID = m_KeyDict;
            SetControlGroups(groups);
            m_ModifierRegister = register;
        }

        private void SetControlGroups(KeyGroup[] groups)
        {
            m_GroupIDDict = new Dictionary<int, KeyGroup>();
            // Link all key ID's to their respective control groups
            // So we can find them back quickly later.
            for (int i = 0; i < groups.Length; i++)
            {
                int[] IDs = groups[i].GetAllKeyIDs();
                for (int j = 0; j < IDs.Length; j++)
                {
                    m_GroupIDDict.Add(IDs[j], groups[i]);
                }
            }
        }

        private KeyGroup FindContainingGroup(int id)
        {
            KeyGroup group;

            if (!m_GroupIDDict.TryGetValue(id, out group))
                throw new System.Exception(string.Format(ERR_001, id));

            return group;
        }

        private KeyGroup FindKeyGroup(string name)
        {
            for (int i = 0; i < m_Groups.Length; i++)
            {
                if (m_Groups[i].Name == name)
                    return m_Groups[i];
            }
            throw new System.Exception(string.Format(ERR_004, name));
        }

        public void KeyUp(KeyCode keycode)
        {
            m_ModifierRegister.KeyUp(keycode);
            PerformOnAll(m_active, x => x.KeyUp(keycode));
        }

        public void KeyDown(KeyCode keycode)
        {
            m_ModifierRegister.KeyDown(keycode);
            PerformOnAll(m_active, x => x.KeyDown(keycode));
        }

        public bool GetKeyUp(int id)
        {
            KeyGroup group = FindContainingGroup(id);
            if (group.Active)
                return group.GetKey(id).IsReleased;
            else return false;
        }

        public bool GetKey(int id)
        {

            KeyGroup group = FindContainingGroup(id);
            if (group.Active)
                return group.GetKey(id).IsHeld;
            else return false;

        }

        public bool GetKeyDown(int id)
        {
            KeyGroup group = FindContainingGroup(id);
            if (group.Active)
                return group.GetKey(id).IsPressed;
            else return false;

        }

        public int GetKeyID(string name)
        {
            int id;
            if (!m_KeyNameToGroupID.TryGetValue(name, out id))
                throw new System.Exception(string.Format(ERR_003, name));
            return id;
        }

        public void Start()
        {
            m_KeyNames = GetAllKeyNames();
            for (int i = 0; i < m_Groups.Length; i++)
            {
                m_Groups[i].Start();
            }
        }

        private string[] GetAllKeyNames()
        {
            List<string> keynames = new List<string>();
            for (int i = 0; i < m_Groups.Length; i++)
            {
                for (int j = 0; j < m_Groups[i].Keys.Length; j++)
                {
                    keynames.Add(m_Groups[i].Keys[j].Name);
                }
            }
            return keynames.ToArray();
        }

        public Key GetInteralKey(int id)
        {
            KeyGroup group = FindContainingGroup(id);
            for (int i = 0; i < group.Keys.Length; i++)
            {
                if (group.Keys[i].ID == id)
                    return group.Keys[i];
            }
            return null;
        }

        public Key GetInteralKey(string name)
        {
            return GetInteralKey(GetKeyID(name));
        }

        public void SubscribeKeyUp(string name, VoidDelegate method)
        {
            GetInteralKey(name).OnKeyUp += method;
        }

        public void SubscribeKeyDown(string name, VoidDelegate method)
        {
            GetInteralKey(name).OnKeyDown += method;
        }

        public void UnsubscribeKeyUp(string name, VoidDelegate method)
        {
            GetInteralKey(name).OnKeyUp -= method;
        }

        public void UnsubscribeKeyDown(string name, VoidDelegate method)
        {
            GetInteralKey(name).OnKeyDown -= method;
        }

        public void SetActiveGroup(string name, bool active)
        {
            KeyGroup group;
            m_GroupNameDict.TryGetValue(name, out group);

            if (!active)
            {
                if (m_active.Contains(group))
                {
                    group.SilentKill();
                    m_inactive.Add(group);
                    m_active.Remove(group);
                }
            }
            else
            {

                if (m_inactive.Contains(group))
                {
                    m_active.Add(group);
                    m_inactive.Remove(group);
                }
            }
        }
    }
}
