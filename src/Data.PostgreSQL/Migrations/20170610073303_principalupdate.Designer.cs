using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DotNetBoilerplate.Data;
using DotNetBoilerplate.Data.Model.Lookup;

namespace Data.PostgreSQL.Migrations
{
    [DbContext(typeof(PostgreSQLContext))]
    [Migration("20170610073303_principalupdate")]
    partial class principalupdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.AuditTicket", b =>
                {
                    b.Property<long>("TicketId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayUrl");

                    b.Property<DateTimeOffset?>("EndTime");

                    b.Property<string>("IpAddress");

                    b.Property<string>("RequestId");

                    b.Property<string>("RequestMethod");

                    b.Property<string>("SecurityTokenId");

                    b.Property<string>("SecurityTokenIssuer");

                    b.Property<DateTimeOffset>("StartTime");

                    b.Property<string>("UserAgent");

                    b.Property<long?>("UserId");

                    b.HasKey("TicketId");

                    b.ToTable("AuditTickets");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.HubLog", b =>
                {
                    b.Property<long>("HubLogId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Arguments");

                    b.Property<long?>("AuditTicketId");

                    b.Property<string>("HubName");

                    b.Property<bool>("IsIncoming");

                    b.Property<string>("MethodName");

                    b.HasKey("HubLogId");

                    b.HasIndex("AuditTicketId");

                    b.ToTable("HubLogs");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.Principal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreatedTicketId");

                    b.Property<Guid>("ExternalId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Label")
                        .IsRequired();

                    b.Property<long>("ModifiedTicketId");

                    b.Property<int>("PrincipalType");

                    b.HasKey("Id");

                    b.HasIndex("CreatedTicketId");

                    b.HasIndex("ExternalId")
                        .IsUnique();

                    b.HasIndex("ModifiedTicketId");

                    b.ToTable("Principals");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.PrincipalClosureMap", b =>
                {
                    b.Property<long>("AncestorId");

                    b.Property<long>("DescendantId");

                    b.Property<long>("CreatedTicketId");

                    b.Property<int>("Domain");

                    b.Property<int>("PathLength");

                    b.HasKey("AncestorId", "DescendantId");

                    b.HasIndex("CreatedTicketId");

                    b.HasIndex("DescendantId");

                    b.ToTable("PrincipalClosureMaps");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.RequestLog", b =>
                {
                    b.Property<long>("AuditTicketId");

                    b.Property<string>("RequestBody");

                    b.HasKey("AuditTicketId");

                    b.ToTable("RequestLogs");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.ResponseLog", b =>
                {
                    b.Property<long>("AuditTicketId");

                    b.Property<string>("ResponseBody");

                    b.HasKey("AuditTicketId");

                    b.ToTable("ResponseLogs");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.UserAccount", b =>
                {
                    b.Property<long>("Id");

                    b.Property<long>("CreatedTicketId");

                    b.Property<string>("Culture")
                        .IsRequired();

                    b.Property<long>("ModifiedTicketId");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

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

                    b.Property<long>("CreatedTicketId");

                    b.Property<long>("ModifiedTicketId");

                    b.HasKey("AuthenticationSource", "Subject", "UserId");

                    b.HasIndex("CreatedTicketId");

                    b.HasIndex("ModifiedTicketId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAuthenticationSources");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.HubLog", b =>
                {
                    b.HasOne("DotNetBoilerplate.Data.Entity.AuditTicket", "AuditTicket")
                        .WithMany()
                        .HasForeignKey("AuditTicketId");
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.Principal", b =>
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

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.PrincipalClosureMap", b =>
                {
                    b.HasOne("DotNetBoilerplate.Data.Entity.Principal", "AncestorPrincipal")
                        .WithMany()
                        .HasForeignKey("AncestorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DotNetBoilerplate.Data.Entity.AuditTicket", "CreatedAuditTicket")
                        .WithMany()
                        .HasForeignKey("CreatedTicketId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DotNetBoilerplate.Data.Entity.Principal", "DescendantPrincipal")
                        .WithMany()
                        .HasForeignKey("DescendantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.RequestLog", b =>
                {
                    b.HasOne("DotNetBoilerplate.Data.Entity.AuditTicket", "AuditTicket")
                        .WithMany()
                        .HasForeignKey("AuditTicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.ResponseLog", b =>
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

                    b.HasOne("DotNetBoilerplate.Data.Entity.Principal", "Principal")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DotNetBoilerplate.Data.Entity.AuditTicket", "ModifiedAuditTicket")
                        .WithMany()
                        .HasForeignKey("ModifiedTicketId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DotNetBoilerplate.Data.Entity.UserAuthenticationSource", b =>
                {
                    b.HasOne("DotNetBoilerplate.Data.Entity.AuditTicket", "CreatedAuditTicket")
                        .WithMany()
                        .HasForeignKey("CreatedTicketId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DotNetBoilerplate.Data.Entity.AuditTicket", "ModifiedAuditTicket")
                        .WithMany()
                        .HasForeignKey("ModifiedTicketId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DotNetBoilerplate.Data.Entity.UserAccount", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
