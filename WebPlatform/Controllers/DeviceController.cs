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
            ArrayList retArrayPower = new ArrayList();

            DateTime selDate = DateTime.Now.AddDays(1- DateTime.Now.Day).Date;
            DateTime LastselDate = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).Date;

            IEnumerable<Cinema_SellInfo> SellList = dbData.Cinema_SellInfo.Where(c => c.StartDate >= selDate).OrderBy(c => c.StartDate);
            IEnumerable<Cinema_SellInfo> LastSellList = dbData.Cinema_SellInfo.Where(c => c.StartDate >= LastselDate && c.StartDate < selDate).OrderBy(c => c.StartDate);

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
                if (dicDateSell.Keys.Contains(sellInfo.StartDate.Day.ToString()))
                {
                    dicDateSell[sellInfo.StartDate.Day.ToString()] += sellInfo.SellCount;
                }
                else
                {
                    dicDateSell.Add(sellInfo.StartDate.Day.ToString(), sellInfo.SellCount);
                }
            }

            //影院上月日期票房
            Dictionary<string, int> dicLastDateSell = new Dictionary<string, int>();

            foreach (Cinema_SellInfo sellInfo in LastSellList)
            {
                if (dicLastDateSell.Keys.Contains(sellInfo.StartDate.Day.ToString()))
                {
                    dicLastDateSell[sellInfo.StartDate.Day.ToString()] += sellInfo.SellCount ;
                }
                else
                {
                    dicLastDateSell.Add(sellInfo.StartDate.Day.ToString(), sellInfo.SellCount);
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

                    //用电量
                    if (d.DevType.ToLower() == "pow")
                    {
                        //电流配比
                        decimal ct = 30;
                        IEnumerable devCT = GetDeviceDataList(d.ID, 1, "00 8E 00 02");
                        foreach (Device_Data dataCt in devCT)
                        {
                            ct = decimal.Parse(ResolvePowDeviceData(dataCt.ReciveData, 4, 2));
                        }

                        //电量
                        IEnumerable<Device_Data> devDataList = GetDeviceDataList(d.ID, 1000000, "00 00 00 04").Cast<Device_Data>();
                        devDataList = devDataList.OrderBy(dev => dev.SendTime);

                        ArrayList retArrayPowerDate = new ArrayList();
                        ArrayList retArrayPowerValue = new ArrayList();

                        DateTime lastMaxTime = new DateTime(1900, 1, 1);
                        DateTime curSendTime = new DateTime(1900, 1, 1);
                        decimal lastMaxValue = 0;
                        decimal curValue = 0;

                        foreach (Device_Data ddata in devDataList)
                        {
                            //第一个
                            if (curSendTime.Year == 1900)
                            {
                                curSendTime = ddata.SendTime;
                                curValue = decimal.Parse(ResolvePowDeviceData(ddata.ReciveData, 4, 4)) / 100 * ct;
                            }

                            //同一时间
                            if (curSendTime.ToString("yyyyMM") == ddata.SendTime.ToString("yyyyMM"))
                            {
                                curSendTime = ddata.SendTime;
                                curValue = decimal.Parse(ResolvePowDeviceData(ddata.ReciveData, 4, 4)) / 100 * ct;
                            }
                            else
                            {
                                //第一个计算差额
                                if (lastMaxTime.Year == 1900)
                                {
                                    lastMaxTime = curSendTime;
                                    lastMaxValue = curValue;

                                    curSendTime = ddata.SendTime;
                                    curValue = decimal.Parse(ResolvePowDeviceData(ddata.ReciveData, 4, 4)) / 100 * ct;
                                }
                                else
                                {

                                    //入队列
                                    retArrayPowerDate.Add(lastMaxTime.ToString("yyyy-MM"));
                                    retArrayPowerValue.Add(curValue - lastMaxValue);

                                    //更新值
                                    lastMaxTime = curSendTime;
                                    lastMaxValue = curValue;

                                    curSendTime = ddata.SendTime;
                                    curValue = decimal.Parse(ResolvePowDeviceData(ddata.ReciveData, 4, 4)) / 100 * ct;
                                }
                            }
                        }

                        retArrayPower.Add(retArrayPowerDate);
                        retArrayPower.Add(retArrayPowerValue);
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

            ArrayList powerDateArray = new ArrayList();
            ArrayList powerValueArray = new ArrayList();

            if (retArrayPower.Count > 0)
            {
                if (retArrayPower.Count > 2)
                {
                    powerDateArray = (ArrayList)retArrayPower[0];

                    for (int i = 1; i <= retArrayPower.Count - 1; i = i + 2)
                    {
                        ArrayList pValue = (ArrayList)retArrayPower[i];

                        for (int j = 0; j < pValue.Count; j++)
                        {
                            if (powerValueArray.Count > j)
                            {
                                powerValueArray[j] = (decimal)powerValueArray[j] + (decimal)pValue[j];
                            }
                            else
                            {
                                powerValueArray.Add(pValue[j]);
                            }
                        }
                    }
                }
                else
                {
                    powerDateArray = (ArrayList)retArrayPower[0];
                    powerValueArray = (ArrayList)retArrayPower[1];
                }
            }

            //电表数据
            ViewBag.powerDateArray = powerDateArray;
            ViewBag.powerValueArray = powerValueArray;

            //影厅当前数据
            ViewBag.retArray = retArray;
            //影院当月日期票房
            ViewBag.dicDateSell = dicDateSell;
            //影院上月日期票房
            ViewBag.dicLastDateSell = dicLastDateSell;

            if (powerValueArray.Count > 1)
            {
                //影院当月总电量
                ViewBag.totalP = powerValueArray[powerValueArray.Count - 1];
                //影院上月总电量
                ViewBag.lastTotalP = powerValueArray[powerValueArray.Count - 2];
            }
            else
            {
                //影院当月总电量
                ViewBag.totalP = "0";
                //影院上月总电量
                ViewBag.lastTotalP = "0";
            }

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

        /// <summary>
        /// 设备指令交互(单个指令)
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
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
        /// 获取电表电量、功率最近50条信息
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        private ArrayList PowDeviceDataList(Guid devID)
        {
            ArrayList retArrayList = new ArrayList();

            //电流配比
            decimal ct = 30;
            IEnumerable devCT = GetDeviceDataList(devID, 1, "00 8E 00 02");
            foreach (Device_Data dataCt in devCT)
            {
                ct = decimal.Parse(ResolvePowDeviceData(dataCt.ReciveData, 4, 2));
            }

            //电量
            IEnumerable<Device_Data> devDataList = GetDeviceDataList(devID, 650, "00 00 00 04").Cast<Device_Data>();
            devDataList = devDataList.OrderBy(d => d.SendTime);

            ArrayList retArrayPower = new ArrayList();
            ArrayList retArrayPowerDate = new ArrayList();
            ArrayList retArrayPowerValue = new ArrayList();

            DateTime lastMaxTime = new DateTime(1900, 1, 1);
            DateTime curSendTime = new DateTime(1900, 1, 1);
            decimal lastMaxValue = 0;
            decimal curValue = 0;

            foreach (Device_Data ddata in devDataList)
            {
                //第一个
                if (curSendTime.Year == 1900)
                {
                    curSendTime = ddata.SendTime;
                    curValue = decimal.Parse(ResolvePowDeviceData(ddata.ReciveData, 4, 4)) / 100 * ct;
                }

                //同一时间
                if (curSendTime.ToString("yyyyMMddhh") == ddata.SendTime.ToString("yyyyMMddhh"))
                {
                    curSendTime = ddata.SendTime;
                    curValue = decimal.Parse(ResolvePowDeviceData(ddata.ReciveData, 4, 4)) / 100 * ct;
                }
                else
                {
                    //第一个计算差额
                    if (lastMaxTime.Year == 1900)
                    {
                        lastMaxTime = curSendTime;
                        lastMaxValue = curValue;

                        curSendTime = ddata.SendTime;
                        curValue = decimal.Parse(ResolvePowDeviceData(ddata.ReciveData, 4, 4)) / 100 * ct;
                    }
                    else
                    {

                        //入队列
                        ArrayList retArray = new ArrayList();

                        retArrayPowerDate.Add(lastMaxTime.ToString("yyyy-MM-dd hh"));
                        retArrayPowerValue.Add(curValue - lastMaxValue);

                        //更新值
                        lastMaxTime = curSendTime;
                        lastMaxValue = curValue;

                        curSendTime = ddata.SendTime;
                        curValue = decimal.Parse(ResolvePowDeviceData(ddata.ReciveData, 4, 4)) / 100 * ct;
                    }
                }
            }

            if (retArrayPowerDate.Count > 50 && retArrayPowerValue.Count > 50)
            {
                retArrayPowerDate.RemoveRange(50, retArrayPowerDate.Count - 50);
                retArrayPowerValue.RemoveRange(50, retArrayPowerValue.Count - 50);
            }

            retArrayPower.Add(retArrayPowerDate);
            retArrayPower.Add(retArrayPowerValue);


            //功率
            IEnumerable devDataListR = GetDeviceDataList(devID, 50, "00 6A 00 02");
            ArrayList retArrayRate = new ArrayList();
            ArrayList retArrayDate = new ArrayList();
            ArrayList retArrayValue = new ArrayList();

            foreach (Device_Data ddata in devDataListR)
            {
                retArrayDate.Add(ddata.SendTime.ToString("yyyy-MM-dd hh"));
                retArrayValue.Add(decimal.Parse(ResolvePowDeviceData(ddata.ReciveData, 4, 4)) / 100 * ct);
            }

            retArrayDate.Reverse();
            retArrayValue.Reverse();

            retArrayRate.Add(retArrayDate);
            retArrayRate.Add(retArrayValue);

            retArrayList.Add(retArrayPower);
            retArrayList.Add(retArrayRate);

            return retArrayList;
        }

        public ActionResult Test()
        {
            Guid testID = new Guid("B52ABFF9-8F3D-47A4-97F9-0478F48B1859");

            ArrayList testArr = PowDeviceDataList(testID);

            return Content("ok");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult PowDeviceData(Guid devID)
        {

            IEnumerable<Device_Send> sendDataList = GetSenDataList(devID);
            ArrayList retArray = new ArrayList();

            try
            {
                foreach (Device_Send send in sendDataList)
                {
                    Dictionary<string, string> dicRet = SendData(devID, send.SendData);

                    if (!dicRet.ContainsKey("ReciveData"))
                    {
                        Thread.Sleep(500);

                        dicRet = SendData(devID, send.SendData);
                    }

                    if (send.SendData.Trim().Contains("00 00 00 04"))//电量
                    {
                        retArray.Insert(0, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 4)) / 100);
                    }

                    if (send.SendData.Trim().Contains("00 61 00 02"))//A电压
                    {
                        retArray.Insert(1, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 10);
                    }

                    if (send.SendData.Trim().Contains("00 62 00 02"))//B电压
                    {
                        retArray.Insert(2, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 10);
                    }

                    if (send.SendData.Trim().Contains("00 63 00 02"))//C电压
                    {
                        retArray.Insert(3, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 10);
                    }

                    if (send.SendData.Trim().Contains("00 64 00 02"))//A电流
                    {
                        retArray.Insert(4, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 100);
                    }

                    if (send.SendData.Trim().Contains("00 65 00 02"))//B电流
                    {
                        retArray.Insert(5, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 100);
                    }

                    if (send.SendData.Trim().Contains("00 66 00 02"))//C电流
                    {
                        retArray.Insert(6, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 100);
                    }

                    if (send.SendData.Trim().Contains("00 67 00 02"))//A功率
                    {
                        retArray.Insert(7, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 1000);
                    }

                    if (send.SendData.Trim().Contains("00 68 00 02"))//B功率
                    {
                        retArray.Insert(8, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 1000);
                    }

                    if (send.SendData.Trim().Contains("00 69 00 02"))//C功率
                    {
                        retArray.Insert(9, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 1000);
                    }

                    if (send.SendData.Trim().Contains("00 6A 00 02"))//总功率
                    {
                        retArray.Insert(10, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)) / 1000);
                    }

                    if (send.SendData.Trim().Contains("00 8E 00 02"))//电流配比CT
                    {
                        retArray.Insert(11, decimal.Parse(ResolvePowDeviceData(dicRet["ReciveData"], 4, 2)));
                    }
                }

                retArray[0] = decimal.Parse(retArray[0].ToString()) * decimal.Parse(retArray[11].ToString());
                retArray[4] = decimal.Parse(retArray[4].ToString()) * decimal.Parse(retArray[11].ToString());
                retArray[5] = decimal.Parse(retArray[5].ToString()) * decimal.Parse(retArray[11].ToString());
                retArray[6] = decimal.Parse(retArray[6].ToString()) * decimal.Parse(retArray[11].ToString());
                retArray[7] = decimal.Parse(retArray[7].ToString()) * decimal.Parse(retArray[11].ToString());
                retArray[8] = decimal.Parse(retArray[8].ToString()) * decimal.Parse(retArray[11].ToString());
                retArray[9] = decimal.Parse(retArray[9].ToString()) * decimal.Parse(retArray[11].ToString());
                retArray[10] = decimal.Parse(retArray[10].ToString()) * decimal.Parse(retArray[11].ToString());
            }
            catch { }

            //图形数据List
            ArrayList retArrayList = PowDeviceDataList(devID);

            ArrayList retArrayPowerValue = (ArrayList)retArrayList[0];
            ArrayList retArrayPowerRate = (ArrayList)retArrayList[1];

            ViewBag.DevID = devID;
            ViewBag.retArray = retArray;
            ViewBag.retArrayPowerValue = retArrayPowerValue;
            ViewBag.retArrayPowerRate = retArrayPowerRate;

            return View();
        }

        /// <summary>
        /// 电表数据解析
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private string ResolvePowDeviceData(string data, int start, int num)
        {
            string retStr = string.Empty;

            string[] byteData = data.Replace("0x", "@").Split("@".ToCharArray());

            if (byteData.Length > 8)
            {
                string strData = string.Empty;
                for (int i = start; i < start + num; i++)
                {
                    strData += byteData[i];
                }

                retStr = Int32.Parse(strData, System.Globalization.NumberStyles.HexNumber).ToString();
            }

            return retStr;
        }

        /// <summary>
        /// Vrv空调数据设置
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult VrvDeviceSet(Guid devID)
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

                if (statusCode == "00")
                {
                    statusCode = "60";
                }

                if (statusCode == "01")
                {
                    statusCode = "61";
                }
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
                setTmp = int.Parse(Request.Form["SEL_SetTmp"]).ToString("X2");
            }

            //01 10 07 D9 00 03 06 10 60 00 02 00 A0 0B 52
            string sendDataTpl = "{0}06{1}{2}";
            string sendData = string.Empty;
            string[] addCode = JsonConvert.DeserializeObject<Dictionary<string, string>>(device_Info.PropertyJson)["addCode"].Split("-".ToCharArray());

            ArrayList sendArray = new ArrayList();

            sendArray.Add(string.Format(sendDataTpl, addCode[0], addCode[1], fanModeCode + statusCode));
            sendArray.Add(string.Format(sendDataTpl, addCode[0], addCode[1], "00" + modeCode));
            sendArray.Add(string.Format(sendDataTpl, addCode[0], addCode[1], "00" + setTmp));


            //sendData = string.Format(sendDataTpl,addCode[0],addCode[1],fanModeCode+statusCode,modeCode,setTmp);

            foreach (string s in sendArray)
            {
                sendData = BLL.BLLHelper.CRC_16(s);

                SendData(devID, sendData);

                Thread.Sleep(500);
            }

            return RedirectToAction("DeviceData", new { ID = devID, devType = "Vrv" });
        }

        /// <summary>
        /// Vrv空调数据读取
        /// </summary>
        /// <param name="devID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult VrvDeviceData(Guid devID)
        {
            Dictionary<string, string> dicRet = GetDeviceData(devID);

            ArrayList retArray = new ArrayList();
            if (dicRet.ContainsKey("ReciveData"))
            {
                retArray = ResolveVrvDeviceData(dicRet["ReciveData"]);
            }

            ViewBag.DevID = devID;
            ViewBag.retArray = retArray;

            return View();
        }


        /// <summary>
        /// vrv空调读取数据解析
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ArrayList ResolveVrvDeviceData(string data)
        {
            ArrayList retArray = new ArrayList();

            string[] byteData = data.Replace("0x", "@").Split("@".ToCharArray());

            if (byteData.Length > 15)
            {
                //FanMode
                retArray.Add(byteData[4]);
                //Status
                retArray.Add(byteData[5]);
                //Mode
                retArray.Add(byteData[7]);
                //SetTmp
                retArray.Add(((Int32.Parse(byteData[8], System.Globalization.NumberStyles.HexNumber) * 256 + int.Parse(byteData[9], System.Globalization.NumberStyles.HexNumber)) / 10).ToString());
                //Tmp
                retArray.Add(((Int32.Parse(byteData[12], System.Globalization.NumberStyles.HexNumber) * 256 + int.Parse(byteData[13], System.Globalization.NumberStyles.HexNumber)) / 10).ToString());
            }

            return retArray;
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

        private IEnumerable GetDeviceDataList(Guid devID, int rowCount, string dataType)
        {
            IEnumerable devDataList = dbData.Device_Data.Where(dev => dev.DeviceID == devID && dev.SendData.Substring(0, 2).ToUpper().Trim() == dev.ReciveData.Substring(2, 2).ToUpper().Trim() && dev.SendData.Contains(dataType)).OrderByDescending(dev => dev.SendTime).Take(rowCount).ToList();

            return devDataList;
        }

        private IEnumerable GetDeviceDataList(Guid devID, int rowCount, int year, string dataType)
        {
            IEnumerable devDataList = dbData.Device_Data.Where(dev => dev.DeviceID == devID && dev.SendData.Substring(0, 2).ToUpper().Trim() == dev.ReciveData.Substring(2, 2).ToUpper().Trim() && dev.SendData.Contains(dataType) && dev.SendTime.Year == year).OrderByDescending(dev => dev.SendTime).Take(rowCount).ToList();

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

        private IEnumerable<Device_Send> GetSenDataList(Guid devID)
        {
            string sendData = string.Empty;

            IEnumerable<Device_Send> sendDataList = db.Device_Send.Where(dev => dev.DeviceID == devID).OrderBy(dev => dev.SendData).ToList();

            return sendDataList;
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

            if (!spd.PortStatus())
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

            /*
             *     string[] statusName = new string[] { "关", "开", "防冻启动" };
             *     string[] modeName = new string[] { "", "制冷", "制热", "通风" };
             *     string[] fanModeName = new string[] { "自动", "高速", "中速", "低速" };
             */

            ArrayList sendArray = new ArrayList();

            /*
             * 06 00 02 
             * 00：关、01：开
            */
            if (Request.Form["SEL_Status"] != null)
            {
                statusCode = Request.Form["SEL_Status"];

                if (string.IsNullOrEmpty(statusCode))
                {
                    sendArray.Add("06000200" + statusCode);
                }
            }

            /*
             * 06 00 03 
             * 1：制冷、2：制热、3：通风；
            */
            if (Request.Form["SEL_Mode"] != null)
            {
                modeCode = Request.Form["SEL_Mode"];

                if (string.IsNullOrEmpty(modeCode))
                {
                    sendArray.Add("06000300" + modeCode);
                }

            }

            /*
             * 06 00 05 
             * 01：高速、02：中速、03：低速、00：自动；
            */
            if (Request.Form["SEL_FanMode"] != null)
            {
                fanModeCode = Request.Form["SEL_FanMode"];

                if (string.IsNullOrEmpty(fanModeCode))
                {
                    sendArray.Add("06000500" + fanModeCode);
                }

            }

            /*
             * 06 00 04 
             * 5-35；
            */
            if (Request.Form["SEL_SetTmp"] != null)
            {
                setTmp = int.Parse(Request.Form["SEL_SetTmp"]).ToString("X2");

                sendArray.Add("06000400" + setTmp);
            }

            string sendData = string.Empty;
            string addCode = JsonConvert.DeserializeObject<Dictionary<string, string>>(device_Info.PropertyJson)["addCode"];

            foreach (string s in sendArray)
            {
                sendData = BLL.BLLHelper.CRC_16(addCode + s);

                SendData(devID, sendData);

                Thread.Sleep(500);
            }

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
                retArray.Add(byteData[5]);

                //模式
                retArray.Add(byteData[7]);

                //设置温度
                retArray.Add(Int32.Parse(byteData[8], System.Globalization.NumberStyles.HexNumber).ToString() + "." + Int32.Parse(byteData[9], System.Globalization.NumberStyles.HexNumber).ToString());

                //风机模式
                retArray.Add(byteData[11]);

                //机型
                retArray.Add(byteData[13]);

                //低温保护温度
                retArray.Add(Int32.Parse(byteData[16], System.Globalization.NumberStyles.HexNumber).ToString() + "." + Int32.Parse(byteData[17], System.Globalization.NumberStyles.HexNumber).ToString());

                //室内温度
                retArray.Add(Int32.Parse(byteData[18], System.Globalization.NumberStyles.HexNumber).ToString() + "." + Int32.Parse(byteData[19], System.Globalization.NumberStyles.HexNumber).ToString());

                //防冻功能
                retArray.Add(byteData[23]);

                //键盘锁定
                retArray.Add(byteData[25]);
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
