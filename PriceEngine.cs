using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class PriceRequest
    {
        public RiskData RiskData;
    }

    public class RiskData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Value { get; set; }
        public string Make { get; set; }
        public DateTime? DOB { get; set; }

    }

    public class QuotationSystem1
    {
        public QuotationSystem1(string url, string port)
        {

        }

        public dynamic GetPrice(dynamic request)
        {
            //makes a call to an external service - SNIP
            //var response = _someExternalService.PostHttpRequest(requestData);

            dynamic response = new ExpandoObject();
            response.Price = 123.45M;
            response.IsSuccess = true;
            response.Name = "Test Name";
            response.Tax = 123.45M * 0.12M;

            return response;
        }
    }

    public class QuotationSystem2
    {
        public QuotationSystem2(string url, string port, dynamic request)
        {

        }

        public dynamic GetPrice()
        {
            //makes a call to an external service - SNIP
            //var response = _someExternalService.PostHttpRequest(requestData);

            dynamic response = new ExpandoObject();
            response.Price = 234.56M;
            response.HasPrice = true;
            response.Name = "qewtrywrh";
            response.Tax = 234.56M * 0.12M;

            return response;
        }
    }

    public class QuotationSystem3
    {
        public QuotationSystem3(string url, string port)
        {

        }

        public dynamic GetPrice(dynamic request)
        {
            //makes a call to an external service - SNIP
            //var response = _someExternalService.PostHttpRequest(requestData);

            dynamic response = new ExpandoObject();
            response.Price = 92.67M;
            response.IsSuccess = true;
            response.Name = "zxcvbnm";
            response.Tax = 92.67M * 0.12M;

            return response;
        }
    }

    public class QuoteEngineOutput
    {
        public int QuoteEngineSystemID { get; set; }
        public decimal Price { get; set; } = -1;
        public string InsurerName { get; set; }
        public decimal Tax { get; set; }

    }
}
