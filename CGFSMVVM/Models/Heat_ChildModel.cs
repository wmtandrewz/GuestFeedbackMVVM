using System;
using Xamarin.Forms;

namespace CGFSMVVM.Models
{
    public class Heat_ChildModel
    {
        public string ItemID { get; }
        public string ButtonID { get; }
        public HeatButtonModel ButtonModel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.Models.Heat_ChildModel"/> class.
        /// </summary>
        /// <param name="itemid">Identifier.</param>
        /// <param name="buttonModel">Button.</param>
        /// 
        public Heat_ChildModel(string itemid,string buttonid, HeatButtonModel buttonModel)
        {
            this.ItemID = itemid;
            this.ButtonID = buttonid;
            this.ButtonModel = buttonModel;
        }
    }
}
