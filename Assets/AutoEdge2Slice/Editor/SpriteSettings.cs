using System;
using System.Linq;
using PlasticGui.Gluon.WorkspaceWindow.Views.IncomingChanges;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AutoEdge2Slice.Editor
{
    [FilePath("ProjectSettings/SpriteSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class SpriteSettings : ScriptableSingleton<SpriteSettings>
    {
        [SerializeField]
        private TextureImporterCompression _textureImporterCompression = TextureImporterCompression.Uncompressed;
        [SerializeField]
        private FilterMode _filterMode = FilterMode.Point;
        [SerializeField]
        private float _pixelPerUnit = 64;
        [SerializeField]
        private int _maxTextureSize = 16384;

        [SerializeField]
        private string _outlineGeneratorFactoryType;
        private Object _outlineGeneratorFactory;

        public TextureImporterCompression TextureImporterCompression => _textureImporterCompression;
        public FilterMode FilterMode => _filterMode;
        public float PixelPerUnit => _pixelPerUnit;
        public int MaxTextureSize => _maxTextureSize;
        public IOutlineGeneratorFactory OutlineGeneratorFactory
        {
            get
            {
                if (_outlineGeneratorFactory == null)
                {
                    var methodCollection = TypeCache.GetMethodsWithAttribute(typeof(OutlineGeneratorFactoryMethodAttribute));
                    foreach (var method in methodCollection)
                    {
                        var obj = method.Invoke(null, null) as Object;
                        if (obj.GetType().Name == _outlineGeneratorFactoryType)
                        {
                            _outlineGeneratorFactory = obj;
                            break;
                        }
                    }
                }
                return _outlineGeneratorFactory as IOutlineGeneratorFactory;
            }
        }
        private void OnEnable()
        {
            hideFlags &= ~HideFlags.NotEditable;
        }

        public void Save()
        {
            Save(true);
        }
    }
}