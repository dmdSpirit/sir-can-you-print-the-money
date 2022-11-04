#nullable enable
using System.IO;
using UnityEngine;

namespace NovemberProject
{
    public class ClicheBible
    {
        private readonly string[] _cliches;

        public ClicheBible(string clicheBibleFile)
        {
            var asset = Resources.Load<TextAsset>(clicheBibleFile);
            if (asset == null)
            {
                throw new FileLoadException($"Could not load resource file {clicheBibleFile}");
            }

            _cliches = asset.text.Split(System.Environment.NewLine);
        }

        public string GetCliche()
        {
            int rnd = Random.Range(0, _cliches.Length);
            return _cliches[rnd];
        }
    }
}