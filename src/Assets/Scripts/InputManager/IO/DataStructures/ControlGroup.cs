namespace AnotherRTS.Management.RemappableInput.IO
{
    public struct KeyGroupData
    {
        public readonly string name;
        public readonly KeyData[] keys;

        public KeyGroupData(string name, KeyData[] keys)
        {
            this.name = name;
            this.keys = keys;
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(name);
            sb.Append('\n');
            for (int i = 0; i < keys.Length; i++)
            {
                sb.Append(keys[i].ToString());
                sb.Append('\n');
            }
            return sb.ToString();
        }
    }
}
