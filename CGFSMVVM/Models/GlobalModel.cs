using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CGFSMVVM.Models
{
    public static class GlobalModel
    {
        public static List<Image> EmojiIconList
        {
            get;
            set;

        }

        public static List<Label> EmojiDescLabelList
        {
            get;
            set;
        }

        public static List<Label> MultiOptionsLabelList
        {
            get;
            set;
        }
        public static List<Label> MultiSelectionsLabelList
        {
            get;
            set;
        }

        public static List<Button> HeatButonList
        {
            get;
            set;
        }

        public static List<Image> DualOptionList
        {
            get;
            set;
        }

        public static Editor CommentEditor
        {
            get;
            set;
        }

        public static List<ChildHeatListModel> ChildHeatListCollection
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the <see cref="T:CGFSMVVM.Models.GlobalModel"/> class.
        /// </summary>
        static GlobalModel()
        {
            EmojiIconList = new List<Image>();
            EmojiDescLabelList = new List<Label>();
            MultiOptionsLabelList = new List<Label>();
            MultiSelectionsLabelList = new List<Label>();
            HeatButonList = new List<Button>();
            DualOptionList = new List<Image>();
            CommentEditor = new Editor();
            ChildHeatListCollection = new List<ChildHeatListModel>();
            TimeSpan = 0.5;
        }

        /// <summary>
        /// Cleans the global model.
        /// </summary>
        public static void CleanGlobalModel()
        {
            EmojiIconList.Clear();
            EmojiDescLabelList.Clear();
            MultiOptionsLabelList.Clear();
            MultiSelectionsLabelList.Clear();
            HeatButonList.Clear();
            DualOptionList.Clear();
            ChildHeatListCollection.Clear();
            CommentEditor = new Editor();
        }

        /// <summary>
        /// Gets the time span.
        /// </summary>
        /// <value>The time span.</value>
        public static double TimeSpan
        {
            get;
        }

        /// <summary>
        /// The color list seconary.
        /// </summary>
        public static List<Color> ColorListSeconary = new List<Color>
        {
            Color.FromRgb(119, 229, 0),
            Color.FromRgb(140, 160, 12),
            Color.FromRgb(175, 130, 20),
            Color.FromRgb(210, 100, 30),
            Color.FromRgb(240, 60, 40)
        };

        /// <summary>
        /// The color list.
        /// </summary>
        public static List<Color> ColorList = new List<Color>
        {
            Color.FromRgb(119, 229, 0),
            Color.FromRgb(126, 206, 4),
            Color.FromRgb(133, 183, 8),
            Color.FromRgb(140, 160, 12),
            Color.FromRgb(157, 147, 16),
            Color.FromRgb(175, 130, 20),
            Color.FromRgb(190, 117, 25),
            Color.FromRgb(210, 100, 30),
            Color.FromRgb(220, 85, 35),
            Color.FromRgb(240, 60, 40)
        };
    }
}
