using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ACBACustomer.Data.DataAccess;
using ACBACustomer.Data.DataModel;

namespace ACBACustomer.Data.BusinessService
{
    /// <summary>
    /// class to query the DataAccess, implements ICustomerService interface
    /// </summary>
    public class CustomerService : ICustomerService
    {
        /// <summary>
        /// interface of CustomerAccess
        /// </summary>
        private ICustomerAccess customerAccess;

        /// <summary>
        /// Initializes a new instance of the CustomerService class
        /// </summary>
        public CustomerService()
        {
            //this.customerAccess = new CustomerAccess();
        }

        /// <summary>
        /// Service method to get customer by Id
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>Data row</returns>
        public DataRow GetCustomerById(int id)
        {
            return this.customerAccess.GetCustomerById(id);
        }

        /// <summary>
        /// Service method to get all customers
        /// </summary>
        /// <returns>Data table</returns>
        public DataTable GetAllCustomers()
        {
            return this.customerAccess.GetAllCustomers();
        }

        /// <summary>
        /// Service method to search records by multiple parameters
        /// </summary>
        /// <param name="firstName">first name value</param>
        /// <param name="lastName">last name value</param>
        /// <param name="docNumber">document number value</param>
        /// <returns>Data table</returns>
        public DataTable SearchCustomers(string firstName, string lastName, string docNumber)
        {
            return this.customerAccess.SearchCustomers(firstName, lastName, docNumber);
        }

        /// <summary>
        /// Service method to create new customer
        /// </summary>
        /// <param name="customer">customer model</param>
        /// <returns>true or false</returns>
        public bool RegisterCustomer(Customer customer)
        {
            return this.customerAccess.AddCustomer(customer);
        }

        /// <summary>
        /// Service method to update customer
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>true / false</returns>
        public bool UpdateCustomer(Customer customer)
        {
            return this.customerAccess.UpdateCustomer(customer);
        }

        /// <summary>
        /// Method to delete a customer
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>true / false</returns>
        public bool DeleteCustomer(int id)
        {
            return this.customerAccess.DeleteCustomer(id);
        }
    }
}
