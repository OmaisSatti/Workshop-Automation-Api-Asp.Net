using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Vehicle_Workshop_Automation_Api.Models;
using System.Data.Entity;

namespace Vehicle_Workshop_Automation_Api.Controllers
{
    public class CustomerController : ApiController
    {
        Vehicle_WorkShop_AutomationEntities db = new Vehicle_WorkShop_AutomationEntities();

        [HttpPost]
        public IHttpActionResult Login(Customer customer)
        {
            try
            {
                var user = CustomerLogin(customer);
                if (user != null)
                {
                    user.Role = "Customer";
                    return Json(user);
                }
                else
                {
                    var admin = AdminLogin(customer);
                    if (admin != null)
                    {
                        admin.Role = "Admin";
                        return Json(admin);
                    }
                    else
                    {
                        var mechanic = MechanicLogin(customer);
                        if (mechanic != null)
                        {
                            mechanic.Role = "Mechanic";
                            return Json(mechanic);
                        }
                        else
                        {
                            //return NotFound();
                            var surveyor = SurveyorLogin(customer);
                            if (surveyor != null)
                            {
                                surveyor.Role = "Surveyor";
                                return Json(surveyor);
                            }
                            else
                            {
                                return NotFound();
                            }
                        } // end of else.
                    } // end of else.
                } // end of else.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public Admin AdminLogin(Customer customer)
        {
            var admin = db.Admins.Where(c => c.Mobile_No == customer.Mobile_No && c.Password == customer.Password).FirstOrDefault();
            if (admin != null)
            {
                return admin;
            }
            return admin;
        }

        [HttpPost]
        public Customer CustomerLogin(Customer customer)
        {
            var cus = db.Customers.FirstOrDefault(c => c.Mobile_No == customer.Mobile_No && c.Password == customer.Password);
            if (cus != null)
            {
                return cus;
            }
            return cus;
        }

        [HttpPost]
        public Mechanic MechanicLogin(Customer customer)
        {
            var machanic = db.Mechanics.Where(c => c.Mobile_No == customer.Mobile_No && c.Password == customer.Password).FirstOrDefault();
            if (machanic != null)
            {
                return machanic;
            }
            return machanic;
        }

        [HttpPost]
        public Surveyor SurveyorLogin(Customer customer)
        {
            var surveyor = db.Surveyors.Where(c => c.Mobile_No == customer.Mobile_No && c.Password == customer.Password).FirstOrDefault();
            if (surveyor != null)
            {
                return surveyor;
            }
            return surveyor;
        }

        [HttpPost]
        public IHttpActionResult CustomerSignup(Customer customer)
        {
            try
            {
                var user = db.Customers.Where(c => c.Mobile_No == customer.Mobile_No).FirstOrDefault();
                if (user == null)
                {
                    db.Customers.Add(customer);
                    int n = db.SaveChanges();
                    if (n > 0)
                        return Ok();
                    else
                        return NotFound();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public IHttpActionResult GetVehicleCompany()
        {
            try
            {
                var vehicles = db.Vehicle_Company.ToList();
                if (vehicles != null)
                {
                    return Ok(vehicles);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public IHttpActionResult GetVehicleBasedOnCompany(int id)
        {
            try
            {
                var vehicles = db.Vehicle_Name.Where(s => s.Company_Id== id).ToList();
                if (vehicles != null)
                {
                    return Ok(vehicles);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public IHttpActionResult CustomersVehicles(string mobileno)
        {
            try
            {
                var vehicle = db.Vehicle_Customer.Where(c => c.Customer_Mobile_No == mobileno).ToList();
                if (vehicle == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(vehicle);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IHttpActionResult AddVehicle(Vehicle_Customer vehiclecustomer)
        {
            try
            {
                var vehicle = db.Vehicle_Customer.Where(c => c.Reg_No == vehiclecustomer.Reg_No).FirstOrDefault();
                if (vehicle == null)
                {
                    db.Vehicle_Customer.Add(vehiclecustomer);
                    int n = db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public IHttpActionResult CustomerAppointments(Appointment appointment)
        {
            try
            {

                var appoint = db.Appointments.Where(c => c.Appointment_No == appointment.Appointment_No).FirstOrDefault();
                if (appoint == null)
                {
                    db.Appointments.Add(appointment);
                    int n = db.SaveChanges();
                    if (n > 0)
                        return Ok();
                    else
                        return NotFound();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpDelete]
        public IHttpActionResult DeleteCustomerVehicle(int id,string Reg_No)
        {
            var vehicleCustomer = db.Vehicle_Customer.Where(t => t.Id == id && t.Reg_No == Reg_No).FirstOrDefault();
            var app=db.Appointments.Where(d => d.Vehicle_Reg_No == Reg_No).ToList();
            if (app.Count>0) 
            {
                foreach (var item in app)
                {
                    var ape= db.App_Service.Where(d => d.App_No == item.Appointment_No).ToList();
                    if (ape.Count>0) 
                    {
                        foreach (var val in ape)
                        {
                            db.Entry(val).State = EntityState.Deleted;
                            db.SaveChanges();
                        }
                    }
                    
                }
                foreach(var item in app)
                {
                    db.Entry(item).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            db.Entry(vehicleCustomer).State = EntityState.Deleted;
            db.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public IHttpActionResult EditUser(Vehicle_Customer vehicle_Customer)
        {
            db.Entry(vehicle_Customer).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public IHttpActionResult GetServices()
        {
            try
            {
                var services = db.Services.ToList();
                if (services != null)
                {
                    return Ok(services);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public IHttpActionResult AddAppointment(Appointment app)
        {
            try
            {
                var lst = db.Appointments.Where(a => a.Slot == app.Slot && a.Date == app.Date)
                            .OrderByDescending(o=>o.Bay_No).FirstOrDefault();
                if (lst!=null)
                {
                   
                    int bNo = int.Parse(lst.Bay_No.ToString());
                    bNo++;
                    app.Bay_No = bNo;

                }
                else
                {
                    app.Bay_No = 1;

                }
                db.Appointments.Add(app);
                db.SaveChanges();
                return Ok(app.Appointment_No);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError(ex);
            }

        }
        [HttpPost]
        public IHttpActionResult AddAppointmentService(List<App_Service> aps)
        {
            try
            {
                foreach (var item in aps)
                {
                    db.App_Service.Add(item);
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError();
            }

        }
        [HttpPost]
        public IHttpActionResult AddSurveyor(Surveyor su)
        {
            try
            {
                db.Surveyors.Add(su);
                int n = db.SaveChanges();
                return Ok();
        
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpPost]
        public IHttpActionResult AddAdmin(Admin ad)
        {
            try
            {
                db.Admins.Add(ad);
                int n = db.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpPost]
        public IHttpActionResult AddMechanic(Mechanic mc)
        {
            try
            {
                db.Mechanics.Add(mc);
                int n = db.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpGet]
        public IHttpActionResult AllAdmin()
        {
            try
            {
                var lst=db.Admins.ToList();
                return Ok(lst);

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpGet]
        public IHttpActionResult AllSurveyor()
        {
            try
            {
                var lst = db.Surveyors.ToList();
                return Ok(lst);

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpGet]
        public IHttpActionResult AllMechanic()
        {
            try
            {
                var lst = db.Mechanics.ToList();
                return Ok(lst);

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpPost]
        public IHttpActionResult EditAdmin(Admin ad)
        {
            try
            {
                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpPost]
        public IHttpActionResult EditSurveyor(Surveyor sa)
        {
            try
            {
                db.Entry(sa).State = EntityState.Modified;
                db.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpPost]
        public IHttpActionResult EditMechanic(Mechanic mc)
        {
            try
            {
                db.Entry(mc).State = EntityState.Modified;
                db.SaveChanges();
                return Ok();


            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpDelete]
        public IHttpActionResult DeleteAdmin(int id)
        {
            try
            {
                var lst= db.Admins.FirstOrDefault(t=> t.Id == id);
                db.Entry(lst).State = EntityState.Deleted;
                db.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpDelete]
        public IHttpActionResult DeleteSurveyor(int id)
        {
            try
            {
                var lst = db.Surveyors.Where(t => t.Id == id).FirstOrDefault();
                db.Entry(lst).State = EntityState.Deleted;
                db.SaveChanges();
                return Ok();

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpDelete]
        public IHttpActionResult DeleteMechanic(int id)
        {
            try
            {
                var lst = db.Mechanics.Where(t => t.Id == id).FirstOrDefault();
                db.Entry(lst).State = EntityState.Deleted;
                db.SaveChanges();
                return Ok();


            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpGet]
        public IHttpActionResult GetAppointment(string contact)
        {
            try
            {
                var services = db.Appointments.Where(a=>a.Customer_Mobile_No==contact).ToList();
                if (services != null)
                {
                    return Ok(services);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public IHttpActionResult UpdateAppointment(Appointment ap)
        {
            try
            {
                var services = db.Appointments.FirstOrDefault(a =>a.Appointment_No==ap.Appointment_No);
                if (services != null)
                {
                    services.Status = "Yes";
                    services.CurrentMiles = ap.CurrentMiles;
                    db.SaveChanges();
                    return Ok(services);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public IHttpActionResult GetSurveyorAppointment()
        {
            try
            {
                var app = db.Appointments.Where(a=>a.Status=="Yes" && a.SCheck!="Yes").ToList();
                if (app != null)
                {
                    return Ok(app);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public IHttpActionResult AllAppointments()
        {
            try
            {
                var lst = db.Appointments.ToList();
                return Ok(lst);

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpGet]
        public IHttpActionResult AllAppService()
        {
            try
            {
                var lst = db.App_Service.ToList();
                return Ok(lst);

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
        [HttpPost]
        public IHttpActionResult UpdateAppointmentService(List<App_Service> aps)
        {
            try
            {
                var serviceList = aps;
                foreach (var item in serviceList)
                {
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                }
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return InternalServerError();
            }

        }
        [HttpGet]
        public IHttpActionResult MechanicChecked(int apno)
        {
            try
            {
                var services = db.Appointments.FirstOrDefault(a => a.Appointment_No == apno);
                if (services != null)
                {
                    services.MCheck = "Yes";
                    db.SaveChanges();
                    return Ok(services);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public IHttpActionResult SurveyorChecked(int aps)
        {
            try
            {
                var services = db.Appointments.FirstOrDefault(a => a.Appointment_No == aps);
                if (services != null)
                {
                    services.SCheck = "Yes";
                    db.SaveChanges();
                    return Ok(services);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public IHttpActionResult SetTotalBill(int apno,int amount)
        {
            try
            {
                var services = db.Appointments.FirstOrDefault(a => a.Appointment_No == apno);
                if (services != null)
                {
                    services.Total_Bill = amount.ToString();
                    db.SaveChanges();
                    return Ok(services);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public IHttpActionResult GetMechanicAppointment()
        {
            try
            {
                var app = db.Appointments.Where(a => a.Status == "Yes" && a.MCheck!="Yes" && a.SCheck=="Yes").ToList();
                if (app != null)
                {
                    return Ok(app);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public IHttpActionResult AllCustomers()
        {
            try
            {
                var services = db.Customers.ToList();
                if (services != null)
                {
                    return Ok(services);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
