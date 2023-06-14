﻿namespace Identity_API.Data
{
    public sealed class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {         
        }
    }
}
