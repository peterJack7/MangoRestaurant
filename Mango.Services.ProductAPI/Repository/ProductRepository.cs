using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Repository;
using Mango.Services.ProductAPI.DBContexts;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Newtonsoft.Json;

namespace Mango.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplictionDBContext _db;
        private IMapper _mapper;
        private readonly IDALRepository _dal;
        public ProductRepository(ApplictionDBContext db, IMapper mapper, IDALRepository dal)
        {
            _db = db;
            _mapper = mapper;
            _dal = dal;
        }
        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            Product product = _mapper.Map<ProductDto, Product>(productDto);
            if (product.ProductId > 0)
            {
                _db.Products.Update(product);
            }
            else
            {
                _db.Products.Add(product);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<Product, ProductDto>(product);
        }

        public async Task<bool> DeletProduct(int productId)
        {
            try
            {
                Product product = await _db.Products.FirstOrDefaultAsync(u => u.ProductId == productId);
                if (product == null)
                {
                    return false;
                }
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<ProductDto> GetProductById(int id)
        {
            Product product = await _db.Products.Where(x => x.ProductId == id).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            string result = "";
            DataSet ds = await _dal.getDataSetForSqlParam("sp_getproducts", null);
            if (ds.Tables.Count > 0)
            {
                result = _dal.ConvertDataTableTojSonString(ds.Tables[0]);
            }
            else
            {
                result = "";
            }

            List<Product> productsList = JsonConvert.DeserializeObject<List<Product>>(result);
            return _mapper.Map<List<ProductDto>>(productsList);

        }
    }
}
