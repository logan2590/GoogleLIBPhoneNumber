using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GoogleLIBPhoneNumber.Models;
using PhoneNumbers;

namespace GoogleLIBPhoneNumber.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(ValidatePhoneNumberModel validatePhoneNumberModel)
        {
            if (validatePhoneNumberModel.OriginalNumber != null)
                validatePhoneNumberModel = CallGoogleLib(validatePhoneNumberModel);

            return View(validatePhoneNumberModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        private ValidatePhoneNumberModel CallGoogleLib(ValidatePhoneNumberModel validatePhoneNumberModel)
        {
            var data = new ValidatePhoneNumberModel();
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                //string telephoneNumber = "412 650 7354";
                //string countryCode = "US";
                PhoneNumbers.PhoneNumber phoneNumber = phoneUtil.Parse(validatePhoneNumberModel.OriginalNumber, validatePhoneNumberModel.Region);

                bool isMobile = false;
                bool isValidNumber = phoneUtil.IsValidNumber(phoneNumber); // returns true for valid number    

                // returns trueor false w.r.t phone number region  
                bool isValidRegion = phoneUtil.IsValidNumberForRegion(phoneNumber, validatePhoneNumberModel.Region);

                string region = phoneUtil.GetRegionCodeForNumber(phoneNumber); // GB, US , PK    

                var numberType = phoneUtil.GetNumberType(phoneNumber); // Produces Mobile , FIXED_LINE    

                string phoneNumberType = numberType.ToString();

                if (!string.IsNullOrEmpty(phoneNumberType) && phoneNumberType == "MOBILE")
                {
                    isMobile = true;
                }

                var originalNumber = phoneUtil.Format(phoneNumber, PhoneNumberFormat.E164); // Produces "+923336323997"    

                //Console.WriteLine("telephoneNumber-----" + telephoneNumber);
                //Console.WriteLine("originalNumber------" + originalNumber);
                //Console.WriteLine("isMobile------------" + isMobile);
                //Console.WriteLine("isValidNumber-------" + isValidNumber);
                //Console.WriteLine("isValidRegion-------" + isValidRegion);
                //Console.WriteLine("region--------------" + region);

                data.OriginalNumber = validatePhoneNumberModel.OriginalNumber;
                data.FormattedNumber = originalNumber;
                data.IsMobile = isMobile;
                data.IsValidNumber = isValidNumber;
                data.IsValidNumberForRegion = isValidRegion;
                data.Region = region;

                return data;

                //returnResult = new GenericResponse<ValidatePhoneNumberModel>()
                //{
                //    Data = data,
                //    StatusCode = HttpStatusCode.OK,
                //    StatusMessage = "Success"
                //};

            }
            catch (NumberParseException ex)
            {

                String errorMessage = "NumberParseException was thrown: " + ex.Message.ToString();


                //returnResult = new GenericResponse<ValidatePhoneNumberModel>()
                //{
                //    Message = errorMessage,
                //    StatusCode = HttpStatusCode.BadRequest,
                //    StatusMessage = "Failed"
                //};


            }
            return data;
        }
    }
}
