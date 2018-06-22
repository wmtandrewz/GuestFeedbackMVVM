using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CGFSMVVM.Models
{
    public class ChildHeatListModel
    {
        public string ID { get; }
        public Label titleLabel;
        public Label childTitleLabel;
        public List<Button> buttonList { get; }


        public ChildHeatListModel(string id, Label label, Label childLabel, List<Button> button)
        {
            this.ID = id;
            this.titleLabel = label;
            this.childTitleLabel = childLabel;
            this.buttonList = button;
        }
    }
}
