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

                //await LoadDimEmployee();
                //await LoadDimProductCategory();
                //await LoadDimCustomers();
                //await LoadDimShippers();
                //await LoadDimDate();
                //await LoadViewOrderDates();
                //await LoadFactSales();
                //await LoadFactCustomerServed();
                //await LoadFactClientesAtendidos();
                await LoadFactOrders();



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
        private async Task<OperactionResult> LoadDimDate()
        {
            OperactionResult result = new OperactionResult() { Success = true };

            try
            {
                // Obtener todas las fechas únicas de VwDates   
                var vwDates = await _northwindContext.ViewOrderDates
                    .AsNoTracking()
                    .Where(v => v.DateKey.HasValue && v.FullDate.HasValue)
                    .Select(v => new
                    {
                        DateKey = v.DateKey.Value,
                        FullDate = v.FullDate.Value,
                        Year = v.Year.Value,
                        Month = v.Month.Value,
                    })
                    .Distinct()
                    .ToListAsync();

                var existingDates = await _northwindOrdersContext.DimDate
                    .Select(d => d.FullDate)
                    .ToListAsync();

                var newDates = vwDates
                    .Where(d => !existingDates.Contains(d.FullDate))
                    .Select(d => new DimDate
                    {
                        DateID = d.DateKey,
                        FullDate = d.FullDate,
                        Year = d.Year,
                        Month = d.Month,
                    })
                    .ToList();

                if (newDates.Any())
                {
                    await _northwindOrdersContext.DimDate.AddRangeAsync(newDates);
                    await _northwindOrdersContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la dimensión de fechas: {ex.Message}";
            }

            return result;
        }





        private async Task ClearTables()
        {
            try
            {
                // Eliminar datos de las Dimension Tables
                //await _northwindOrdersContext.DimEmployee.ExecuteDeleteAsync();
                //await _northwindOrdersContext.DimCustomer.ExecuteDeleteAsync();
                //await _northwindOrdersContext.DimProductCategory.ExecuteDeleteAsync();
                //await _northwindOrdersContext.DimShipper.ExecuteDeleteAsync();

                // Eliminar datos de las Fact Tables
                await _northwindOrdersContext.FactOrders.ExecuteDeleteAsync();   
                //await _northwindOrdersContext.FactClientesAtendidos.ExecuteDeleteAsync(); // FactTable3
                                                                                  

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al limpiar las tablas: {ex.Message}", ex);
            }
        }


        //cargar la vista de norwind
        private async Task<OperactionResult> LoadFactSales()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                var ventas = await _northwindContext.Vwventas.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el fact de ventas {ex.Message} ";
            }

            return result;
        }

        //cargar vista
        private async Task<OperactionResult> LoadViewOrderDates()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                
                var fechas = await _northwindContext.ViewOrderDates.AsNoTracking().ToListAsync();

                
                result.Success = true;
               
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando la vista de fechas: {ex.Message}";
            }

            return result;
        }


        //cargar la vista de norwind
        private async Task<OperactionResult> LoadFactCustomerServed()
        {
            OperactionResult result = new OperactionResult() { Success = true };

            try
            {
                var customerServed = await _northwindContext.ServedCustomers.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el fact de clientes atendidos {ex.Message} ";
            }
            return result;
        }


        //carga a mi FactClientesAtendidos
        private async Task<OperactionResult> LoadFactClientesAtendidos()
        {
            OperactionResult result = new OperactionResult() { Success = true };

            try
            {
                var customerServeds = await _northwindContext.ServedCustomers.AsNoTracking().ToListAsync();

                int[] customerIds = _northwindOrdersContext.FactClientesAtendidos.Select(cli => cli.IdClienteAtendido).ToArray();

                

                if (customerIds.Any())
                {
                    await _northwindOrdersContext.FactClientesAtendidos.Where(fact => customerIds.Contains(fact.IdClienteAtendido))
                                                            .AsNoTracking()
                                                            .ExecuteDeleteAsync();
                }

                
                foreach (var customer in customerServeds)
                {
                    var employee = await _northwindOrdersContext.DimEmployee
                                                      .SingleOrDefaultAsync(emp => emp.EmployeeID ==
                                                                               customer.EmployeeId);


                    FactClientesAtendidos factClienteAtendido = new FactClientesAtendidos()
                    {
                        IdEmployee = employee.EmployeeID,
                        TotalClientes = customer.TotalCustomersServed
                    };


                    await _northwindOrdersContext.FactClientesAtendidos.AddAsync(factClienteAtendido);

                    await _northwindOrdersContext.SaveChangesAsync();
                }

                result.Success = true;

            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = $"Error cargando el fact de clientes atendidos {ex.Message} ";
            }
            return result;
        }


        private async Task<OperactionResult> LoadFactOrders()
        {
            OperactionResult result = new OperactionResult();

            try
            {
                var ventas = await _northwindContext.Vwventas.AsNoTracking().ToListAsync();

                int[] ordersId = await _northwindOrdersContext.FactOrders.Select(cd => cd.IdOrder).ToArrayAsync();

                if (ordersId.Any())
                {
                    await _northwindOrdersContext.FactOrders.Where(cd => ordersId.Contains(cd.IdOrder))
                                                .AsNoTracking()
                                                .ExecuteDeleteAsync();
                }

                foreach (var venta in ventas)
                {
                    var customer = await _northwindOrdersContext.DimCustomer.SingleOrDefaultAsync(cust => cust.IdCustomer == venta.CustomerId);
                    var employee = await _northwindOrdersContext.DimEmployee.SingleOrDefaultAsync(emp => emp.EmployeeID == venta.EmployeeId);
                    var shipper = await _northwindOrdersContext.DimShipper.SingleOrDefaultAsync(ship => ship.IdShipper == venta.ShipperId);
                    var product = await _northwindOrdersContext.DimProductCategory.SingleOrDefaultAsync(pro => pro.IdProduct == venta.ProductId);

                    
                    var date = await _northwindOrdersContext.DimDate.SingleOrDefaultAsync(dt => dt.DateID == venta.DateKey);

                    if (date == null)
                    {
                        
                        date = new DimDate
                        {
                            DateID = venta.DateKey.HasValue ? venta.DateKey.Value : 0, 
                                                                                       
                        };
                        
                        await _northwindOrdersContext.DimDate.AddAsync(date);
                        await _northwindOrdersContext.SaveChangesAsync();
                    }

                    FactOrders factOrder = new FactOrders()
                    {
                        CantidadVendida = venta.Cantidad.Value,
                        Country = venta.City,
                        IdCustomer = customer.IdCustomer,
                        IdEmployee = employee.EmployeeID,
                        IdFecha = date.DateID, 
                        IdProducto = product.IdProduct,
                        IdShipper = shipper.IdShipper,
                        TotalVenta = Convert.ToDecimal(venta.TotalVentas)
                    };

                    await _northwindOrdersContext.FactOrders.AddAsync(factOrder);
                    await _northwindOrdersContext.SaveChangesAsync();
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error cargando el fact de ventas {ex.Message}";
            }

            return result;
        }






    }
}
