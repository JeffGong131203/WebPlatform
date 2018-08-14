using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebPlatform.Models;

namespace WebPlatform.Controllers
{
    public class DeviceController : Controller
    {
        private WebPlatformContext db = new WebPlatformContext();
        private WebPlatformDataContext dbData = new WebPlatformDataContext();

        // GET: Device
        [Authorize]
        public ActionResult Index(string devType)
        {
            IEnumerable devList;

            if (string.IsNullOrEmpty(devType))
            {
                devList = db.Device_Info.OrderBy(dev => dev.DevCode).ToList();
            }
            else
            {
                devList = db.Device_Info.Where(dev => dev.DevType == devType).OrderBy(dev => dev.DevCode).ToList();
            }

            return View(devList);
        }

        // GET: Device
        [Authorize]
        public ActionResult List(string devType)
        {
            IEnumerable devList;

            if (string.IsNullOrEmpty(devType))
            {
                devList = db.Device_Info.OrderBy(dev => dev.DevCode).ToList();
            }
            else
            {
                devList = db.Device_Info.Where(dev => dev.DevType == devType).OrderBy(dev => dev.DevCode).ToList();
            }

            return View(devList);
        }

        [Authorize]
        public ActionResult DeviceData(Guid id, string devType)
        {
            //switch (devType.ToLower())
            //{
            //    case "air":
            //        return RedirectToAction("AirDeviceData", new { devID = id });
            //    case "io":
            //        return RedirectToAction("IODeviceData", new { devID = id });
            //    case "panel":
            //        return RedirectToAction("PanelDeviceData", new { devID = id }); ;
            //    default:
            //        return RedirectToAction("List", new { devType = devType });
            //}

            ViewBag.devID = id;
            ViewBag.devType = devType;

            return View();
        }

        // GET: Device/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device_Info device_Info = db.Device_Info.Find(id);
            if (device_Info == null)
            {
                return HttpNotFound();
            }
            return View(device_Info);
        }

        // GET: Device/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Device/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,DevCode,DevName,ParentID,DevType,PropertyJson")] Device_Info device_Info)
        {
            //if (ModelState.IsValid)
            //{
            device_Info.ID = Guid.NewGuid();
            db.Device_Info.Add(device_Info);
            db.SaveChanges();
            return RedirectToAction("Index");
            //}

            //return View(device_Info);
        }

        // GET: Device/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device_Info device_Info = db.Device_Info.Find(id);
            if (device_Info == null)
            {
                return HttpNotFound();
            }
            return View(device_Info);
        }

        // POST: Device/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,DevCode,DevName,ParentID,DevType,PropertyJson")] Device_Info device_Info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device_Info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(device_Info);
        }

        // GET: Device/Delete/5
        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device_Info device_Info = db.Device_Info.Find(id);
            if (device_Info == null)
            {
                return HttpNotFound();
            }
            return View(device_Info);
        }

        // POST: Device/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Device_Info device_Info = db.Device_Info.Find(id);
            db.Device_Info.Remove(device_Info);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 设备自定义属性名
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult DevCusName(Guid? devID)
        {
            if (devID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device_Info device_Info = db.Device_Info.Find(devID);
            if (device_Info == null)
            {
                return HttpNotFound();
            }

            string viewName = string.Empty;

            //if(device_Info.DevType == "IO")
            //{
            //    viewName = "IOCusName";
            //}
            //else
            //{
            //    viewName = "DevCusName";
            //}

            return View(device_Info);
        }

        /// <summary>
        /// 设备自定义属性名称后台操作
        /// </summary>
        /// <param name="device_Info"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DevCusName(Device_Info deviceInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deviceInfo).State = EntityState.Modified;

                Dictionary<string, string> propertyJsonDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(deviceInfo.PropertyJson);
                Dictionary<string, string> propertyJsonDicNew = new Dictionary<string, string>();

                foreach (KeyValuePair<string, string> kvp in propertyJsonDic)
                {
                    if (kvp.Key.ToLower() != "addcode")
                    {
                        if (!string.IsNullOrEmpty(Request.Form[kvp.Key]))
                        {
                            propertyJsonDicNew.Add(kvp.Key, Request.Form[kvp.Key]);
                        }
                        else
                        {
                            propertyJsonDicNew.Add(kvp.Key, string.Empty);
                        }
                    }
                    else
                    {
                        propertyJsonDicNew.Add(kvp.Key, kvp.Value);
                    }
                }

                deviceInfo.PropertyJson = JsonConvert.SerializeObject(propertyJsonDicNew, Formatting.Indented);

                db.SaveChanges();
                return RedirectToAction("List", new { devType = deviceInfo.DevType });
            }

            return RedirectToAction("DevCusName", new { devID = deviceInfo.ID });
        }

        /// <summary>
        /// 开关状态读取
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult IODeviceData(Guid devID)
        {
            string sendData = GetSendData(devID);
            Dictionary<string, string> dicRet = SendData(devID, sendData);

            if (!dicRet.ContainsKey("ReciveData"))
            {
                Thread.Sleep(500);

                dicRet = SendData(devID, sendData);
            }

            ArrayList retArray = new ArrayList();
            if (dicRet.ContainsKey("ReciveData"))
            {
                retArray = ResolveIODeviceData(dicRet["ReciveData"]);
            }

            ViewBag.DevID = devID;
            ViewBag.retArray = retArray;

            return View();
        }

        /// <summary>
        /// 开关设置
        /// </summary>
        /// <param name="devID"></param>
        public ActionResult IODeviceStartOFF(Guid devID, int ioIndex, bool onOff)
        {
            Device_Info device_Info = db.Device_Info.Find(devID);
            if (device_Info == null)
            {
                return HttpNotFound();
            }

            string sendData = string.Empty;
            string addCode = JsonConvert.DeserializeObject<Dictionary<string, string>>(device_Info.PropertyJson)["addCode"];

            switch (ioIndex)
            {
                case 0:
                    if (onOff)
                    {
                        //sendData = "01 05 00 10 ff 00 8d ff";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "050010ff00");
                    }
                    else
                    {
                        //sendData = "01 05 00 10 00 00 cc 0f";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "0500100000");
                    }
                    break;
                case 1:
                    if (onOff)
                    {
                        //sendData = "01 05 00 11 ff 00 dc 3f";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "050011ff00");
                    }
                    else
                    {
                        //sendData = "01 05 00 11 00 00 9d cf";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "0500110000");
                    }
                    break;
                case 2:
                    if (onOff)
                    {
                        //sendData = "01 05 00 12 ff 00 2c 3f";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "050012ff00");
                    }
                    else
                    {
                        //sendData = "01 05 00 12 00 00 6d cf";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "0500120000");
                    }
                    break;
                case 3:
                    if (onOff)
                    {
                        //sendData = "01 05 00 13 ff 00 7d ff";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "050013ff00");
                    }
                    else
                    {
                        //sendData = "01 05 00 13 00 00 3c 0f";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "0500130000");
                    }
                    break;
                case 4:
                    if (onOff)
                    {
                        //sendData = "01 05 00 14 ff 00 cc 3e";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "050014ff00");
                    }
                    else
                    {
                        //sendData = "01 05 00 14 00 00 8d ce";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "0500140000");
                    }
                    break;
                case 5:
                    if (onOff)
                    {
                        //sendData = "01 05 00 15 ff 00 9d fe";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "050015ff00");
                    }
                    else
                    {
                        //sendData = "01 05 00 15 00 00 dc 0e";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "0500150000");
                    }
                    break;
                case 6:
                    if (onOff)
                    {
                        //sendData = "01 05 00 16 ff 00 6d fe";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "050016ff00");
                    }
                    else
                    {
                        //sendData = "01 05 00 16 00 00 2c 0e";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "0500160000");
                    }
                    break;
                case 7:
                    if (onOff)
                    {
                        //sendData = "01 05 00 17 ff 00 3c 3e";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "050017ff00");
                    }
                    else
                    {
                        //sendData = "01 05 00 17 00 00 7d ce";
                        sendData = BLL.BLLHelper.CRC_16(addCode + "0500170000");
                    }
                    break;
            }

            SendData(devID, sendData);

            return RedirectToAction("DeviceData", new { ID = devID,devType="IO" });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        private ArrayList AirDeviceDataList(Guid devID)
        {
            ArrayList airDataList = new ArrayList();

            IEnumerable devDataList = dbData.Device_Data.Where(dev => dev.DeviceID == devID).OrderByDescending(dev => dev.SendTime).Take(50).ToList();

            foreach (Device_Data ddata in devDataList)
            {
                ArrayList retArray = ResolveAirDeviceData(ddata.ReciveData);

                if (retArray.Count == 5)
                {
                    retArray.Insert(0, ddata.SendTime.ToShortTimeString());

                    airDataList.Add(retArray);
                }
            }

            return airDataList;
        }

        /// <summary>
        /// 空气传感器读取状态
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AirDeviceData(Guid devID)
        {
            string sendData = GetSendData(devID);
            Dictionary<string, string> dicRet = SendData(devID, sendData);

            if (!dicRet.ContainsKey("ReciveData"))
            {
                Thread.Sleep(500);

                dicRet = SendData(devID, sendData);
            }

            ArrayList retArray = new ArrayList();
            if (dicRet.ContainsKey("ReciveData"))
            {
                retArray = ResolveAirDeviceData(dicRet["ReciveData"]);
            }

            ArrayList dataList = AirDeviceDataList(devID);

            ViewBag.DevID = devID;

            //ArrayList retArray = new ArrayList();
            //retArray.Add(((ArrayList)dataList[0])[1]);
            //retArray.Add(((ArrayList)dataList[0])[2]);
            //retArray.Add(((ArrayList)dataList[0])[3]);
            //retArray.Add(((ArrayList)dataList[0])[4]);
            //retArray.Add(((ArrayList)dataList[0])[5]);

            ViewBag.retArray = retArray;
            ViewBag.dataList = dataList;

            return View();
        }

        private string GetSendData(Guid devID)
        {
            string sendData = string.Empty;

            IEnumerable<Device_Send> sendDataList = db.Device_Send.Where(dev => dev.DeviceID == devID).ToList();

            if (sendDataList.Count() > 0)
            {
                sendData = sendDataList.ToList()[0].SendData;
            }

            return sendData;
        }

        /// <summary>
        /// 指令发送、接收
        /// </summary>
        /// <param name="devID"></param>
        /// <param name="sendData"></param>
        /// <returns></returns>
        private Dictionary<string, string> SendData(Guid devID, string sendData)
        {
            WebComSvc.SerialPortData spd = new WebComSvc.SerialPortData();
            spd.Url = GetWebComUrl(devID);

            spd.SendData(sendData, devID.ToString());

            string retData = spd.GetReciveData();

            DateTime t1 = DateTime.Now;
            DateTime t2 = DateTime.Now;
            TimeSpan ts = t2 - t1;

            while (string.IsNullOrEmpty(retData))
            {
                retData = spd.GetReciveData();

                Thread.Sleep(200);

                t2 = DateTime.Now;
                ts = t2 - t1;
                if (ts.TotalSeconds > 5)
                {
                    break;
                }
            }

            Dictionary<string, string> dicRet = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(retData))
            {
                dicRet = JsonConvert.DeserializeObject<Dictionary<string, string>>(retData);

                t1 = DateTime.Now;
                while(string.IsNullOrEmpty(dicRet["ReciveData"].ToString()))
                {
                    retData = spd.GetReciveData();

                    Thread.Sleep(200);

                    t2 = DateTime.Now;
                    ts = t2 - t1;
                    if (ts.TotalSeconds > 5)
                    {
                        break;
                    }
                }

                if(!string.IsNullOrEmpty(retData))
                {
                    dicRet = JsonConvert.DeserializeObject<Dictionary<string, string>>(retData);
                }
            }

            return dicRet;
        }

        /// <summary>
        /// 面板读取近50条数据
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        private ArrayList PanelDeviceDataList(Guid devID)
        {
            ArrayList panelDataList = new ArrayList();

            IEnumerable devDataList = dbData.Device_Data.Where(dev => dev.DeviceID == devID).OrderByDescending(dev => dev.SendTime).Take(50).ToList();

            foreach (Device_Data ddata in devDataList)
            {
                ArrayList retArray = ResolvePanelDeviceData(ddata.ReciveData);

                if (retArray.Count > 5)
                {
                    retArray.Insert(0, ddata.SendTime.ToShortTimeString());

                    panelDataList.Add(retArray);
                }
            }

            return panelDataList;
        }

        /// <summary>
        /// 面板读取状态
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult PanelDeviceData(Guid devID)
        {
            string sendData = GetSendData(devID);
            Dictionary<string, string> dicRet = SendData(devID, sendData);

            if (!dicRet.ContainsKey("ReciveData"))
            {
                Thread.Sleep(500);

                dicRet = SendData(devID, sendData);
            }

            ArrayList retArray = new ArrayList();
            if (dicRet.ContainsKey("ReciveData"))
            {
                retArray = ResolvePanelDeviceData(dicRet["ReciveData"]);
            }

            ArrayList dataList = PanelDeviceDataList(devID);

            ViewBag.DevID = devID;

            //ArrayList retArray = new ArrayList();
            //for (int i = 1; i < ((ArrayList)dataList[0]).Count; i++)
            //{
            //    retArray.Add(((ArrayList)dataList[0])[i]);
            //}

            ViewBag.retArray = retArray;
            ViewBag.dataList = dataList;

            return View();
        }

        /// <summary>
        /// 面板数据字节解析
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ArrayList ResolvePanelDeviceData(string data)
        {
            ArrayList retArray = new ArrayList();

            string[] byteData = data.Replace("0x", "@").Split("@".ToCharArray());

            if (byteData.Length > 22)
            {
                //状态
                retArray.Add(Int32.Parse(byteData[5], System.Globalization.NumberStyles.HexNumber).ToString());

                //模式
                retArray.Add(Int32.Parse(byteData[7], System.Globalization.NumberStyles.HexNumber).ToString());

                //设置温度
                retArray.Add(Int32.Parse(byteData[8], System.Globalization.NumberStyles.HexNumber).ToString() + "." + Int32.Parse(byteData[9], System.Globalization.NumberStyles.HexNumber).ToString());

                //风机模式
                retArray.Add(Int32.Parse(byteData[11], System.Globalization.NumberStyles.HexNumber).ToString());

                //机型
                retArray.Add(Int32.Parse(byteData[13], System.Globalization.NumberStyles.HexNumber).ToString());

                //低温保护温度
                retArray.Add(Int32.Parse(byteData[16], System.Globalization.NumberStyles.HexNumber).ToString() + "." + Int32.Parse(byteData[17], System.Globalization.NumberStyles.HexNumber).ToString());

                //室内温度
                retArray.Add(Int32.Parse(byteData[18], System.Globalization.NumberStyles.HexNumber).ToString() + "." + Int32.Parse(byteData[19], System.Globalization.NumberStyles.HexNumber).ToString());

                //防冻功能
                retArray.Add(Int32.Parse(byteData[23], System.Globalization.NumberStyles.HexNumber).ToString());

                //键盘锁定
                retArray.Add(Int32.Parse(byteData[25], System.Globalization.NumberStyles.HexNumber).ToString());
            }

            return retArray;
        }

        /// <summary>
        /// 开关设备数据字节解析
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ArrayList ResolveIODeviceData(string data)
        {
            ArrayList retArray = new ArrayList();

            string[] byteData = data.Replace("0x", "@").Split("@".ToCharArray());

            if (byteData.Length == 8)
            {
                string ioState = System.Convert.ToString(System.Convert.ToInt32(byteData[6], 16), 2).PadLeft(8, '0');

                if (ioState.Length == 8)
                {
                    for (int i = 7; i >= 0; i--)
                    {
                        retArray.Add(ioState[i].ToString());
                    }
                }
            }

            return retArray;
        }

        /// <summary>
        /// 空气传感器解析数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ArrayList ResolveAirDeviceData(string data)
        {
            ArrayList retArray = new ArrayList();

            string[] byteData = data.Replace("0x", "@").Split("@".ToCharArray());

            if (byteData.Length == 15)
            {
                retArray.Add(Int32.Parse(byteData[4] + byteData[5], System.Globalization.NumberStyles.HexNumber).ToString());
                retArray.Add(Int32.Parse(byteData[6] + byteData[7], System.Globalization.NumberStyles.HexNumber).ToString());
                retArray.Add(Int32.Parse(byteData[8] + byteData[9], System.Globalization.NumberStyles.HexNumber).ToString());
                retArray.Add(Int32.Parse(byteData[10] + byteData[11], System.Globalization.NumberStyles.HexNumber).ToString());
                retArray.Add(Int32.Parse(byteData[12] + byteData[13], System.Globalization.NumberStyles.HexNumber).ToString());
            }

            return retArray;
        }

        private string GetWebComUrl(Guid devID)
        {
            string serverUrl = string.Empty;
            Device_Info dinfo = db.Device_Info.Find(devID);

            Device_Info webCom = db.Device_Info.Find(dinfo.ParentID);

            if (webCom.DevType == "COMPort")
            {
                serverUrl = webCom.PropertyJson;
            }

            return serverUrl;
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
