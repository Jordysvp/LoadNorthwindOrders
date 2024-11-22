using LoadNortwindOrders.Data.Context;
using LoadNortwindOrders.Data.Entities.NorthwindOrders;
using LoadNortwindOrders.Data.Entities.Nortwind;
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

        public async Task<OperactionResult> LoadDHW()
        {
            OperactionResult result = new OperactionResult();
            try
            {
                await ClearTables();

                await LoadDimEmployee();
                //await LoadDimProductCategory();
                //await LoadDimCustomers();
                //await LoadDimShippers();
                
                
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el DWH Ventas. {ex.Message}";
            }

            return result;
        }

        private async Task<OperactionResult> LoadDimCustomers()
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
                    Phone = cust.Phone,

                }).AsNoTracking()
                  .ToListAsync();

                // Carga dimension de cliente.

                string[] customersIds = customers.Select(cust => cust.IdCustomer).ToArray();

                await _northwindOrdersContext.DimCustomer.Where(cust => customersIds.Contains(cust.IdCustomer))
                                          .AsNoTracking()
                                          .ExecuteDeleteAsync();

                await _northwindOrdersContext.DimCustomer.AddRangeAsync(customers);
                await _northwindOrdersContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                operaction.Success = false;
                operaction.Message = $"Error: {ex.Message} cargando la dimension de clientes.";
            }
            return operaction;
        }

        private async Task<OperactionResult> LoadDimEmployee()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                

                // Obtener los empleados desde Northwind
                var employees = await _northwindContext.Employees.AsNoTracking().Select(emp => new DimEmployee()
                {
                    EmployeeID = emp.EmployeeId,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    Title = emp.Title,
                    BirthDate = emp.BirthDate,
                    HireDate = emp.HireDate,
                    City = emp.City,
                    Country = emp.Country,
                }).ToListAsync();

                // Insertar los nuevos empleados
                await _northwindOrdersContext.DimEmployee.AddRangeAsync(employees);
                await _northwindOrdersContext.SaveChangesAsync();

                result.Success = true;
                result.Message = $"{employees.Count} empleados cargados correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimensión de empleados: {ex.Message}";
            }

            return result;
        }



        private async Task<OperactionResult> LoadDimProductCategory()
        {
            OperactionResult result = new OperactionResult();
            try
            {
                

                var productCategories = await(from product in _northwindContext.Products
                                              join category in _northwindContext.Categories on product.CategoryId equals category.CategoryId
                                              select new DimProductCategory()
                                              {
                                                  ProductName = product.ProductName,
                                                  CategoryId = category.CategoryId,
                                                  CategoryName = category.CategoryName,
                                                  IdProduct = product.ProductId,
                                                  Description = category.Description,
                                                  }).AsNoTracking().ToListAsync();


                

                await _northwindOrdersContext.DimProductCategory.AddRangeAsync(productCategories);
                await _northwindOrdersContext.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimension de producto y categoria. {ex.Message}";
            }
            return result;
        }

        private async Task<OperactionResult> LoadDimShippers()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                

                // Obtener los shippers desde Northwind
                var shippers = await _northwindContext.Shippers.Select(ship => new DimShipper()
                {
                    IdShipper = ship.ShipperID,
                    CompanyName = ship.CompanyName,
                    Phone = ship.Phone,
                }).ToListAsync();

                // Insertar los nuevos shippers
                await _northwindOrdersContext.DimShipper.AddRangeAsync(shippers);
                await _northwindOrdersContext.SaveChangesAsync();

                result.Success = true;
                result.Message = $"{shippers.Count} shippers cargados correctamente.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimensión de shippers: {ex.Message}";
            }

            return result;
        }


        private async Task ClearTables()
        {
            try
            {
                await _northwindOrdersContext.DimEmployee.ExecuteDeleteAsync();
                await _northwindOrdersContext.DimCustomer.ExecuteDeleteAsync();
                await _northwindOrdersContext.DimProductCategory.ExecuteDeleteAsync();
                await _northwindOrdersContext.DimShipper.ExecuteDeleteAsync();
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al limpiar las tablas: {ex.Message}", ex);
            }
        }



    }
}
