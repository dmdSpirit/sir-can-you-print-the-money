#nullable enable
using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NovemberProject.ClicheSpeech
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

            _cliches = asset.text.Split(Environment.NewLine);
        }

        public string GetCliche()
        {
            int rnd = Random.Range(0, _cliches.Length);
            return _cliches[rnd];
        }
    }
}