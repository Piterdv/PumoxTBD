namespace PumoxTBD.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    //ADD MOdels:)
    using PumoxTBD.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<PumoxTBD.Models.PumoxTBDContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PumoxTBD.Models.PumoxTBDContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Companies.AddOrUpdate(x => x.Id,
                new Company() { Id = 1, Name = "PRS", EstablishmentYear = 2015 },
                new Company() { Id = 2, Name = "Piroundsoft", EstablishmentYear = 2013 },
                new Company() { Id = 3, Name = "Nova Corp", EstablishmentYear = 1 }
                );

            context.Employees.AddOrUpdate(x => x.Id,
                new Employee() { Id = 1, CompanyId = 1, FirstName = "Pit", LastName = "Lud", DateOfBirth = DateTime.Parse("2000-1-1"), JobTitle = JobTitle.Administrator.ToString() },
                new Employee() { Id = 2, CompanyId = 1, FirstName = "Moo", LastName = "Boo", DateOfBirth = DateTime.Parse("1990-1-1"), JobTitle = JobTitle.Manager.ToString() },
                new Employee() { Id = 3, CompanyId = 2, FirstName = "Ala", LastName = "Mala", DateOfBirth = DateTime.Parse("1980-5-13"), JobTitle = JobTitle.Administrator.ToString() },
                new Employee() { Id = 4, CompanyId = 3, FirstName = "Mis", LastName = "Boom", DateOfBirth = DateTime.Parse("1981-12-23"), JobTitle = JobTitle.Architect.ToString() },
                new Employee() { Id = 5, CompanyId = 3, FirstName = "Aaa", LastName = "Baa", DateOfBirth = DateTime.Parse("1975-7-23"), JobTitle = JobTitle.Developer.ToString() }
                );
        }
    }
}
