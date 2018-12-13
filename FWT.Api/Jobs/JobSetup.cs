namespace Dormer.Scheduler.Jobs
{
    using Dapper;
    using FWT.Core.Services.Dapper;
    using FWT.Core.Services.Sql;

    public static class JobsSetup
    {
        public static void Purge(IDatabaseConnector<HangfireDatabaseCredentials> database)
        {
            database.Execute(conn =>
            {
                conn.Execute(@"
                ALTER TABLE [HangFire].[State] DROP CONSTRAINT [FK_HangFire_State_Job];
                ALTER TABLE [HangFire].[JobParameter] DROP CONSTRAINT [FK_HangFire_JobParameter_Job];
                DROP TABLE [HangFire].[Schema];
                DROP TABLE [HangFire].[Job];
                DROP TABLE [HangFire].[State];
                DROP TABLE [HangFire].[JobParameter];
                DROP TABLE [HangFire].[JobQueue];
                DROP TABLE [HangFire].[Server];
                DROP TABLE [HangFire].[List];
                DROP TABLE [HangFire].[Set];
                DROP TABLE [HangFire].[Counter];
                DROP TABLE [HangFire].[Hash];
                DROP TABLE [HangFire].[AggregatedCounter];
                DROP SCHEMA [HangFire];");
            });
        }
    }
}
