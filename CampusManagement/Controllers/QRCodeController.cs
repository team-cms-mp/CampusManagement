using CampusManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZXing;

namespace CampusManagement.Controllers
{
    public class QRCodeController : Controller
    {
        private ModelCMSNewContainer db = new ModelCMSNewContainer();
        // GET: QRCode
        public ActionResult Generate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Generate(QRCode qrcode)
        {
            try
            {
                int EmpID = Convert.ToInt32(Session["emp_id"]);
                qrcode.QRCodeImagePath = GenerateQRCode(qrcode.QRCodeURL);

                QRCode barcode = db.QRCodes.FirstOrDefault(bc => bc.QRCodeID == 2);
                barcode.QRCodeImagePath = qrcode.QRCodeImagePath;
                barcode.QRCodeURL = qrcode.QRCodeURL;
                barcode.CreatedBy = EmpID;
                barcode.CreatedOn = DateTime.Now;
                barcode.ModifiedBy = EmpID;
                barcode.ModifiedOn = DateTime.Now;
                db.Entry(barcode).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.Message = "QR Code Created successfully";
                ViewBag.MessageType = "success";
            }
            catch (Exception ex)
            {
                ViewBag.MessageType = "error";
                ViewBag.Message = ex.Message;
            }
            return View(qrcode);
        }

        private string GenerateQRCode(string qrcodeText)
        {
            string folderPath = "~/Content/images/QRCodeImages";
            var imagePath = string.Format(@"{0}{1}.jpg", "~/Content/images/QRCodeImages/", DateTime.Now.Ticks);
            // If the directory doesn't exist then create it.
            if (Directory.Exists(Server.MapPath(folderPath)))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath(folderPath));
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    file.Delete();
                }
            }
            else
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                    barcodeBitmap.Dispose();
                }
            }

            return imagePath;
        }

        public ActionResult Read()
        {
            return View(ReadQRCode());
        }

        private QRCode ReadQRCode()
        {
            QRCode barcode = db.QRCodes.FirstOrDefault(bc => bc.QRCodeID == 2);
            string barcodeText = "";
            string imagePath = barcode.QRCodeImagePath;
            string barcodePath = Server.MapPath(imagePath);
            var barcodeReader = new BarcodeReader();

            var result = barcodeReader.Decode(new Bitmap(barcodePath));
            if (result != null)
            {
                barcodeText = result.Text;
            }
            return new QRCode() { QRCodeURL = barcodeText, QRCodeImagePath = imagePath };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}