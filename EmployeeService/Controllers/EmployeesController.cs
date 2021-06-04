using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace EmployeeService.Controllers
{
    public class EmployeesController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        public Employee GetEmployee(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.FirstOrDefault(e => e.ID == id);
            }
        }

        public HttpResponseMessage PostEmployee([FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    entities.Employees.Add(employee);
                    entities.SaveChanges();

                    //HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.Created);

                    var response = Request.CreateResponse(HttpStatusCode.Created, employee);
                    response.Headers.Location = new Uri($"{Request.RequestUri}{employee.ID.ToString()}");
                        
                    return response;

                }
            }
            catch (Exception Ex)
            {

                //var response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Ex.Message);
                
            }

        }

    
    }
}