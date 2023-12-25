using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using SmartXGPT.Service.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartXGPT.Infrastructure.Contexts.SmartXDbContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public virtual DbSet<MesssageGPT> MessageGPTs { get; set; }
        public virtual DbSet<SurveyResult> SurveyResults { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
