#nullable enable
using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NovemberProject.ClicheSpeech
{
    public sealed class ClicheBible
    {
        private const string CLICHE_BIBLE_FILE = "cliche_bible";

        private readonly string[] _cliches;

        public ClicheBible()
        {
            var asset = Resources.Load<TextAsset>(CLICHE_BIBLE_FILE);
            if (asset == null)
            {
                throw new FileLoadException($"Could not load resource file {CLICHE_BIBLE_FILE}");
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