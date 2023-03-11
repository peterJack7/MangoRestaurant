using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IDALRepository _dalRepository;
        private IProductRepository _productRepository;

        public ProductAPIController(IProductRepository productRepository, IDALRepository dALRepository)
        {
            _productRepository = productRepository;
            _dalRepository= dALRepository;  
            this._response=new ResponseDto();
        }

        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = await _productRepository.GetProducts();
                _response.Result= productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess=false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }
            return _response;
        }

        //[HttpGet]
        //public  string GetSqlProduct()
        //{
        //    string response = "";
        //    try
        //    {
              
        //        //SqlParameter[] parameters = {
        //        //    new SqlParameter("@user_type_id",SqlDbType.VarChar){Value=""}

        //        //};
        //        DataSet productDtos =  _dalRepository.getDataSetForSqlParam("SpGetProducts", null);

        //        response = _dalRepository.ConvertDataTableTojSonString(productDtos.Tables[0]);
        //    }
        //    catch (Exception ex)
        //    {
        //        //_response.IsSuccess = false;
        //        //_response.ErrorMessages = new List<string>() { ex.Message.ToString() };
        //    }
        //    return response;
        //}

        [HttpGet]
        [Route("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                ProductDto productDtos = await _productRepository.GetProductById(id);
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }
            return _response;
        }

        [HttpPost]
        public async Task<object> Post([FromBody] ProductDto productDto)
        {
            try
            {
               ProductDto productDtos = await _productRepository.CreateUpdateProduct(productDto);
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }
            return _response;
        }

        [HttpPut]
        public async Task<object> Put([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto productDtos = await _productRepository.CreateUpdateProduct(productDto);
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }
            return _response;
        }

        [HttpDelete]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool IsSuccess = await _productRepository.DeletProduct(id);
                _response.Result = IsSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.Message.ToString() };
            }
            return _response;
        }
    }
}
