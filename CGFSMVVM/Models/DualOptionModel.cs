using System;
using Xamarin.Forms;

namespace CGFSMVVM.Models
{
    public class DualOptionModel
    {

        public string ID;
        public Image image;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.Models.DualOptionModel"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="image">Image.</param>
        public DualOptionModel(string id,Image image)
        {
            this.ID = id;
            this.image = image;
        }
    }
}
