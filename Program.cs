using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Symbol.RFID3;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Text.Json;

using System.Threading.Tasks;

namespace RFIDSecurity
{
    class Program
    {
        
        public static string hostname = "192.168.1.125";
        private static RFIDReader rfid3 = new RFIDReader(hostname, 5084, 0);

        static void Main(string[] args)
        {


            try
            {
                Console.WriteLine("Connecting...");

                rfid3.Connect();
                ushort[] antID = rfid3.Config.Antennas.AvailableAntennas;
                Antennas.Config antConfig = rfid3.Config.Antennas[antID[0]].GetConfig();
                int TPI = 0;
                antConfig.TransmitPowerIndex = (ushort)TPI;
                rfid3.Config.Antennas[antID[0]].SetConfig(antConfig);

                rfid3.Actions.PurgeTags();
                
                //rfid3.Events.NotifyInventoryStartEvent = true;
               

              
                
                //rfid3.Config.GPO[1].PortState = GPOs.GPO_PORT_STATE.FALSE;

                //ReadTags(null);
                //rfid3.Config.GPO[1].PortState = GPOs.GPO_PORT_STATE.FALSE;
                
                while (true)
                {
                    rfid3.Actions.Inventory.Perform();
                    Thread.Sleep(300);

                    rfid3.Actions.Inventory.Stop();
                    List<string> tags = new();
                    TagData[] tagData = rfid3.Actions.GetReadTags(1000);
                    
                    
                

                   
                    if (tagData != null)
                    {

                        foreach (var tag in tagData)
                        {
                            if (!tags.Contains(tag.TagID))
                            {
                                tags.Add(tag.TagID);

                            }


                        }

                        if (tags.Count > 0)
                        {



                            //foreach (var tag in tags)
                            //{
                            //    //Thread.Sleep(1000);
                            //    Console.WriteLine(tag);

                            //}
                            //tags.Clear();

                            //Console.WriteLine(tags.Count);
                            using (var client = new WebClient()) //WebClient  
                            {
                                //string a = "hehe";
                                client.Headers.Add("Content-Type:application/json"); //Content-Type  
                                client.Headers.Add("Accept:application/json");
                                string result = client.UploadString("https://localhost:44342/RFID/checkRFID", "POST", JsonConvert.SerializeObject(tags)); //URI  


                                Tag deptObj = System.Text.Json.JsonSerializer.Deserialize<Tag>(result);
                                if (deptObj.unpaids.Count > 0)
                                {
                                    //rfid3.Config.GPO[1].PortState = GPOs.GPO_PORT_STATE.TRUE;

                                    Console.WriteLine("Yes");
                                    //c = 0;
                                }
                                else
                                {
                                    //rfid3.Config.GPO[1].PortState = GPOs.GPO_PORT_STATE.FALSE;
                                    Console.WriteLine("No");
                                }


                            }

                            tags.Clear();
                            //tagData = null;
                            //CallWebAPI(tags);

                        }
                        else
                        {
                            Console.WriteLine("No");
                        }

                    }
                    else
                    {
                        Console.WriteLine("No");
                    }

                }
                //CallWebAPI();
                //Console.WriteLine("Lên đèn!!!!");

            }
            catch (OperationFailureException operationException)
            {
                Console.WriteLine(operationException.StatusDescription);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //public static async Task DoSomethingEveryTenSeconds()
        //{
        //    while (true)
        //    {
        //        // wait until at least 10s elapsed since delayTask created
        //    }
        //}


    }
    }
  


