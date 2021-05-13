namespace GraphicTool
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("gallery")]
    public partial class gallery
    {
        [Key]
        public int painting_id { get; set; }

        [StringLength(10)]
        public string username { get; set; }

        [Required]
        [StringLength(1)]
        public string painting { get; set; }

        public DateTime? date_created { get; set; }

        public virtual user user { get; set; }
    }
}
