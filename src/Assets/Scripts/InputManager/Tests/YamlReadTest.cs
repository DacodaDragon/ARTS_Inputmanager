using UnityEngine;


namespace AnotherRTS.Management.RemappableInput.IO
{
    public class YamlReadTest : MonoBehaviour
    {
        [SerializeField]
        TextAsset YamlDocument;
        public void Start()
        {
            YamlKeyBindingReader s = new YamlKeyBindingReader();
            s.FromString(YamlDocument.text);
        }
    }
}
