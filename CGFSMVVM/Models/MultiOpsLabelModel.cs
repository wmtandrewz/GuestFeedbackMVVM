using System;
using Xamarin.Forms;

namespace CGFSMVVM.Models
{
    public class MultiOpsLabelModel
    {
        public string ID { get; }

        public Label OptionLabel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.Models.MultiOpsLabelModel"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="optionLabel">Option label.</param>
        public MultiOpsLabelModel(string id,Label optionLabel)
        {
            this.ID = id;
            this.OptionLabel = optionLabel;
        }
    }
}
