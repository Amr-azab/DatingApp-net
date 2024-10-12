using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser , AppRole , int,
        IdentityUserClaim<int>,AppUserRole, IdentityUserLogin<int>,IdentityRoleClaim<int>,IdentityUserToken<int>>(options)
{
  // public DbSet<AppUser> Users { get; set; }
  public DbSet<UserLike> Likes { get; set; }
  public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u=>u.User)
            .HasForeignKey(Ur=>Ur.UserId)
            .IsRequired();
        builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u=>u.Role)
            .HasForeignKey(Ur=>Ur.RoleId)
            .IsRequired();    

        builder.Entity<UserLike>()
          .HasKey(k=>new { k.SourceUserId, k.TargetUserId });
        builder.Entity<UserLike>()
          .HasOne(s=>s.SourceUser)
          .WithMany(l=>l.LikedUsers)
          .HasForeignKey(s=>s.SourceUserId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserLike>()
          .HasOne(s=>s.TargetUser)
          .WithMany(l=>l.LikedByUsers)
          .HasForeignKey(s=>s.TargetUserId)
          .OnDelete(DeleteBehavior.Cascade); 

        builder.Entity<Message>()
          .HasOne(x=>x.Recipient)
          .WithMany(x=>x.MessageReceived)
          .OnDelete(DeleteBehavior.Restrict); 

        builder.Entity<Message>()
          .HasOne(x=>x.Sender)
          .WithMany(x=>x.MessageSent)
          .OnDelete(DeleteBehavior.Restrict);            
    }
}
