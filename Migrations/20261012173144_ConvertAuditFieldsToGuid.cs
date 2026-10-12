using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Market.Server.Migrations
{
    /// <inheritdoc />
    public partial class ConvertAuditFieldsToGuid : Migration
    {
        private const string GuidTextPattern =
            "^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ConvertAuditColumnToUuid(migrationBuilder, "Products", "EditedBy");
            ConvertAuditColumnToUuid(migrationBuilder, "Products", "CreatedBy");
            ConvertAuditColumnToUuid(migrationBuilder, "Orders", "EditedBy");
            ConvertAuditColumnToUuid(migrationBuilder, "Orders", "CreatedBy");
            ConvertAuditColumnToUuid(migrationBuilder, "Categories", "EditedBy");
            ConvertAuditColumnToUuid(migrationBuilder, "Categories", "CreatedBy");
            ConvertAuditColumnToUuid(migrationBuilder, "Carts", "EditedBy");
            ConvertAuditColumnToUuid(migrationBuilder, "Carts", "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ConvertAuditColumnToText(migrationBuilder, "Products", "EditedBy");
            ConvertAuditColumnToText(migrationBuilder, "Products", "CreatedBy");
            ConvertAuditColumnToText(migrationBuilder, "Orders", "EditedBy");
            ConvertAuditColumnToText(migrationBuilder, "Orders", "CreatedBy");
            ConvertAuditColumnToText(migrationBuilder, "Categories", "EditedBy");
            ConvertAuditColumnToText(migrationBuilder, "Categories", "CreatedBy");
            ConvertAuditColumnToText(migrationBuilder, "Carts", "EditedBy");
            ConvertAuditColumnToText(migrationBuilder, "Carts", "CreatedBy");
        }

        private static void ConvertAuditColumnToUuid(MigrationBuilder migrationBuilder, string table, string column)
        {
            migrationBuilder.Sql(
                $"""
                ALTER TABLE "{table}" ALTER COLUMN "{column}" TYPE uuid USING (
                    CASE
                        WHEN TRIM("{column}") ~ '{GuidTextPattern}'
                            THEN TRIM("{column}")::uuid
                        ELSE '00000000-0000-0000-0000-000000000000'::uuid
                    END
                );
                """);
        }

        private static void ConvertAuditColumnToText(MigrationBuilder migrationBuilder, string table, string column)
        {
            migrationBuilder.Sql(
                $"""
                ALTER TABLE "{table}" ALTER COLUMN "{column}" TYPE text USING "{column}"::text;
                """);
        }
    }
}
