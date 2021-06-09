using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;
using System.Web.Http.Cors;

namespace EmployeeService.Controllers
{
   // [EnableCorsAttribute("*","*","*")]
    public class EmployeesController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage LoadEmployees(string gender="All")
        {
            if (gender == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter can not be empty");
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    switch (gender.ToLower())
                    {
                        case "all":
                            return Request.CreateResponse(HttpStatusCode.OK,
                                entities.Employees.ToList());
                        case "male":
                            return Request.CreateResponse(HttpStatusCode.OK,
                                entities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                        case "female":
                            return Request.CreateResponse(HttpStatusCode.OK,
                                entities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                      
                        default:
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"Gender value" +
                                $" should be either All, Male or Female. {gender} is invalid");

                    }

                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.StackTrace);
            }
            
        }

        [HttpGet]
        public HttpResponseMessage LoadEmployeeById(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }

                else 
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Employee with Id:{id.ToString()} could not be found.");
            }
        }

        [HttpPost]
        public HttpResponseMessage AddNewEmployee([FromBody] Employee employee)
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

        [HttpDelete]
        public HttpResponseMessage RemoveEmployee(int id)
        {
            using(EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                try
                {
                    var entity = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if(entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The entity with Id:"+ id.ToString() + " does not exist");
                    }

                    entities.Employees.Remove(entity);
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "Employee Successfully deleted");

                }
                catch (Exception Ex)
                {

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"An error ocurred.{Ex.Message}");
                }
              
            }

        }

        [HttpPut]
        public HttpResponseMessage ModifyEmployee(int id, [FromUri] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var empl = entities.Employees.FirstOrDefault(e => e.ID == id);

                    if (empl == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Requested entity does not exist");
                    }

                    empl.FirstName = employee.FirstName;
                    empl.LastName = employee.LastName;
                    empl.Salary = employee.Salary;
                    empl.Gender = employee.Gender;

                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "Update was successful");
                }

            }
            catch (Exception)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "An error occured");
            }
             
        }

    
    }
}