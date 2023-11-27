﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ShareLink.Dal;
using ShareLink.Domain.Models;

#nullable disable

namespace ShareLink.Migrations.Migrations.ApplicationMigrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231127000426_AddedCaseInsansitiveCollation")]
    partial class AddedCaseInsansitiveCollation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LinkTag", b =>
                {
                    b.Property<string>("LinksId")
                        .HasColumnType("text");

                    b.Property<string>("TagsName")
                        .HasColumnType("text");

                    b.HasKey("LinksId", "TagsName");

                    b.HasIndex("TagsName");

                    b.ToTable("LinkTag");
                });

            modelBuilder.Entity("LinkUserProfile", b =>
                {
                    b.Property<string>("LikedByUserId")
                        .HasColumnType("text");

                    b.Property<string>("LikedLinksId")
                        .HasColumnType("text");

                    b.HasKey("LikedByUserId", "LikedLinksId");

                    b.HasIndex("LikedLinksId");

                    b.ToTable("UserLikedLinks", (string)null);
                });

            modelBuilder.Entity("LinkUserProfile1", b =>
                {
                    b.Property<string>("SavedByUserId")
                        .HasColumnType("text");

                    b.Property<string>("SavedLinksId")
                        .HasColumnType("text");

                    b.HasKey("SavedByUserId", "SavedLinksId");

                    b.HasIndex("SavedLinksId");

                    b.ToTable("UserSavedLinks", (string)null);
                });

            modelBuilder.Entity("ShareLink.Domain.Models.Link", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .UseCollation("secondary_strength");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserNickname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<YoutubeData>("Youtube")
                        .HasColumnType("jsonb");

                    b.HasKey("Id");

                    b.HasIndex("CreatedAt");

                    b.HasIndex("Title");

                    b.HasIndex("Type");

                    b.HasIndex("UserId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("ShareLink.Domain.Models.Tag", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("ShareLink.Domain.Models.UserProfile", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("UserProfiles");
                });

            modelBuilder.Entity("LinkTag", b =>
                {
                    b.HasOne("ShareLink.Domain.Models.Link", null)
                        .WithMany()
                        .HasForeignKey("LinksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShareLink.Domain.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LinkUserProfile", b =>
                {
                    b.HasOne("ShareLink.Domain.Models.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("LikedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShareLink.Domain.Models.Link", null)
                        .WithMany()
                        .HasForeignKey("LikedLinksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LinkUserProfile1", b =>
                {
                    b.HasOne("ShareLink.Domain.Models.UserProfile", null)
                        .WithMany()
                        .HasForeignKey("SavedByUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShareLink.Domain.Models.Link", null)
                        .WithMany()
                        .HasForeignKey("SavedLinksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
