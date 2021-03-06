﻿using DataAccess.Repositories;
using Kommunikationsverktyg_för_informatik.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DataAccess.Models
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DataInitializer());
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Kategori> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<TimeSuggestion> TimeSuggestions { get; set; }
        public DbSet<TimeAnswer> TimeAnswers { get; set; }
        public DbSet<RecieveMeetingInvitation> RMInvites { get; set; }
        public DbSet<ResearchPost> ResearchPosts { get; set; }
        public DbSet<EducationPost> EducationPosts { get; set; }
        public DbSet<Location> Location { get; set; }
    }
}
