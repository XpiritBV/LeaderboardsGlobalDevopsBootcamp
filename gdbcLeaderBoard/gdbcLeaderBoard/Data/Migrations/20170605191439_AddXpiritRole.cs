using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gdbcLeaderBoard.Data.Migrations
{
    public partial class AddXpiritRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
           INSERT 
             INTO [dbo].[AspNetRoles]
           VALUES(1,GETDATE(), 'Xpirit','Xpirit')

           INSERT 
             INTO [dbo].[AspNetRoles]
           VALUES(2,GETDATE(), 'Venue','Venue')
             
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
