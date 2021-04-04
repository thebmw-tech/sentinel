﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sentinel.Core;

namespace Sentinel.Core.Migrations
{
    [DbContext(typeof(SentinelDatabaseContext))]
    [Migration("20210404034357_Initial_Interface_Migrations")]
    partial class Initial_Interface_Migrations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("Sentinel.Core.Entities.Gateway", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GatewayType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<int>("IPVersion")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InterfaceName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("RevisionId", "Id");

                    b.HasIndex("InterfaceName");

                    b.ToTable("Gateways");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.Interface", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IPv4Address")
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.Property<int>("IPv4ConfigurationType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<Guid?>("IPv4GatewayId")
                        .HasColumnType("TEXT");

                    b.Property<byte?>("IPv4SubnetMask")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IPv6Address")
                        .HasMaxLength(39)
                        .HasColumnType("TEXT");

                    b.Property<int>("IPv6ConfigurationType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<Guid?>("IPv6GatewayId")
                        .HasColumnType("TEXT");

                    b.Property<byte?>("IPv6SubnetMask")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InterfaceType")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("SpoofedMAC")
                        .HasColumnType("INTEGER");

                    b.HasKey("RevisionId", "Name");

                    b.ToTable("Interfaces");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.Revision", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CommitDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ConfirmDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Revisions");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.Route", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<byte>("SubnetMask")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("GatewayId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("RevisionId", "Address", "SubnetMask");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.SystemConfiguration", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<string>("Hostname")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("TEXT");

                    b.HasKey("RevisionId");

                    b.ToTable("SystemConfigurations");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
