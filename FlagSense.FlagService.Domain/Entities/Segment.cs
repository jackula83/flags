﻿using Common.Domain.Core.Extensions;
using FlagSense.FlagService.Core.Extensions;
using FlagSense.FlagService.Core.Models;
using FlagSense.FlagService.Domain.Interfaces;
using FlagSense.FlagService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing;

namespace FlagSense.FlagService.Domain.Entities
{
    public class Segment : FsEntity, IRuleTarget, IColourCoding
    {
        public int EnvironmentId { get; set; }
        public Env? Environment { get; set; }

        public static int DefaultColour = Color.DeepSkyBlue.ToArgb();
        public int ColourCoding { get; set; } = DefaultColour;

        public List<Flag> Flags { get; set; } = new();
        public bool IsEnabled { get; set; } = false;
        public FlagValue DefaultServe { get; set; } = new();
        public List<RuleGroup> RuleGroups { get; set; } = new();

        public override void SetupEntity(ModelBuilder builder)
        {
            var entity = builder.Entity<Segment>();
            entity.HasMany(e => e.Flags);                
            entity.HasMany(e => e.RuleGroups);
            entity
                .HasOne(e => e.Environment)
                .WithMany(v => v.Segments)
                .HasForeignKey(nameof(EnvironmentId));
            entity
                .Property(e => e.DefaultServe)
                .HasConversion(
                    v => v.Serialise(),
                    v => v.Deserialise<FlagValue>()!);
        }
    }
}