using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PumoxTBD.Models;

namespace PumoxTBD.Controllers
{
    public class CompaniesController : ApiController
    {
        private PumoxTBDContext db = new PumoxTBDContext();

        //Najprawdopodobniej źle stworzyłem EF + DB - nie ma automatycznego pobierania Company + Employee
        //jednak dobrze to zrobiłem - działa PUT + JSON Company + List<Employee>
        // GET: api/Companies
        //public IQueryable<Company> GetCompanies()
        //{
        //    return db.Companies; //.Include(e=>;
        //}


        //OK!
        // GET: api/Companies
        [ActionName("Get")]
        public IQueryable<CompanyDetailDTO> GetCompanies()
        {
            var cmp = from c in db.Companies
                      select new CompanyDetailDTO()
                      {
                          Id = c.Id,
                          Name = c.Name,
                          EstablishmentYear = c.EstablishmentYear,
                          Employees = db.Employees.Where(e => e.CompanyId == c.Id).ToList<Employee>()
                      };
            return cmp;
        }

        //to nie działa z serializacją - wyskakuje błąd JSON!!!
        //public IQueryable<Company> GetCompanies()
        //{
        //    var cmp = from c in db.Companies
        //              select new Company()
        //              {
        //                  Id = c.Id,
        //                  Name = c.Name,
        //                  Employees = db.Employees.Where(e => e.CompanyId == c.Id).ToList<Employee>()
        //              };
        //    return cmp;
        //}

        // GET: api/Companies/5
        [ActionName("Get")]
        [ResponseType(typeof(Company))]
        public async Task<IHttpActionResult> GetCompany(long id)
        {
            Company company = await db.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            Company cmp = new Company
            {
                Id = company.Id,
                Name = company.Name,
                EstablishmentYear = company.EstablishmentYear,
                Employees = db.Employees.Where(e => e.CompanyId == id).ToList<Employee>()
            };

            //return Ok(company);
            return Ok(cmp);
        }

        //-----------UPDATE---------OK:)

        // PUT: api/Employees/5
        // PUT: /companies/upc/123
        [ActionName("upc")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmployee(long id, Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("1. Bad model: " + ModelState);
            }

            //wymuszam "nadanie" id - w innym przypadku nie działa?
            //ale dla EmployeesController to samo działa?
            if (id != 0 && id != company.Id)
            {
                company.Id = id; // db.Companies.Where(c => c.Id == id).FirstOrDefault<Company>();
            }

            //Faza I
            //dodaję Company - wycynając jednocześnie Employee
            //w przeciwnym wypadku następuje dublowanie z zapisaniem w kluczach obcych
            IEnumerable<Employee> ieE = company.Employees;
            company.Employees = null;

            if (id != company.Id)
            {
                return BadRequest($"3. Ther's no Id! ({id}|{company.Id})");
            }

            db.Entry(company).State = EntityState.Modified;

            //--1 faza OK

            //Faza II
            //prubuję dodać nowych Employee - musi być nadany Emplyee.CompanyID przed zapisaniem!!!
            //może być problem - zapisuje Company, i jednocześnie Employee, w momencie tego zapisu?
            //co prowadzi do problemu z kluczem obcym?
            if (ieE != null && ieE.Count() > 0)
            {
                foreach (Employee e in ieE)
                {
                    Employee em = new Employee();
                    em.CompanyId = id;
                    em.FirstName = e.FirstName;
                    em.LastName = e.LastName;
                    em.DateOfBirth = e.DateOfBirth;
                    em.JobTitle = e.JobTitle;
                    db.Employees.Add(em);
                    //ponieważ mamy powiązanie m-y Company a Employee - nie możemy wymuszać podwójnego
                    //EntityState.Modified wystarcza, i tak wymusza odświeżenie całego EF (chyba:)
                    //db.Entry(em).State = EntityState.Modified;
                }
            }


            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        #region cmpCreateV1
        //// PUT: api/Companies/5
        //// PUT: /companies/update/123
        ////[HttpPut]
        //[ActionName("upc")]
        //[ResponseType(typeof(void))]
        ////[ResponseType(typeof(List<Employee>))]
        ////public async Task<IHttpActionResult> PutCompany(long id, Company company)
        //public IHttpActionResult PutCompany(long id, Company company)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    #region Original
        //    //o co mu chodzi? - nie mam company.Id - mam tylko id z URL!
        //    //if (id != company.Id)
        //    //{
        //    //    return BadRequest($"Where's this stupid Company...? (id={id})");
        //    //}

        //    //db.Entry(company).State = EntityState.Modified;

        //    //try
        //    //{
        //    //    await db.SaveChangesAsync();
        //    //}
        //    //catch (DbUpdateConcurrencyException)
        //    //{
        //    //    if (!CompanyExists(id))
        //    //    {
        //    //        return NotFound();
        //    //    }
        //    //    else
        //    //    {
        //    //        throw;
        //    //    }
        //    //}

        //    //return StatusCode(HttpStatusCode.NoContent); 
        //    #endregion

        //    using (var ctx = new PumoxTBDContext())
        //    {
        //        var exCompany = ctx.Companies.Where(c => c.Id == id)
        //                                                .FirstOrDefault<Company>();

        //        if (exCompany != null)
        //        {
        //            exCompany.Name = company.Name;
        //            exCompany.EstablishmentYear = company.EstablishmentYear;

        //            ctx.SaveChanges();
        //        }
        //        else
        //        {
        //            return NotFound();
        //        }

        //        //!Employee - coś mi to nie działa z automatu:)
        //        //i tak się zastanawiam, czy mozliwe jest aby ziałało:)
        //        //jedynie mogę dodać nowe pozycje do Employee!!!
        //        //teraz sprawdź List<Employee>!!!

        //    }

        //    List<Employee> edto = company.Employees.ToList();

        //    return Ok();
        //} 
        #endregion

        //// POST: api/Companies
        ////POST: /companies/create
        //[ActionName("create")]
        //[ResponseType(typeof(Company))]
        //public async Task<IHttpActionResult> PostCompany(Company company)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Companies.Add(company);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = company.Id }, company);
        //}
        //{"Name":"Hakki","EstablishmentYear":2010,"Employees"} - OK, idzie:)


        //!!!Działa z automatu + new employee!!!!
        //POST: /companies/create
        [ActionName("create")]
        //[ResponseType(typeof(Company))]
        //response type - do zwrotu jedynie nowe Id
        [ResponseType(typeof(CompanyIdDTO))]
        public async Task<IHttpActionResult> PostCompany(Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Companies.Add(company);
            await db.SaveChangesAsync();

            //return CreatedAtRoute("DefaultApi", new { id = company.Id }, company);
            return CreatedAtRoute("MyApi", new { id = company.Id }, new CompanyIdDTO() { Id = company.Id });
        }

        // DELETE: api/Companies/5
        // DELETE /companies/delete/123
        [ActionName("delete")]
        [ResponseType(typeof(Company))]
        public async Task<IHttpActionResult> DeleteCompany(long id)
        {
            Company company = await db.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            db.Companies.Remove(company);
            await db.SaveChangesAsync();

            return Ok(company);
        }

        #region SearchResult - problem
        //[HttpGet]
        //[ResponseType(typeof(List<CompanyDetailDTO>))]
        ////public async Task<IHttpActionResult> SearchAsync([FromUri] SearchOptions searchOptions)
        //public async Task<IHttpActionResult> SearchAsync(SearchOptions searchOptions)
        //{
        //    if (searchOptions == null)
        //    {
        //        return BadRequest("Invalid search options");
        //    }

        //    var searchResult = await db.SearchAsync(searchOptions);

        //    return Ok(searchResult);
        //} 
        #endregion

        [HttpPost]
        [ActionName("search")]
        [ResponseType(typeof(List<CompanyDetailDTO>))]
        #region inne mieszania - usuń!
        //[ResponseType(typeof(List<CompanyDetailDTO>))]
        //Task + LINQ - nie za bardzo!!!
        //public async Task<IHttpActionResult> SearchAsync([FromUri] SearchOptions searchOptions)
        //public IHttpActionResult Search(SearchOptions so)
        //OK, działa z URI: [FromUri] SearchOptions so
        //public IQueryable<CompanyDetailDTO> Search(SearchOptions so)
        //public IQueryable<CompanyDetailDTO> Search(SearchOptions so)
        //[ResponseType(typeof(SearchOptions))] 
        #endregion
        //LINQ + Task = boom
        public IHttpActionResult Search(SearchOptions so)
        {
            if (so == null)
            {
                return BadRequest("Where're your search options, doc?");
            }

            DateTime dOfBFrom;
            DateTime dOfBTo;
            DateTime.TryParse(so.DateOfBirthFrom, out dOfBFrom);
            DateTime.TryParse(so.DateOfBirthTo, out dOfBTo);

            var sr = from c in db.Companies
                     join e in db.Employees on c.Id equals e.CompanyId
                     where (
                     c.Name.Contains(so.Keywords)
                     || e.FirstName.Contains(so.Keywords)
                     || e.LastName.Contains(so.Keywords)
                     || e.JobTitle.Contains(so.JobTitles)
                     || (e.DateOfBirth >= dOfBFrom && e.DateOfBirth <= dOfBTo)
                     )
                     select new CompanyDetailDTO()
                     {
                         Id = c.Id,
                         Name = c.Name,
                         EstablishmentYear = c.EstablishmentYear,
                         Employees = db.Employees.Where(e => e.CompanyId == c.Id).ToList<Employee>()
                     };

            return Ok(sr);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompanyExists(long id)
        {
            return db.Companies.Count(e => e.Id == id) > 0;
        }
    }
}