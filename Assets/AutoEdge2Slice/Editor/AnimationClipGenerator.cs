using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace AutoEdge2Slice.Editor
{
    public class AnimationClipGenerator
    {
        /// <summary>
        /// AnimationClipを作成して返す。
        /// ただしアセット化は行わない。
        /// </summary>
        /// <returns></returns>
        public AnimationClip CreateAnimationClip(Sprite[] sprites,XDocument document)
        {
            var clip = new AnimationClip();
            var settings = new AnimationClipSettings();
            AnimationUtility.SetAnimationClipSettings(clip, settings);

            
            
            var objectReferenceKeyframes = GetAnimationData(document, sprites).ToArray();
            
            var editorCurveBinding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");
            AnimationUtility.SetObjectReferenceCurve(clip, editorCurveBinding, objectReferenceKeyframes);
            return clip;
        }

        private static IEnumerable<ObjectReferenceKeyframe> GetAnimationData(XDocument document,IEnumerable<Sprite> targetSprites)
        {
            var root = document.Root;
            var pages = root?.Elements("Page");
            if (pages == null) throw new NullReferenceException("XMLファイルにPageが存在しません");
            
            var times = pages.Select(x =>
            {
                var delayString = x.Element("Delay")?.Value;
                return delayString == null ? 0 : float.Parse(delayString);
            }).PreScan(0f, (s, r) => s + r);
            
            return targetSprites.Zip(times, (s, t) => new ObjectReferenceKeyframe() { time = t, value = s });
        }

        public AnimationClipSettings GetAnimationClipSettings()
        {
            return new AnimationClipSettings()
            {
                stopTime = 10,
                startTime = 0,
            };
        }
    }
}