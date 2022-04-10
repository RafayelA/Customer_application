using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ACBACustomer.Data.DataModel;

namespace ACBACustomer.Data.DataAccess
{
    /// <summary>
    /// Interface ICustomerAccess
    /// </summary>
    public interface ICustomerAccess
    {
        /// <summary>
        /// Method to get customer by Id
        /// </summary>
        /// <returns>Data table</returns>
        DataRow GetCustomerById(int Id);

        /// <summary>
        /// Method to get all customers
        /// </summary>
        /// <returns>Data table</returns>
        DataTable GetAllCustomers();

        /// <summary>
        /// Service method to search records by multiple parameters
        /// </summary>
        /// <param name="firstName">first name value</param>
        /// <param name="lastName">last name value</param>
        /// <param name="docNumber">document number value</param>
        /// <returns>Data table</returns>
        DataTable SearchCustomers(string firstName, string lastName, string docNumber);

        /// <summary>
        /// Service method to create new customer
        /// </summary>
        /// <param name="customer">customer model</param>
        /// <returns>true or false</returns>
        bool AddCustomer(Customer customer);

        /// <summary>
        /// Service method to update customer
        /// </summary>
        /// <param name="customer">customer</param>
        /// <returns>true / false</returns>
        bool UpdateCustomer(Customer customer);

        /// <summary>
        /// Method to delete a customer
        /// </summary>
        /// <param name="id">customer id</param>
        /// <returns>true / false</returns>
        bool DeleteCustomer(int id);
    }
}
