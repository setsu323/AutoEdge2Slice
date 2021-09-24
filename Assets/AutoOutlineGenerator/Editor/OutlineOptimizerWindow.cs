using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace AutoOutlineGenerator.Editor
{
    public class OutlineOptimizerWindow : EditorWindow
    {
        [MenuItem("Tools/OutlineOptimizerWindow")]
        public static void ShowWindow()
        {
            GetWindow<OutlineOptimizerWindow>();
        }

        private void OnEnable()
        {
            var outlineToleranceSlider = new Slider() {lowValue = 0, highValue = 1};
            var generateOutlineButton = new Button();
        }

        /// <summary>
        /// outlineを生成するためのボタン
        /// </summary>
        private void OnGenerateOutline()
        {
            //問題はテクスチャのプレビューが出来ない点か……
        }
    }
}