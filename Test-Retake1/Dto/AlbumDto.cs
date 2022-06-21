using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_Retake1.Dto
{
    public class AlbumDto
    {
        public string AlbumName { get; set; }
        public DateTime PublishDate { get; set; }
        public string MusicLabel { get; set; }

        public List<TrackDto> Tracks { get; set; }
    }
}
