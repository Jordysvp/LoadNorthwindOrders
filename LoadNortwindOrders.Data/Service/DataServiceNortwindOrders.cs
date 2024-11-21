using LoadNortwindOrders.Data.Context;
using LoadNortwindOrders.Data.Entities.NorthwindOrders;
using LoadNortwindOrders.Data.Interfaces;
using LoadNortwindOrders.Data.Result;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Service
{
    public class DataServiceNortwindOrders : IDataServiceNortwindOrders
    {
        private readonly NorthwindContext _northwindContext;
        private readonly NorthwindOrdersContext _northwindOrdersContext;

        public DataServiceNortwindOrders(NorthwindContext northwindContext,
                                   NorthwindOrdersContext northwindOrdersContext)
        {
            _northwindContext = northwindContext;
            _northwindOrdersContext = northwindOrdersContext;
        }

        public async Task<OperactionResult> LoadDimCustomer()
        {
            OperactionResult operaction = new OperactionResult() { Success = false };


            try
            {
                // Obtener clientes de norwind

                var customers = await _northwindContext.Customers.Select(cust => new DimCustomer()
                {
                    IdCustomer = cust.CustomerId,
                    CustomerName = cust.CompanyName,
                    City = cust.City,
                    Country = cust.Country,
                    Phone = cust.Phone

                }).AsNoTracking()
                  .ToListAsync();

                // Carga dimension de cliente.

                await _northwindOrdersContext.DimCustomers.AddRangeAsync(customers);
                await _northwindOrdersContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                operaction.Success = false;
                operaction.Message = $"Error: {ex.Message} cargando la dimension de clientes.";
            }
            return operaction;
        }

        public async Task<OperactionResult> LoadDimEmployee()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                //Obtener los empleados de la base de datos de norwind.
                var employees = await _northwindContext.Employees.AsNoTracking().Select(emp => new DimEmployee()
                {
                    EmployeeId = emp.EmployeeId,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Title = emp.Title,
                    BirthDate = emp.BirthDate,
                    HireDate = emp.HireDate,
                    City = emp.City,
                    Country= emp.Country,
                }).ToListAsync();




                // Carga la dimension de empleados.

                await _northwindOrdersContext.DimEmployees.AddRangeAsync(employees);

                await _northwindOrdersContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando la dimension de empleado {ex.Message}";
            }


            return result;
        }

        public async Task<OperactionResult> LoadDimProductCategory()
        {
            OperactionResult result = new OperactionResult();
            try
            {
                

                var productCategories = await(from product in _northwindContext.Products
                                              join category in _northwindContext.Categories on product.CategoryId equals category.CategoryId
                                              select new DimProductCategory()
                                              {
                                                  ProductName = product.ProductName,
                                                  CategoryName = category.CategoryName,
                                                  IdProduct = product.ProductId,
                                                  }).AsNoTracking().ToListAsync();


                

                await _northwindOrdersContext.DimProductCategories.AddRangeAsync(productCategories);
                await _northwindOrdersContext.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimension de producto y categoria. {ex.Message}";
            }
            return result;
        }

        public async Task<OperactionResult> LoadDimShippers()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                var shippers = await _northwindContext.Shippers.Select(ship => new DimShipper()
                {
                    IdShipper = ship.ShipperID,
                    CompanyName = ship.CompanyName,
                    Phone = ship.Phone,
                }).ToListAsync();


                await _northwindOrdersContext.DimShippers.AddRangeAsync(shippers);
                await _northwindOrdersContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando la dimension de shippers {ex.Message} ";
            }
            return result;
        }
    }
}
