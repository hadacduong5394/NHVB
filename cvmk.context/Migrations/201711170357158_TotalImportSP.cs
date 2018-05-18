namespace cvmk.context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TotalImportSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetTotalImportSP", p => new
            {
                fromDate = p.String(),
                toDate = p.String(),
                comid = p.String()
            },
            @"select ip.CreateDate Date, SUM(ip.TotalAmount) Total from ImportProducts ip 
            where ComId = cast(@comid as int) and Status = 1 and ip.CreateDate >= cast(@fromDate as date) and ip.CreateDate <= cast(@toDate as date)
            Group by ip.CreateDate;");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.GetTotalImportSP");
        }
    }
}
