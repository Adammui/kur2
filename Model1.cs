using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace GraphicTool
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=userContext")
        {
        }

        public virtual DbSet<gallery> galleries { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
