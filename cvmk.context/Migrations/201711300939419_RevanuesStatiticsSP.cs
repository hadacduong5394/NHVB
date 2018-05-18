namespace cvmk.context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevanuesStatiticsSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("GetRevanuesStatiticsSP", p => new
            {
                fromDate = p.String(),
                toDate = p.String(),
                comid = p.String()
            },
            @"select temp.Date as Date, SUM(temp.reve) as Revanues, SUM(temp.ben) as Benefit from 
            (select o.CreateDate as Date, SUM(o.Total) as reve, (SUM(o.Total) - SUM(odt.RootPrice)) as ben
            from Orders as o inner join OrderDetails as odt on o.Id = odt.OrderId
            where o.ComId = cast(@comid as int)
            and o.CreateDate >= cast(@fromDate as date) and o.CreateDate <= cast(@toDate as date)
            group by o.CreateDate, o.Total) as temp group by temp.Date;");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.GetRevanuesStatiticsSP");
        }
    }
}
