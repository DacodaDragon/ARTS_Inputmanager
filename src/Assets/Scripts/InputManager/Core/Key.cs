using UnityEngine;
using AnotherRTS.Util;

namespace AnotherRTS.Management.RemappableInput
{
    public delegate void VoidDelegate();
    public class Key
    {
        private int m_id;
        private int m_framePress = 0;
        private int m_frameRelease = 0;
        private bool m_held = false;
        private KeyCode[] m_keys;
        private KeyCode[] m_modifiers;
        private bool[] m_keysPressed;
        private bool[] m_modifierChecklist;
        private string m_name;

        private ModifierKeyRegister m_modifierRegister;

        private event VoidDelegate m_KeyUpEvent;
        private event VoidDelegate m_KeyDownEvent;

        // Propperties
        public int ID { get { return m_id; } }

        public string Name { get { return m_name; } }

        public bool IsPressed  { get { return (m_framePress == Time.frameCount); } }
        public bool IsReleased { get { return (m_frameRelease == Time.frameCount); } }
        public bool IsHeld     { get { return m_held; } }

        public event VoidDelegate OnKeyUp
        {
            add    { m_KeyUpEvent += value; }
            remove { m_KeyUpEvent -= value; }
        }

        public event VoidDelegate OnKeyDown
        {
            add    { m_KeyDownEvent += value; }
            remove { m_KeyDownEvent -= value; }
        }

        public KeyCode[] Keys {
            get { return m_keys; }
            set
            {
                m_keys = value; m_keysPressed = new bool[m_keys.Length];
                Reset();
            }
        }

        public KeyCode[] Modifiers{
            get { return m_modifiers; }
            set
            {
                m_modifierRegister.Remove(m_modifiers);
                m_modifiers = value;
                m_modifierRegister.Add(m_modifiers);
                m_modifierChecklist = m_modifierRegister.GetModifierCheckArray(m_modifiers);
                Reset();
            }
        }

        private void Reset()
        {
            m_held = false;
            m_framePress = 0;
            m_frameRelease = 0;
            ArrayUtil.Fill(m_keysPressed, false);
        }

        public Key(int id, string name, KeyCode[] keys, KeyCode[] modifiers, ModifierKeyRegister register)
        { 
            m_id = id;
            m_name = name;
            m_keys = keys;
            m_modifiers = modifiers;
            m_modifierRegister = register;

            m_keysPressed = new bool[keys.Length];

            // Init all keys to not be pressed
            ArrayUtil.Fill(m_keysPressed, false);
        }

        public void Start()
        {
            m_modifierChecklist = m_modifierRegister
                .GetModifierCheckArray(m_modifiers);
        }

        private void UpdateKey()
        {
            if (CheckValid())
                Activate();
            else Deactivate();
        }

        private bool CheckValid()
        {
            return ArrayUtil.Contains(m_keysPressed, true) &&
                m_modifierRegister.Check(m_modifierChecklist);
        }

        private void Activate()
        {
            if (m_held)
                return;

            m_KeyDownEvent?.Invoke();
            m_framePress = Time.frameCount;
            m_held = true;
        }

        private void Deactivate()
        {
            if (!m_held)
                return;

            m_KeyUpEvent?.Invoke();
            m_frameRelease = Time.frameCount;
            m_held = false;
        }

        public void SilentKill()
        {
            m_held = false;
        }

        // [TODO] Fix Input to update when keys are released. 
        public void KeyUp(KeyCode KeyCode)
        {
            if (m_modifierRegister.IsValidModifier(KeyCode))
            {
                UpdateKey();
                return;
            }

            for (int i = 0; i < Keys.Length; i++)
            {
                if (Keys[i] == KeyCode)
                {
                    m_keysPressed[i] = true;
                    UpdateKey();
                    return;
                }
            }
        }

        public void KeyDown(KeyCode KeyCode)
        {
            if (m_modifierRegister.IsValidModifier(KeyCode))
            {
                UpdateKey();
                return;
            }

            for (int i = 0; i < Keys.Length; i++)
            {
                if (Keys[i] == KeyCode)
                {
                    m_keysPressed[i] = false;
                    UpdateKey();
                    return;
                }
            }
        }
    }
}
