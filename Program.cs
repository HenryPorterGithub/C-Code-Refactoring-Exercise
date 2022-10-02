using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            //SNIP - collect input (risk data from the user)

            var request = new PriceRequest()
            {
                RiskData = new RiskData() //hardcoded here, but would normally be from user input above
                {
                    DOB = DateTime.Parse("1980-01-01"),
                    FirstName = "John",
                    LastName = "Smith",
                    Make = "Cool New Phone",
                    Value = 500
                }
            };

            RequestValidation(request);

            QuoteEngineOutput BestPriceClass = GetPrice(request);

            Console.WriteLine(String.Format("You price is {0}, from insurer: {1}. This includes tax of {2}", BestPriceClass.Price, BestPriceClass.InsurerName, BestPriceClass.Tax));
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        //pass request with risk data with details of a gadget, return the best price retrieved from 3 external quotation engines
        public static QuoteEngineOutput GetPrice(PriceRequest request)
        {
            //now call x external systems
            List<QuoteEngineOutput> QuoteList = new List<QuoteEngineOutput>();
            List<int> QuoteSystemList = new List<int>();
            QuoteSystemListLogic(request, QuoteSystemList);

            foreach (int QuoteSystem in QuoteSystemList)
            {
                GetQuote(request, QuoteList, QuoteSystem);
            }

            //get the best price
            var BestPriceClass = QuoteList.OrderBy(EngineOutput => EngineOutput.Price).First();

            if (BestPriceClass.Price == -1)
            {
                throw new ArgumentException("Quote Failure");
                //log exception
            }

            return BestPriceClass;
        }

        public static void QuoteSystemListLogic(PriceRequest request, List<int> QuoteSystemList)
        {
            //This will eventually have to be SNIPPED when something more sophisticated gets developed
            //I am keeping it here to avoid interfering with the quote system
            if (request.RiskData.DOB != null)
            {
                QuoteSystemList.Add(1);
            }
            if (request.RiskData.Make == "examplemake1" || request.RiskData.Make == "examplemake2" || request.RiskData.Make == "examplemake3")
            {
                QuoteSystemList.Add(2);
            }
            QuoteSystemList.Add(3);
        }

        public static void GetQuote(PriceRequest request, List<QuoteEngineOutput> QuoteEngineOutputList, int SystemID)
        {
            dynamic systemRequest = new ExpandoObject();
            systemRequest.FirstName = request.RiskData.FirstName;
            systemRequest.Surname = request.RiskData.LastName;
            systemRequest.DOB = request.RiskData.DOB;
            systemRequest.Make = request.RiskData.Make;
            systemRequest.Amount = request.RiskData.Value;

            dynamic systemResponse = new System.Dynamic.ExpandoObject();
            systemResponse = QuoteSystemResponse(SystemID, systemRequest, systemResponse);

            //Normally I would expect more uniformity in responses, but we are dealing with domains here
            //Rather than interfere with the Quotation System I am preserving it as-is
            //That is why we use both IsSuccess and HasPrice on the next line, for example
            if (((IDictionary<String, object>)systemResponse).ContainsKey("HasPrice") || ((IDictionary<String, object>)systemResponse).ContainsKey("IsSuccess"))
            {
                QuoteEngineOutput QuoteEngineOutput = new QuoteEngineOutput();
                QuoteEngineOutput.QuoteEngineSystemID = SystemID;
                QuoteEngineOutput.Price = systemResponse.Price;
                QuoteEngineOutput.InsurerName = systemResponse.Name;
                QuoteEngineOutput.Tax = systemResponse.Tax;
                QuoteEngineOutputList.Add(QuoteEngineOutput);
            }

        }

        public static dynamic QuoteSystemResponse(int SystemID, dynamic systemRequest, dynamic systemResponse)
        {
            switch (SystemID)
            {
                //Normally I would save sensitive information like connection strings somewhere more secure
                case 1:
                    QuotationSystem1 system1 = new QuotationSystem1("http://quote-system-1.com", "1234");
                    systemResponse = system1.GetPrice(systemRequest);
                    return systemResponse;
                case 2:
                    QuotationSystem2 system2 = new QuotationSystem2("http://quote-system-2.com", "1235", systemRequest);
                    systemResponse = system2.GetPrice();
                    return systemResponse;
                case 3:
                    QuotationSystem3 system3 = new QuotationSystem3("http://quote-system-3.com", "100");
                    systemResponse = system3.GetPrice(systemRequest);
                    return systemResponse;
                default:
                    throw new ArgumentException("Valid SystemID must be supplied");
                    //log exception
            }
        }

        public static void RequestValidation(PriceRequest request)
        {
            //Normally I would validate the model itself using DataAnnotaton
            //I would specify fields as Required by the model
            if (request.RiskData == null)
            {
                throw new ArgumentException("Risk Data is missing");
                //log exception
            }

            if (request.RiskData.Value == 0)
            {
                throw new ArgumentException("Risk Data Value is required");
                //log exception
            }

            if (String.IsNullOrEmpty(request.RiskData.FirstName))
            {
                throw new ArgumentException("First name is required");
                //log exception
            }

            if (String.IsNullOrEmpty(request.RiskData.LastName))
            {
                throw new ArgumentException("Surname is required");
                //log exception
            }


        }
    }
}
