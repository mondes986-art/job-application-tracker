using Microsoft.EntityFrameworkCore;
using JobApplicationTracker.Models;

namespace JobApplicationTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<JobApplication> JobApplications { get; set; }
    }
}