using System;
using Xamarin.Forms;

namespace CGFSMVVM.Models
{
    public class HeatButtonModel
    {
        public string ID { get; }
        public Button button { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.Models.HeatButtonModel"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="button">Button.</param>
        public HeatButtonModel(string id,Button button)
        {
            this.ID = id;
            this.button = button;
        }
    }
}
