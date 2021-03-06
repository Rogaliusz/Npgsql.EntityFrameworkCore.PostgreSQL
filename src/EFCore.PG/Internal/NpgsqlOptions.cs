﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;

namespace Npgsql.EntityFrameworkCore.PostgreSQL.Internal
{
    /// <inheritdoc />
    public class NpgsqlOptions : INpgsqlOptions
    {
        /// <inheritdoc />
        public virtual Version PostgresVersion { get; private set; }

        /// <inheritdoc />
        public virtual bool ReverseNullOrderingEnabled { get; private set; }

        /// <inheritdoc />
        public virtual bool CaseInsensitiveEnabled { get; private set; }

        /// <inheritdoc />
        [NotNull]
        public virtual IReadOnlyList<UserRangeDefinition> UserRangeDefinitions { get; private set; }

        public NpgsqlOptions()
            => UserRangeDefinitions = new UserRangeDefinition[0];

        /// <inheritdoc />
        public void Initialize(IDbContextOptions options)
        {
            var npgsqlOptions = options.FindExtension<NpgsqlOptionsExtension>() ?? new NpgsqlOptionsExtension();

            PostgresVersion = npgsqlOptions.PostgresVersion;
            ReverseNullOrderingEnabled = npgsqlOptions.ReverseNullOrdering;
            UserRangeDefinitions = npgsqlOptions.UserRangeDefinitions;
            CaseInsensitiveEnabled = npgsqlOptions.CaseInsensitive;
        }

        /// <inheritdoc />
        public void Validate(IDbContextOptions options)
        {
            var npgsqlOptions = options.FindExtension<NpgsqlOptionsExtension>() ?? new NpgsqlOptionsExtension();

            if (ReverseNullOrderingEnabled != npgsqlOptions.ReverseNullOrdering)
            {
                throw new InvalidOperationException(
                    CoreStrings.SingletonOptionChanged(
                        nameof(NpgsqlDbContextOptionsBuilder.ReverseNullOrdering),
                        nameof(DbContextOptionsBuilder.UseInternalServiceProvider)));
            }
        }
    }
}
