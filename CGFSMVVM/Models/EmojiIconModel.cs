using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CGFSMVVM.Models
{
    public class EmojiIconModel
    {
        public string Id { get;}
        public Image EmojiIcon { get; }
        public Label EmojiDescLabel { get;}

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.Models.EmojiIconModel"/> class.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="emojiIcon">Emoji icon.</param>
        /// <param name="emojiDescLabel">Emoji desc label.</param>
        public EmojiIconModel(string id,Image emojiIcon,Label emojiDescLabel)
        {
            this.Id = id;
            this.EmojiIcon = emojiIcon;
            this.EmojiDescLabel = emojiDescLabel;

        }

    }
}
