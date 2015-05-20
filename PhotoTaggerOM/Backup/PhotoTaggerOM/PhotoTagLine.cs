using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhotoTaggerOM
{
    public class PhotoTagLine
    {
        private Guid id;
        private Guid parentPhotoTagID;
        private PhotoTag parentPhoto;


        public Guid ID
        {
            get { return id; }
        }

        public Guid ParentPhotoTagID
        {
            set { parentPhotoTagID = value; }
        }

        public PhotoTag ParentPhotoTag
        {
            get { return parentPhoto; }
            set { parentPhoto = value; }
        }
    }
}
