using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Models;
namespace WebApplication1.Data
{
    public class CAppDbContext: DbContext
    {
        public CAppDbContext(DbContextOptions<CAppDbContext> options) : base(options) { }

        public DbSet<CBox> CBoxes { get; set; }
        public DbSet<CPack> CPacks { get; set; }
    }
}
