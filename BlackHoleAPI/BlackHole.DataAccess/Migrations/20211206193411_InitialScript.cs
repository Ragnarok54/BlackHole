using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace BlackHole.DataAccess.Migrations
{
    public partial class InitialScript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", @"initialScript.sql");
            var script = File.ReadAllText(sqlFile);

            migrationBuilder.Sql(script);
        }
    }
}
