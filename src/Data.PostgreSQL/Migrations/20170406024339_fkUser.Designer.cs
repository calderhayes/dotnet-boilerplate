using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DotNetBoilerplate.Data;

namespace Data.PostgreSQL.Migrations
{
    [DbContext(typeof(PostgreSQLContext))]
    [Migration("20170406024339_fkUser")]
    partial class fkUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.AuditTicket", b =>
                {
                    b.Property<long>("TicketId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("EndTime");

                    b.Property<string>("IpAddress");

                    b.Property<string>("SecurityTokenId");

                    b.Property<string>("SecurityTokenIssuer");

                    b.Property<DateTimeOffset>("StartTime");

                    b.Property<long?>("UserId");

                    b.HasKey("TicketId");

                    b.ToTable("AuditTickets");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.LogEvent", b =>
                {
                    b.Property<int>("LogEventId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AuditTicketId");

                    b.HasKey("LogEventId");

                    b.HasIndex("AuditTicketId");

                    b.ToTable("LogEvents");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.UserAccount", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreatedTicketId");

                    b.Property<long>("ModifiedTicketId");

                    b.Property<string>("UserName");

                    b.HasKey("UserId");

                    b.HasIndex("CreatedTicketId");

                    b.HasIndex("ModifiedTicketId");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.UserAuthenticationSource", b =>
                {
                    b.Property<string>("AuthenticationSource");

                    b.Property<string>("Subject");

                    b.Property<long>("UserId");

                    b.HasKey("AuthenticationSource", "Subject", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAuthenticationSource");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.LogEvent", b =>
                {
                    b.HasOne("DotNetBoilerplate.Data.Entity.AuditTicket", "AuditTicket")
                        .WithMany()
                        .HasForeignKey("AuditTicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.UserAccount", b =>
                {
                    b.HasOne("DotNetBoilerplate.Data.Entity.AuditTicket", "CreatedAuditTicket")
                        .WithMany()
                        .HasForeignKey("CreatedTicketId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DotNetBoilerplate.Data.Entity.AuditTicket", "ModifiedAuditTicket")
                        .WithMany()
                        .HasForeignKey("ModifiedTicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.UserAuthenticationSource", b =>
                {
                    b.HasOne("DotNetBoilerplate.Data.Entity.UserAccount", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
