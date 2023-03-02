using CaseStudy.Dtos.CropDto;
using CaseStudy.Models;
using CaseStudy.Repository;
using CaseStudy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Data;
using System.Net;
//using System.Net.Mail;
using MailKit.Net.Smtp;

namespace CaseStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CropController : ControllerBase
    {
        private readonly CropService _service;
        ExceptionRepository _exception;
        public CropController(CropService service, ExceptionRepository exception)
        {
            _service = service;
            _exception = exception;
        }

        [HttpPost("addCrop")]
        [Authorize(Roles = "Farmer")]
        public async Task<ActionResult<CropDetail>> AddNewCrop(AddCropDto crop)
        {

            var res = await _service.AddCropAsync(crop);
            if (res == null)
            {
                return BadRequest("Error while adding crop details");
            }
            return Ok(res);

        }


        [HttpGet("getCrops")]
        public async Task<ActionResult<IEnumerable<CropDetail>>> GetAllCrops()
        {
            var res = await _service.GetAllCropAsync();
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }


        [HttpGet("getCrops/{id}")]
        public async Task<ActionResult<CropDetail>> GetCropById(int id)
        {
            var res = await _service.GetCropByIdAsync(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
        [HttpPut("editCrop/{cid}")]
        public async Task<ActionResult<CropDetail>> UpdateCrop(UpdateCropDto crop, int cid)
        {

            var res = await _service.EditCropAsync(cid, crop);
            if (res == null)
            {
                return BadRequest("Error while updating crop details");
            }
            return Ok(res);
        }

        [HttpGet("viewCrop/{id}")]
        public async Task<ActionResult<CropDetail>> viewCropById(int id)
        {
            var res = await _service.ViewCropByIdAsync(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
        [HttpPut("cropimg/{cid}")]
        public async Task CropImage(int cid)
        {
            var filepath = "C:\\cropdeal main\\New folder\\CropDeal-main\\CropDeal-main\\CropDealUI\\src\\assets\\images";
            var files = Request.Form.Files;
            foreach (IFormFile source in files)
            {

                string FileName = source.FileName;
                string filePath = filepath + cid.ToString();
                if (!System.IO.Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string imagePath = filePath + "\\image.jpg";
                using (FileStream stream = System.IO.File.Create(imagePath))
                {
                    await source.CopyToAsync(stream);
                }
                await _service.CropImage(@"https://localhost:44346/uploads/crops/" + cid + "/image.jpg", cid);
            }
        }
        #region testing email
        //        [HttpPost]
        //        public async void SendNoti(string email)
        //        {
        //            try
        //            {

        //                using (MailMessage message = new MailMessage("hansrajsharma87896@gmail.com", "hansrajsharma87896@gmail.com"))
        //                {
        //                    message.Body = "hii"; //+
        //                    //    "------------------------------------------------------------------------------------\n" +
        //                    //    $"Crop Name: {crop.CropName}\n" +
        //                    //    $"Crop Type: " + (crop.CropTypeId == 1 ? "Fruit\n" : crop.CropTypeId == 2 ? "Vegatable\n" : "Grain\n") +
        //                    //    $"Crop Qty: {crop.QtyAvailable}\n" +
        //                    //    $"Crop ExpectedPrice: {crop.ExpectedPrice}\n" +
        //                    //    $"\n Link For the new Crop Added:   " +
        //                    //    "http://localhost:4200/crop-detail/" + crop.CropId +
        //                    //    "\n------------------------------------------------------------------------------------";

        //                    message.Subject = "NEW CROP ADDED";
        //                    message.IsBodyHtml = false;

        //                    using (SmtpClient smtp = new SmtpClient())
        //                    {
        //                        smtp.Host = "smtp.gmail.com";
        //                        smtp.EnableSsl = true;
        //                        NetworkCredential cred = new NetworkCredential("hansrajsharma87896@gmail.com", "smmnlcdcjpnfafzc");
        //                        smtp.UseDefaultCredentials = false;
        //                        smtp.Credentials = cred;
        //                        smtp.Port = 587;
        //                        smtp.Send(message);
        //                    }
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                await _exception.AddException(e, "Send Noti in CropRepo");
        //            }
        //        }
        //    }
        //}
        //[HttpPost("EmailSendings")]
        //public IActionResult EmailSending(string email)
        //{

        //    string textBody = " <p>hello</p>";




        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("hansraj", "hansrajsharma87896@gmail.com"));
        //    message.To.Add(new MailboxAddress("hello testing", email));
        //    message.Subject = "crop added succesfully";
        //    message.Body = new TextPart("html")
        //    {
        //        Text = textBody
        //    };
        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect("smtp.gmail.com", 587, false);
        //        client.Authenticate("hansrajsharma87896@gmail.com", "smmnlcdcjpnfafzc");
        //        client.Send(message);
        //        client.Disconnect(true);
        //    }

        //    return Ok("Email sent Successfully");
        #endregion
    }
}
