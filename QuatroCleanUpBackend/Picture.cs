using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuatroCleanUpBackend
{
    public class Picture
    {
        public  int PictureId { get; set; }

        public int EventId { get; set; }

        public byte[]? PictureData { get; set; } 

        private string _description { get; set; }
        public string Description
        {
            get => _description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(nameof(value), "Description cannot be more than 500 characters.");
                }
                _description = value;
            }
            
        }

        public Picture()
        {
            // Default constructor
        }


        public string ToString()
        {
            return $"PictureId: {PictureId}, EventId: {EventId}, Description: {Description}";
        }
    }
}
