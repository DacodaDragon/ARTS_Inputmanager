using System.Collections.Generic;
using AnotherRTS.Management.RemappableInput.IO;

namespace AnotherRTS.Management.RemappableInput
{
    public class KeyBindingFactory
    {
        private int m_IdIndex = 0;
        private Dictionary<string, int> m_KeyNameID = new Dictionary<string, int>();
        private ModifierKeyRegister m_modifierRegister = new ModifierKeyRegister();

        public KeyBindingDatabase Build(KeyGroupData[] data)
        {
            m_KeyNameID.Clear();
            m_IdIndex = 0;

            KeyGroup[] ControlGroups = new KeyGroup[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                ControlGroups[i] = CreateControlGroup(data[i]);
            }
            return new KeyBindingDatabase(ControlGroups, m_KeyNameID, m_modifierRegister);
        }

        private KeyGroup CreateControlGroup(KeyGroupData group)
        {
            Key[] keys = new Key[group.keys.Length];
            for (int i = 0; i < group.keys.Length; i++)
            {
                keys[i] = CreateKey(group.keys[i]);
            }
            return new KeyGroup(group.name,keys);
        }

        private Key CreateKey(KeyData key)
        {
            m_KeyNameID.Add(key.name, m_IdIndex);
            m_modifierRegister.Add(key.modifiers);
            return new Key(m_IdIndex++,key.name, key.keycodes, key.modifiers, m_modifierRegister);
        }
    }
}
