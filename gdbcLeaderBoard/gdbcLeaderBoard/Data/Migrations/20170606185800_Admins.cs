using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace gdbcLeaderBoard.Data.Migrations
{
    public partial class Admins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var admin = new[] {
"XpiritAdmin"
        };
            foreach (string venuename in admin)
            {
                Guid userid = Guid.NewGuid();
                string username = venuename.Replace('(', ' ').Replace(')', ' ').Replace('/', ' ').Replace(" ", "") + "@gdbc.com";

                string query = @"
           INSERT 
             INTO [dbo].[AspNetUsers]
           VALUES('{userid}'
                  ,0
                  ,'728ee199-cdbf-47a7-8bae-905b9535dc3a','{venuename}',1,1,NULL,'{venuename}','{usernameN}','AQAAAAEAACcQAAAAEMzV0+oS9NLk6gliDBWh/NhJVyj1USYLZjJRNypc5DSJBndXiQ7UFtmhwshAGE62Dg==',NULL,0,'247fa299-8d33-4e39-b2e5-20e474784f7f',0,'{username}')

            INSERT 
             INTO [dbo].[AspNetUserRoles]
            VALUES ('{userid}',1)

          
";

                query = query.Replace("{venuename}", venuename);
                query = query.Replace("{userid}", userid.ToString());
                query = query.Replace("{username}", username);
                query = query.Replace("{usernameN}", username.ToUpper());

                migrationBuilder.Sql(query);
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
