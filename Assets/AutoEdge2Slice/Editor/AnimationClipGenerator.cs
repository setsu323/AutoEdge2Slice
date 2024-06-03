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
        public AnimationClip CreateAnimationClip(Sprite[] sprites,XDocument document,bool loopTime)
        {
            var clip = new AnimationClip();
            return ModifyAnimationClip(clip, sprites, document,loopTime);
        }

        public AnimationClip ModifyAnimationClip(AnimationClip clip, Sprite[] sprites, XDocument document,bool containsLoopName)
        {
            var objectReferenceKeyframes = GetAnimationData(document, sprites).ToArray();
            
            //最後のフレームの時間のまま登録すると、アニメーション全体の長さが
            //Edge上での長さ + 1フレーム
            //になるので、先に1フレーム分だけ値を引いておく。
            var frameTime = 1 / clip.frameRate;
            objectReferenceKeyframes[objectReferenceKeyframes.Length - 1].time -= frameTime;

            var editorCurveBinding = EditorCurveBinding.PPtrCurve("", typeof(SpriteRenderer), "m_Sprite");
            AnimationUtility.SetObjectReferenceCurve(clip, editorCurveBinding, objectReferenceKeyframes);
            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            if (containsLoopName)
            {
                settings.loopTime = true;
            }
            AnimationUtility.SetAnimationClipSettings(clip, settings);
            return clip;
        }

        private static IEnumerable<ObjectReferenceKeyframe> GetAnimationData(XDocument document,IEnumerable<Sprite> targetSprites)
        {
            var enumerable = targetSprites as Sprite[] ?? targetSprites.ToArray();
            var root = document.Root;
            var pages = root?.Elements("Page");
            if (pages == null) throw new NullReferenceException("XMLファイルにPageが存在しません");
            var unit = DelayUnitSettingsConverter.ToUnit(document);

            var times = pages.Select(x =>
            {
                var delayString = x.Element("Delay")?.Value;
                return delayString == null ? 0f : float.Parse(delayString)/unit;
            }).Append(0).PreScan(0f, (s, r) => s + r);
            
            //ObjectReferenceKeyframeの最後が1フレーム多くなってしまう……
            return enumerable.Append(enumerable[times.Count()-2]).Zip(times, (s, t) => new ObjectReferenceKeyframe() { time = t, value = s });
        }
    }
}