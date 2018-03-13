using UnityEngine;
using AnotherRTS.Util;

namespace AnotherRTS.Management.RemappableInput
{
    public class ModifierKeyRegister
    {
        private KeyCode[] m_modifiers;
        private int[] m_referenceCount;
        private bool[] m_pressed;

        public void Add(params KeyCode[] keys)
        {
            if (m_modifiers == null)
            {
                m_modifiers = new KeyCode[0];
                m_referenceCount = new int[0];
                m_pressed = new bool[0];
            }

            for (int i = 0; i < keys.Length; i++)
            {
                if (!ArrayUtil.Contains(m_modifiers, keys[i]))
                {
                    m_modifiers = ArrayUtil.AddToArray(m_modifiers, keys[i]);
                    m_referenceCount = ArrayUtil.AddToArray(m_referenceCount, 1);
                    m_pressed = ArrayUtil.AddToArray(m_pressed, false);
                }
                else
                {
                    int index = ArrayUtil.FindFirstIndex(m_modifiers, keys[i]);
                    ++m_referenceCount[index];
                }
            }
        }

        public void Remove(params KeyCode[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (ArrayUtil.Contains(m_modifiers, keys[i]))
                {
                    int index = ArrayUtil.FindFirstIndex(m_modifiers, keys[i]);
                    if (m_referenceCount[index] > 1)
                    {
                        --m_referenceCount[index];
                    }
                    else
                    {
                        m_modifiers = ArrayUtil.RemoveAt(m_modifiers, index);
                        m_referenceCount = ArrayUtil.RemoveAt(m_referenceCount, index);
                        m_pressed = ArrayUtil.RemoveAt(m_pressed, index);
                    }
                    continue;
                }
                Debug.Log("Tried removing non existant Modifier Key: " + keys.ToString());
            }
        }

        public bool[] GetModifierCheckArray(KeyCode[] keys)
        {
            // Check if Modifier Keys exist
            for (int i = 0; i < keys.Length; i++)
            {
                if (!IsValidModifier(keys[i]))
                    new System.Exception("Modifier Key "
                        + keys[i].ToString() +
                        " doesn't exist!");
            }

            // Create new checklist
            bool[] checkArray = new bool[m_modifiers.Length];
            ArrayUtil.Fill(checkArray, false);

            // Make checklist
            for (int i = 0; i < keys.Length; i++)
            {
                int index = ArrayUtil.FindFirstIndex(m_modifiers, keys[i]);

                if (index == -1)
                {
                    Debug.LogError("ERROR! " + keys[i].ToString() +
                        " wasn't found in list:\n" + ToString());
                }

                checkArray[index] = true;
            }

            // Give back checklist
            return checkArray;
        }

        public bool KeyDown(KeyCode key)
        {
            if (!IsValidModifier(key))
                return false;

            int index = ArrayUtil.FindFirstIndex(m_modifiers, key);
            m_pressed[index] = true;

            return true;
        }

        public bool KeyUp(KeyCode key)
        {
            if (!IsValidModifier(key))
                return false;

            int index = ArrayUtil.FindFirstIndex(m_modifiers, key);
            m_pressed[index] = false;

            return true;
        }

        public bool Check(bool[] checklist)
        {
            return ArrayUtil.CompareSequence(checklist, m_pressed);
        }

        public bool IsValidModifier(KeyCode key)
        {
            return ArrayUtil.Contains(m_modifiers, key);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < m_modifiers.Length; i++)
            {
                sb.Append(m_modifiers[i].ToString() + " : \t" + m_referenceCount[i].ToString());
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}
