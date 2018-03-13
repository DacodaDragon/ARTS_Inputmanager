using System.IO;
using System.Collections.Generic;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace AnotherRTS.Management.RemappableInput.IO.Yaml
{
    public class YamlDeserializedReader
    {
        List<Document> documents;
        public void FromString(string content)
        {
            StringReader input = new StringReader(content);
            Deserializer deserializer = new DeserializerBuilder().Build();
            Parser parser = new Parser(input);

            parser.Expect<StreamStart>();

            while (parser.Accept<DocumentStart>())
            {
                documents.Add(deserializer.Deserialize<Document>(parser));
            }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int i = 0; i < documents.Count; i++)
            {
                sb.Append(documents[i].ToString()).Append('\n');
            }

            return base.ToString();
        }
    }

    public struct Document
    {
        public string name { get; set; }
        public List<Key> keys { get; set; }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append('\t').Append(name).Append('\n');

            for (int i = 0; i < keys.Count; i++)
            {
                sb.Append(keys[i].ToString()).Append('\n');
            }

            return sb.ToString();
        }
    }

    public struct Key
    {
        public string name { get; set; }
        public List<string> keycodes { get; set; }
        public List<string> modifiers { get; set; }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append('\t',2).Append(name).Append('\n');

            sb.Append('\t', 2);
            for (int i = 0; i < keycodes.Count; i++)
            {
                sb.Append(keycodes[i]).Append(' ');
            }

            sb.Append('\n').Append('\t', 2);
            for (int i = 0; i < modifiers.Count; i++)
            {
                sb.Append(modifiers[i]);
            }
            return sb.ToString();
        }
    }
}
