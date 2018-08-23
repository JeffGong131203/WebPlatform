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

        /// <summary>
        /// 影院汇总页面
        /// </summary>
        /// <param name="cusID"></param>
        /// <param name="AreaID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult CinemaDevice(Guid cusID, Guid AreaID)
        {
            //Get ALL Cinema Hall
            IEnumerable<Portal_Customer_Store> storeList = db.Portal_Customer_Store.Where(s => s.AreaID == AreaID);

            ArrayList retArray = new ArrayList();

            DateTime selDate = DateTime.Now.AddDays(-10).Date;

            IEnumerable<Cinema_SellInfo> SellList = dbData.Cinema_SellInfo.Where(c => c.StartDate > selDate).OrderBy(c => c.StartDate);

            Portal_Customer cusInfo = db.Portal_Customer.Find(cusID);
            Portal_Customer_Area areaInfo = db.Portal_Customer_Area.Find(AreaID);

            //影厅当日票房
            Dictionary<string, int> dicCurSell = new Dictionary<string, int>();
            foreach (Cinema_SellInfo sellInfo in SellList)
            {
                if (sellInfo.StartDate == DateTime.Now.Date)
                {
                    if (dicCurSell.Keys.Contains(sellInfo.HallID))
                    {
                        dicCurSell[sellInfo.HallID] += sellInfo.SellCount;
                    }
                    else
                    {
                        dicCurSell.Add(sellInfo.HallID, sellInfo.SellCount);
                    }
                }
            }

            //影院本月日期票房
            Dictionary<string, int> dicDateSell = new Dictionary<string, int>();
            foreach (Cinema_SellInfo sellInfo in SellList)
            {
                if (dicDateSell.Keys.Contains(sellInfo.StartDate.ToString()))
                {
                    dicDateSell[sellInfo.StartDate.ToString()] += sellInfo.SellCount;
                }
                else
                {
                    dicDateSell.Add(sellInfo.StartDate.ToString(), sellInfo.SellCount);
                }
            }

            //影院上月日期票房
            Dictionary<string, int> dicLastDateSell = new Dictionary<string, int>();
            Random rd = new Random();
            foreach (Cinema_SellInfo sellInfo in SellList)
            {
                if (dicLastDateSell.Keys.Contains(sellInfo.StartDate.ToString()))
                {
                    dicLastDateSell[sellInfo.StartDate.ToString()] += sellInfo.SellCount + rd.Next(-10, 10);
                }
                else
                {
                    dicLastDateSell.Add(sellInfo.StartDate.ToString(), sellInfo.SellCount + rd.Next(-10, 10));
                }
            }

            //平均数
            decimal avgPm25 = 0;
            decimal avgTmp = 0;
            decimal avgWet = 0;
            int avgCount = 0;

            int toDaySell = 0;


            foreach (Portal_Customer_Store s in storeList)
            {
                //0:hallid,1:hallname,2:sellcount,3:pm25,4:tmp,5:wet,6:pValue,7:pRate
                ArrayList dataArray = new ArrayList();

                dataArray.Add(s.ID);
                dataArray.Add(s.StoreCode);
                dataArray.Add(s.StoreName);
                dataArray.Add(dicCurSell[s.StoreCode].ToString());

                toDaySell += dicCurSell[s.StoreCode];

                Guid?[] devIDArray = db.Device_Customer_Store.Where(d => d.StoreID == s.ID).Select(d => d.DeviceID).ToArray();
                IEnumerable<Device_Info> devList = db.Device_Info.Where(d => devIDArray.Contains(d.ID)).OrderBy(d => d.DevType).OrderBy(d => d.DevCode);

                foreach (Device_Info d in devList)
                {
                    if (d.DevType.ToLower() == "air")
                    {
                        Dictionary<string, string> dicRet = GetDeviceData(d.ID);

                        ArrayList airArray = new ArrayList();
                        if (dicRet.ContainsKey("ReciveData"))
                        {
                            airArray = ResolveAirDeviceData(dicRet["ReciveData"]);
                        }

                        if (airArray.Count == 5)
                        {
                            //pm25 = retArray[0].ToString();
                            //tmp = (Convert.ToDecimal(retArray[1]) / 10).ToString();
                            //wet = retArray[2].ToString();
                            //co2 = retArray[3].ToString();
                            //tvoc = retArray[4].ToString();

                            if (Convert.ToDecimal(airArray[0]) + Convert.ToDecimal(airArray[1]) + Convert.ToDecimal(airArray[2]) != 0)
                            {
                                dataArray.Add(airArray[0].ToString());
                                dataArray.Add((Convert.ToDecimal(airArray[1]) / 10).ToString());
                                dataArray.Add(airArray[2].ToString());

                                avgPm25 += Convert.ToDecimal(airArray[0].ToString());
                                avgTmp += Convert.ToDecimal(airArray[1]) / 10;
                                avgWet += Convert.ToDecimal(airArray[2].ToString());

                                avgCount++;
                            }


                            //retArray.Add(airArray[3].ToString());
                            //retArray.Add(airArray[4].ToString());
                        }
                    }

                    //
                    if (d.DevType.ToLower() == "power")
                    {
                        dataArray.Add("");
                        dataArray.Add("");
                    }

                }

                //缺设备补足
                if (dataArray.Count < 9)
                {
                    for (int i = dataArray.Count; i < 9; i++)
                    {
                        dataArray.Add("0");
                    }
                }

                retArray.Add(dataArray);

            }

            //影厅当前数据
            ViewBag.retArray = retArray;
            //影院当月日期票房
            ViewBag.dicDateSell = dicDateSell;
            //影院上月日期票房
            ViewBag.dicLastDateSell = dicLastDateSell;
            //影院当月总电量
            ViewBag.totalP = "120";
            //影院上月总电量
            ViewBag.lastTotalP = "100";

            //影厅名称
            ViewBag.cinemaName = areaInfo.AreaName;

            //avgPm25
            ViewBag.avgPm25 = (avgPm25 / avgCount).ToString("#0.00");
            //avgTmp
            ViewBag.avgTmp = (avgTmp / avgCount).ToString("#0.00");
            //avgWet
            ViewBag.avgWet = (avgWet / avgCount).ToString("#0.00");

            //当日总票房
            ViewBag.toDaySell = toDaySell;

            ViewBag.cusID = cusID;
            ViewBag.areaID = AreaID;


            return View();
        }

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

        private Dictionary<string, string> GetDeviceData(Guid devID)
        {
            string sendData = GetSendData(devID);
            Dictionary<string, string> dicRet = SendData(devID, sendData);

            if (!dicRet.ContainsKey("ReciveData"))
            {
                Thread.Sleep(500);

                dicRet = SendData(devID, sendData);
            }

            return dicRet;
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

            return RedirectToAction("DeviceData", new { ID = devID, devType = "IO" });
        }

        private IEnumerable GetDeviceDataList(Guid devID)
        {
            IEnumerable devDataList = dbData.Device_Data.Where(dev => dev.DeviceID == devID && dev.SendData.Substring(0, 2).ToUpper().Trim() == dev.ReciveData.Substring(2, 2).ToUpper().Trim()).OrderByDescending(dev => dev.SendTime).Take(50).ToList();

            return devDataList;
        }

        private IEnumerable GetDeviceDataList(Guid devID, int rowCount)
        {
            IEnumerable devDataList = dbData.Device_Data.Where(dev => dev.DeviceID == devID && dev.SendData.Substring(0, 2).ToUpper().Trim() == dev.ReciveData.Substring(2, 2).ToUpper().Trim()).OrderByDescending(dev => dev.SendTime).Take(rowCount).ToList();

            return devDataList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        private ArrayList AirDeviceDataList(Guid devID)
        {
            ArrayList airDataList = new ArrayList();

            IEnumerable devDataList = GetDeviceDataList(devID);

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

            if(!spd.PortStatus())
            {
                spd.OpenPort();
            }

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
                while (string.IsNullOrEmpty(dicRet["ReciveData"].ToString()))
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

                if (!string.IsNullOrEmpty(retData))
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

            IEnumerable devDataList = GetDeviceDataList(devID);

            foreach (Device_Data ddata in devDataList)
            {
                ArrayList retArray = ResolvePanelDeviceData(ddata.ReciveData);

                if (retArray.Count == 9)
                {
                    retArray.Insert(0, ddata.SendTime.ToShortTimeString());

                    panelDataList.Add(retArray);
                }
            }

            return panelDataList;
        }

        /// <summary>
        /// 面板控制
        /// </summary>
        /// <param name="devID"></param>
        /// <param name="setCode"></param>
        /// <param name="setValue"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult PanelDeviceSet(Guid devID)
        {
            Device_Info device_Info = db.Device_Info.Find(devID);
            if (device_Info == null)
            {
                return HttpNotFound();
            }

            string statusCode = string.Empty;
            string modeCode = string.Empty;
            string fanModeCode = string.Empty;
            string setTmp = string.Empty;

            if (Request.Form["SEL_Status"] != null)
            {
                statusCode = Request.Form["SEL_Status"];
            }

            if (Request.Form["SEL_Mode"] != null)
            {
                modeCode = Request.Form["SEL_Mode"];
            }

            if (Request.Form["SEL_FanMode"] != null)
            {
                fanModeCode = Request.Form["SEL_FanMode"];
            }

            if (Request.Form["SEL_SetTmp"] != null)
            {
                setTmp = Request.Form["SEL_SetTmp"];
            }

            //string sendData = string.Empty;
            //string addCode = JsonConvert.DeserializeObject<Dictionary<string, string>>(device_Info.PropertyJson)["addCode"];

            //sendData = BLL.BLLHelper.CRC_16(addCode + "03" + setCode + setValue);

            //SendData(devID, sendData);

            return RedirectToAction("DeviceData", new { ID = devID, devType = "Panel" });
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
