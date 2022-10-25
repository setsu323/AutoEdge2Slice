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
            return ModifyAnimationClip(clip, sprites, document);
        }

        public AnimationClip ModifyAnimationClip(AnimationClip clip, Sprite[] sprites, XDocument document)
        {
            var objectReferenceKeyframes = GetAnimationData(document, sprites).ToArray();
            
            var editorCurveBinding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");
            AnimationUtility.SetObjectReferenceCurve(clip, editorCurveBinding, objectReferenceKeyframes);
            return clip;
        }

        private static IEnumerable<ObjectReferenceKeyframe> GetAnimationData(XDocument document,IEnumerable<Sprite> targetSprites)
        {
            var enumerable = targetSprites as Sprite[] ?? targetSprites.ToArray();
            var root = document.Root;
            var pages = root?.Elements("Page");
            if (pages == null) throw new NullReferenceException("XMLファイルにPageが存在しません");
            var unit = DelayUnitSettingsConverter.ToUnit(document);
            
            
            var delayUnit = root?.Element("DelayUnit")?.Value;

            var times = pages.Select(x =>
            {
                var delayString = x.Element("Delay")?.Value;
                return delayString == null ? 0f : float.Parse(delayString)/unit;
            }).Append(0).PreScan(0f, (s, r) => s + r);
            
            return enumerable.Append(enumerable.Last()).Zip(times, (s, t) => new ObjectReferenceKeyframe() { time = t, value = s });
        }
    }
}