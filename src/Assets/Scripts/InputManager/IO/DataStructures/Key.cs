using UnityEngine;

namespace AnotherRTS.Management.RemappableInput.IO
{

    public struct KeyData
    {
        public readonly string name;
        public readonly KeyCode[] keycodes;
        public readonly KeyCode[] modifiers;

        public KeyData(string name, KeyCode[] keycodes, KeyCode[] modifiers)
        {
            this.name = name;
            this.keycodes = keycodes;
            this.modifiers = modifiers;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("Key: " + name);
            sb.Append("\n\tKeycode :");

            if (keycodes != null)
            {
                for (int i = 0; i < keycodes.Length; i++)
                {
                    sb.Append(" " + keycodes[i]);
                }
            }

            sb.Append("\n\tModifiers :");
            if (modifiers != null)
            {
                for (int i = 0; i < modifiers.Length; i++)
                {
                    sb.Append(" " + modifiers[i]);
                }
            }

            return sb.ToString();
        }
    }
}
