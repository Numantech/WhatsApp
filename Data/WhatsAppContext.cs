using Microsoft.EntityFrameworkCore;
using WhatsAppAPI.Models;

namespace WhatsAppAPI.Data
{
    public class WhatsAppContext : DbContext
    {
        public WhatsAppContext(DbContextOptions<WhatsAppContext> options) : base(options) { }

        public DbSet<Response> Responses { get; set; }
    }

  
}
