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
    [Migration("20210619185819_AddHistoryToSystemConfiguration")]
    partial class AddHistoryToSystemConfiguration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("Sentinel.Core.Entities.DestinationNatRule", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("DestinationPortRangeEnd")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("DestinationPortRangeStart")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("DestinationSubnetMask")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IPVersion")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InboundInterfaceName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("InvertDestinationMatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool>("InvertSourceMatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool>("Log")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Protocol")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SourceAddress")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("SourcePortRangeEnd")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("SourcePortRangeStart")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("SourceSubnetMask")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TranslationAddress")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("TranslationPortRangeEnd")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("TranslationPortRangeStart")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("TranslationSubnetMask")
                        .HasColumnType("INTEGER");

                    b.HasKey("RevisionId", "Id");

                    b.HasIndex("Order")
                        .IsUnique();

                    b.ToTable("DestinationNatRules");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.FirewallRule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Action")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationAddress")
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("DestinationPortRangeEnd")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("DestinationPortRangeStart")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("DestinationSubnetMask")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("FirewallTableId")
                        .HasColumnType("TEXT");

                    b.Property<int>("IPVersion")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InvertDestinationMatch")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InvertSourceMatch")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Log")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Protocol")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SourceAddress")
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("SourcePortRangeEnd")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("SourcePortRangeStart")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("SourceSubnetMask")
                        .HasColumnType("INTEGER");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FirewallTableId", "Order")
                        .IsUnique();

                    b.ToTable("FirewallRules");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.FirewallTable", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("DefaultAction")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("DefaultLog")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(28)
                        .HasColumnType("TEXT");

                    b.HasKey("RevisionId", "Id");

                    b.ToTable("FirewallTables");
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

                    b.Property<Guid?>("InboundFirewallTableId")
                        .HasColumnType("TEXT");

                    b.Property<int>("InterfaceType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("LocalFirewallTableId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OutboundFirewallTableId")
                        .HasColumnType("TEXT");

                    b.Property<long?>("SpoofedMAC")
                        .HasColumnType("INTEGER");

                    b.HasKey("RevisionId", "Name");

                    b.ToTable("Interfaces");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.InterfaceAddress", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InterfaceName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("AddressConfigurationType")
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(0);

                    b.Property<string>("Address")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("SubnetMask")
                        .HasColumnType("INTEGER");

                    b.HasKey("RevisionId", "InterfaceName", "AddressConfigurationType", "Address");

                    b.ToTable("InterfaceAddresses");
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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("DATETIME('now')");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool>("HasChanges")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("Locked")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CommitDate");

                    b.HasIndex("ConfirmDate");

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

                    b.Property<string>("InterfaceName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("NextHopAddress")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<int>("RouteType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("RevisionId", "Address", "SubnetMask");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.SourceNatRule", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("DestinationPortRangeEnd")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("DestinationPortRangeStart")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("DestinationSubnetMask")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IPVersion")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("InvertDestinationMatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool>("InvertSourceMatch")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool>("Log")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OutboundInterfaceName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("Protocol")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SourceAddress")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("SourcePortRangeEnd")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("SourcePortRangeStart")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("SourceSubnetMask")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TranslationAddress")
                        .HasMaxLength(45)
                        .HasColumnType("TEXT");

                    b.Property<ushort?>("TranslationPortRangeEnd")
                        .HasColumnType("INTEGER");

                    b.Property<ushort?>("TranslationPortRangeStart")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("TranslationSubnetMask")
                        .HasColumnType("INTEGER");

                    b.HasKey("RevisionId", "Id");

                    b.HasIndex("Order")
                        .IsUnique();

                    b.ToTable("SourceNatRules");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.SystemConfiguration", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DHCPHostname")
                        .HasMaxLength(63)
                        .HasColumnType("TEXT");

                    b.Property<string>("Domain")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(true);

                    b.Property<string>("Hostname")
                        .IsRequired()
                        .HasMaxLength(63)
                        .HasColumnType("TEXT");

                    b.Property<uint>("ShellHistoryLength")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(1000u);

                    b.HasKey("RevisionId");

                    b.ToTable("SystemConfigurations");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Sentinel.Core.Entities.VlanInterface", b =>
                {
                    b.Property<int>("RevisionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InterfaceName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ParentInterfaceName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<ushort>("VlanId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RevisionId", "InterfaceName");

                    b.ToTable("vlaninterfaces");
                });
#pragma warning restore 612, 618
        }
    }
}
