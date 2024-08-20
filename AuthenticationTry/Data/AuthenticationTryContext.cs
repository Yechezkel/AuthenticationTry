using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthenticationTry.Models;

namespace AuthenticationTry.Data
{
    public class AuthenticationTryContext : DbContext
    {
        public AuthenticationTryContext (DbContextOptions<AuthenticationTryContext> options)
            : base(options)
        {
        }

        public DbSet<AuthenticationTry.Models.User> User { get; set; } = default!;
    }
}
