using BlazingShop.Shared.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace BlazingShop.Shared.Services
{
    public class BlazingShopServices : IBlazingShopServices
    {
        private readonly string connectionString;

        public BlazingShopServices(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Asynchronous method for getting all products
        public async Task<IEnumerable<Products>> GetAllProductAsync()
        {
            List<Products> lstProduct = new List<Products>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetProduct", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        await con.OpenAsync();
                        using (SqlDataReader rdr = await cmd.ExecuteReaderAsync())
                        {
                            while (await rdr.ReadAsync())
                            {
                                try
                                {
                                    Products product = new Products
                                    {
                                        // Safe casting with exception handling for debugging purposes
                                        Id = rdr.GetInt32(0), // Assuming this column is NOT NULL
                                        Title = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1), // Handle NULL for Title
                                        Description = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2), // Handle NULL for Description
                                        Image = rdr.IsDBNull(3) ? new byte[0] : rdr.GetSqlBytes(3).Value, // Handle NULL for Image (as byte[])
                                        Price = rdr.IsDBNull(4) ? 0 : rdr.GetDecimal(4), // Handle NULL for Price
                                        OrginalPrice = rdr.IsDBNull(5) ? 0 : rdr.GetDecimal(5) // Handle NULL for Original Price
                                    };

                                    lstProduct.Add(product);
                                }
                                catch (IndexOutOfRangeException idxEx)
                                {
                                    // If there is an issue with column indexing
                                    Console.WriteLine($"Column indexing error: {idxEx.Message}");
                                    throw;
                                }
                                catch (InvalidCastException castEx)
                                {
                                    // If there is a data type mismatch
                                    Console.WriteLine($"Data type error: {castEx.Message}");
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    // Generic error
                                    Console.WriteLine($"Error while processing row: {ex.Message}");
                                    throw;
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // SQL-specific exception handling
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                // Catch any other general exceptions
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return lstProduct;
        }

        // Asynchronous method for adding a new product
        public async Task AddProductAsync(Products product)
        {

           
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("AddNewProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                   // cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@Title", product.Title);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    cmd.Parameters.AddWithValue("@Image", product.Image);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@OrginalPrice", product.OrginalPrice);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Asynchronous method for updating a product
        public async Task UpdateProductAsync(Products product)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UpdateProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@Title", product.Title);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    cmd.Parameters.AddWithValue("@Image", product.Image);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@OrginalPrice", product.OrginalPrice);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Asynchronous method for deleting a product
        public async Task DeleteProductAsync(int productId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DeleteProduct", con)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@Id", productId);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<string>> GetDistinctTitlesAsync()
        {

            var distinctTitles = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("GetDistinctTitles", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            distinctTitles.Add(reader.GetString(1)); 
                        }
                    }
                }
            }

            return distinctTitles;
        }
      


    public async Task<List<string>> GetDistinctDescriptionsAsync()
        {
            var distinctDescriptions = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("GetDistinctDescriptions", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            distinctDescriptions.Add(reader.GetString(2));
                        }
                    }
                }
            }

            return distinctDescriptions;
        }
    }
}
