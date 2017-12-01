using Newtonsoft.Json;
using ProductCatalogMVC.Models;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogMVC.Services
{
    public class ProductRepository:IProductRepository
    {
        /// <summary>
        /// Connects to Database
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IRestResponse Connect(Method method)
        {
            var client = new RestClient("https://productbase-ef71.restdb.io/rest/products");
            var request = new RestRequest(method);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("x-apikey", "07c1ce3243617f0f666d1620725ad34130696");
            request.AddHeader("content-type", "application/json");
            IRestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// returns all products in order by ID from database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Product> GetAll()
        {
            IRestResponse response = Connect(Method.GET);
            string json = response.Content.ToString();
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(json);
            var orderedList = products.OrderBy(prod => prod.ProductID).ToList();
            return orderedList.ToArray();
        }
        
        /// <summary>
        /// filters ID from response
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Product GetById(int id)
        {
            IEnumerable<Product> products = this.GetAll();
            return products.FirstOrDefault(prod => prod.ProductID == id);
        }

        /// <summary>
        /// connects to database and creates a product
        /// </summary>
        /// <param name="prod"></param>
        public void CreateProduct(Product prod)
        {
            var client = new RestClient("https://productbase-ef71.restdb.io/rest/products");
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("x-apikey", "07c1ce3243617f0f666d1620725ad34130696");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"Name\":\"" + prod.Name + "\",\"Description\":\"" + prod.Description + "\",\"PricePerUnit\":\"" + prod.PricePerUnit + "\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        /// <summary>
        /// deletes a product from the Database
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProduct(int id)
        {
            IRestResponse response = Connect(Method.GET);
            string json = response.Content.ToString();
            string docid ="unknown";
             
            // get _id -> docid --> needed to delete
            string[] arr = json.Split("}");
            foreach(string item in arr)
            {  
                if (item.Contains("\"ProductID\":"+id))
                {
                    string[] specarr = item.Split(",");
                    foreach(string str in specarr)
                    {
                        System.Console.WriteLine("item: "+str);
                        if (str.Contains("_id"))
                        {
                            string[] idstr = str.Split(":");
                            docid = idstr[1].Trim().Replace("\"","");
                            System.Console.WriteLine("Document ID: "+docid);
                        }
                    }
                }
            }
            // request to delete
            var client = new RestClient("https://productbase-ef71.restdb.io/rest/products/"+docid);
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("x-apikey", "07c1ce3243617f0f666d1620725ad34130696");
            request.AddHeader("content-type", "application/json");
            IRestResponse responsedelete = client.Execute(request);
        }

        /// <summary>
        /// searches a product by the name given
        /// </summary>
        /// <param name="lookout"></param>
        /// <returns></returns>
        public IEnumerable<Product> ProductSearch(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IEnumerable<Product> products = this.GetAll();
                var sol = new List<Product>();

                foreach (var item in products)
                {
                    if (item.Name.Trim().ToLower().Contains(name.Trim().ToLower()))
                    {
                        sol.Add(item);
                    }
                }
                return sol.ToArray();
            }
            return null;
        }
    }
}
