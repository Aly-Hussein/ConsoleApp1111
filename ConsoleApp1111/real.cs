
namespace CoreReceiverApp
{
    using Microsoft.Azure.ServiceBus;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Net.Http.Headers;
    using System.Web;
    using Newtonsoft.Json.Linq;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;




    public class Word
    {
        public string boundingBox { get; set; }
        public string text { get; set; }
    }

    public class Line
    {
        public string boundingBox { get; set; }
        public List<Word> words { get; set; }
    }

    public class Region
    {
        public string boundingBox { get; set; }
        public List<Line> lines { get; set; }
    }

    public class Root1
    {
        public string language { get; set; }
        public double textAngle { get; set; }
        public string orientation { get; set; }
        public List<Region> regions { get; set; }
    }



    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
    public class Root
    {
        public string LicensePlateCaptureTime { get; set; }
        public string LicensePlate { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ContextImageJpg { get; set; }
        public string LicensePlateImageJpg { get; set; }
    }
    class Program
    {
        public static Root1 json1;
        public static string modifiedplate;
        const string ServiceBusConnectionString = "Endpoint=sb://licenseplatepublisher.servicebus.windows.net/;SharedAccessKeyName=ConsumeReads;SharedAccessKey=VNcJZVQAVMazTAfrssP6Irzlg/pKwbwfnOqMXqROtCQ=";
        const string TopicName = "licenseplateread";
        const string SubscriptionName = "elbtjksehwhrcpwf";
        static ISubscriptionClient subscriptionClient;
        static string url;
        public class imageobect
        {
          
            public string Imageurl { get; set; }
        }
        public static int distancenumber(string plate, string list)
        {
            int value = 0;

            for (int i = 0; i < plate.Length; i++)
            {
                if (plate[i] == list[i])
                {

                }
                else
                if ((plate[i] == 'K' || plate[i] == 'X' || plate[i] == 'Y') && (list[i] == 'K' || list[i] == 'X' || list[i] == 'Y'))
                {
                    value++;
                }
                else
                if ((plate[i] == 'B' || plate[i] == '8') && (list[i] == 'B' || list[i] == '8'))
                {
                    value++;
                }
                else
                if ((plate[i] == 'C' || plate[i] == 'G') && (list[i] == 'C' || list[i] == 'G'))
                {
                    value++;
                }

                else
                if ((plate[i] == 'E' || plate[i] == 'F') && (list[i] == 'E' || list[i] == 'F'))
                {
                    value++;
                }
                else
                if ((plate[i] == 'I' || plate[i] == '1' || plate[i] == 'T' || plate[i] == 'J') && (list[i] == 'I' || list[i] == '1' || list[i] == 'T' || list[i] == 'J'))
                {
                    value++;
                }
                else
                if ((plate[i] == 'S' || plate[i] == '5') && (list[i] == 'S' || list[i] == '5'))
                {
                    value++;
                }
                else
                if ((plate[i] == 'O' || plate[i] == 'D' || plate[i] == 'Q' || plate[i] == '0') && (list[i] == 'O' || list[i] == 'D' || list[i] == 'Q' || list[i] == '0'))
                {
                    value++;
                }
                else
                if ((plate[i] == 'P' || plate[i] == 'R') && (list[i] == 'P' || list[i] == 'R'))
                {
                    value++;
                }
                else
                if ((plate[i] == 'Z' || plate[i] == '2') && (list[i] == 'Z' || list[i] == '2'))
                {
                    value++;
                }
                
                
            }

            return value;
        }

        public static string getfinalstring(string plate, string list)
        {

            modifiedplate = "";
            for (int i = 0; i < plate.Length; i++)
            {
                if (plate[i] == list[i])
                {
                    modifiedplate += list[i];
                }
                else
                if ((plate[i] == 'K' || plate[i] == 'X' || plate[i] == 'Y') && (list[i] == 'K' || list[i] == 'X' || list[i] == 'Y'))
                {
                    modifiedplate += list[i];
                }
                else
                if ((plate[i] == 'B' || plate[i] == '8') && (list[i] == 'B' || list[i] == '8'))
                {
                    modifiedplate += list[i];
                }
                else
                if ((plate[i] == 'C' || plate[i] == 'G') && (list[i] == 'C' || list[i] == 'G'))
                {
                    modifiedplate += list[i];
                }
                else

                if ((plate[i] == 'E' || plate[i] == 'F') && (list[i] == 'E' || list[i] == 'F'))
                {
                    modifiedplate += list[i];
                }
                else
                if ((plate[i] == 'I' || plate[i] == '1' || plate[i] == 'T' || plate[i] == 'J') && (list[i] == 'I' || list[i] == '1' || list[i] == 'T' || list[i] == 'J'))
                {
                    modifiedplate += list[i];
                }
                else
                if ((plate[i] == 'S' || plate[i] == '5') && (list[i] == 'S' || list[i] == '5'))
                {
                    modifiedplate += list[i];
                }
                else
                if ((plate[i] == 'O' || plate[i] == 'D' || plate[i] == 'Q' || plate[i] == '0') && (list[i] == 'O' || list[i] == 'D' || list[i] == 'Q' || list[i] == '0'))
                {
                    modifiedplate += list[i];
                }
                else
                if ((plate[i] == 'P' || plate[i] == 'R') && (list[i] == 'P' || list[i] == 'R'))
                {
                    modifiedplate += list[i];
                }
                else
                if ((plate[i] == 'Z' || plate[i] == '2') && (list[i] == 'Z' || list[i] == '2'))
                {
                    modifiedplate += list[i];
                }
                else
                    modifiedplate += plate[i];
               

            }

            return modifiedplate;
        }
        public static async Task<string> UploadImage(string fileName, FileStream inputStream)
        {

            string storageConnection = "DefaultEndpointsProtocol=https;AccountName=stteam25strathack;AccountKey=tVAqnucH9vZy3rAF7vZcLbDigT1tZHOD+fudXHFBDQwHwkhzyubDp2pWLycuK9zS5R4BouyPn7vkK9eM809IHQ==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

            //create a block blob 
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            //create a container 
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("appcontainer");
            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {

                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            }

            string imageName = "wanted" + Guid.NewGuid() + ".jpg";

            //get Blob reference

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}", cloudBlockBlob.Uri);
            //cloudBlockBlob.Properties.ContentType = inputStream.ContentType;

            await cloudBlockBlob.UploadFromStreamAsync(inputStream);

            return cloudBlockBlob.Uri.ToString();

        }
        public static async Task Main(string[] args)
        {
            Class1.initializelist();

            var task1 = Class1.Mainabc();
            var task2 = Main123();
            await Task.WhenAll(task1, task2);
        }
        public static async Task Main123()
        {


            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            // Register subscription message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await subscriptionClient.CloseAsync();
        }
        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 10,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            string body = Encoding.UTF8.GetString(message.Body);



            Root json = JsonConvert.DeserializeObject<Root>(body);
            string contextimage = json.ContextImageJpg;
            string capturetime = json.LicensePlateCaptureTime;
            string liscencePlate = json.LicensePlate;
            string lat = json.Latitude;
            string lon = json.Longitude;
            string licenseimg = json.LicensePlateImageJpg;









            if (Class1.Compare(liscencePlate))
            {




                File.WriteAllBytes(@"temp.jpg", Convert.FromBase64String(contextimage));


                await using (FileStream my_stream = new FileStream("temp.jpg", FileMode.Open, FileAccess.Read))
                {
                    url = await UploadImage(@"temp.jpg", my_stream);
                }




                body = "{ " + '\u0022' + "LicensePlateCaptureTime" + '\u0022' + ": " + '\u0022' + capturetime + '\u0022' + "," + " " + '\u0022' + "LicensePlate" + '\u0022' + ": " + '\u0022' + liscencePlate + '\u0022' + ", " + '\u0022' + "Latitude" + '\u0022' + ": " + lat + ", " + '\u0022' + "Longitude" + '\u0022' + ": " + lon + ", " + '\u0022' + "ContextImageReference" + '\u0022' + ": " + '\u0022' + url + '\u0022' + " }";




                Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{body}");
                var client = new HttpClient();
                var content = new StringContent(body);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "dGVhbTI1OnRGbF9ZaWF4OFtpX2k8bHQ=");

                var result = await client.PostAsync("https://licenseplatevalidator.azurewebsites.net/api/lpr/platelocation", content);
                Console.WriteLine(result);

            }

            else if (!Class1.Compare(liscencePlate))
            {
                string listarraymaker = Class1.list.Replace('\u0022', ' ').Trim('[', ']');
                string[] listarray = listarraymaker.Split(",");
                for (int i = 0; i < listarray.Length; i++)
                {

                    

                        

                        modifiedplate = getfinalstring(liscencePlate, listarray[i].Substring(1, 6));
                    Console.WriteLine("done with fuzzy");

                    if (Class1.Compare(modifiedplate))
                    {

                        


                        File.WriteAllBytes(@"temp.jpg", Convert.FromBase64String(contextimage));


                        await using (FileStream my_stream = new FileStream("temp.jpg", FileMode.Open, FileAccess.Read))
                        {
                            url = await UploadImage(@"temp.jpg", my_stream);
                        }




                        body = "{ " + '\u0022' + "LicensePlateCaptureTime" + '\u0022' + ": " + '\u0022' + capturetime + '\u0022' + "," + " " + '\u0022' + "LicensePlate" + '\u0022' + ": " + '\u0022' + modifiedplate + '\u0022' + ", " + '\u0022' + "Latitude" + '\u0022' + ": " + lat + ", " + '\u0022' + "Longitude" + '\u0022' + ": " + lon + ", " + '\u0022' + "ContextImageReference" + '\u0022' + ": " + '\u0022' + url + '\u0022' + " }";



                        Console.WriteLine("using modified license");
                        Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{body}");
                        var client = new HttpClient();
                        var content = new StringContent(body);
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "dGVhbTI1OnRGbF9ZaWF4OFtpX2k8bHQ=");

                        var result = await client.PostAsync("https://licenseplatevalidator.azurewebsites.net/api/lpr/platelocation", content);
                        Console.WriteLine(result);
                    }






                }
                
            }

            //{
            //    File.WriteAllBytes(@"temp1.jpg", Convert.FromBase64String(licenseimg));


            //    await using (FileStream my_stream = new FileStream("temp1.jpg", FileMode.Open, FileAccess.Read))
            //    {
            //        url = await UploadImage(@"temp1.jpg", my_stream);
            //    }

            //    //{"url":"http://example.com/images/test.jpg"}

            //    string jsonFormatthing = "{" + '\u0022' + "url" + '\u0022' + ":" + '\u0022' + url + '\u0022' + "}";

            //    //imageobect newobj = JsonConvert.DeserializeObject<imageobect>(jsonFormatthing);




            //    var client1 = new HttpClient();
            //    var queryString = HttpUtility.ParseQueryString(string.Empty);

            //    // Request headers
            //    client1.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "c02d4eaab3094399b232e2fb1f6a436e");

            //    // Request parameters
            //    queryString["language"] = "unk";
            //    queryString["detectOrientation "] = "true";
            //    var uri = "https://eastus.api.cognitive.microsoft.com/vision/v1.0/ocr?" + queryString;

            //    HttpResponseMessage response;
            //    // "url":"http://example.com/images/test.jpg"
            //    // Request body


            //    byte[] byteData = Encoding.UTF8.GetBytes(jsonFormatthing);

            //    using (var content = new ByteArrayContent(byteData))
            //    {
            //        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //        response = await client1.PostAsync(uri, content);

            //    }
            //    string contentString = await response.Content.ReadAsStringAsync();


            //    json1 = JsonConvert.DeserializeObject<Root1>(contentString);

            //    for (int i = 0; i < json1.regions.Count; i++)
            //    {
            //        for (int j = 0; j < json1.regions[i].lines.Count; j++)
            //        {
            //            for (int a = 0; a < json1.regions[i].lines[j].words.Count; a++)
            //            {
            //                string bob = json1.regions[i].lines[j].words[a].text;
            //                if (bob.Length < 6)
            //                { bob += json1.regions[i].lines[j].words[a--].text; }
            //                if (bob.Length == 6 || bob.Length == 7)
            //                {


            //                    if (Class1.Compare(bob))
            //                    {
            //                        File.WriteAllBytes(@"temp2.jpg", Convert.FromBase64String(contextimage));


            //                        await using (FileStream my_stream = new FileStream("temp2.jpg", FileMode.Open, FileAccess.Read))
            //                        {
            //                            url = await UploadImage(@"temp2.jpg", my_stream);
            //                        }


            //                        Console.WriteLine(contentString);

            //                        body = "{ " + '\u0022' + "LicensePlateCaptureTime" + '\u0022' + ": " + '\u0022' + capturetime + '\u0022' + "," + " " + '\u0022' + "LicensePlate" + '\u0022' + ": " + '\u0022' + bob + '\u0022' + ", " + '\u0022' + "Latitude" + '\u0022' + ": " + lat + ", " + '\u0022' + "Longitude" + '\u0022' + ": " + lon + ", " + '\u0022' + "ContextImageReference" + '\u0022' + ": " + '\u0022' + url + '\u0022' + " }";




            //                        Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{body}");
            //                        var client = new HttpClient();
            //                        var content = new StringContent(body);
            //                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "dGVhbTI1OnRGbF9ZaWF4OFtpX2k8bHQ=");

            //                        var result = await client.PostAsync("https://licenseplatevalidator.azurewebsites.net/api/lpr/platelocation", content);
            //                        Console.WriteLine(result);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            // Complete the message so that it is not received again.
            // This can be done only if the subscriptionClient is created in ReceiveMode.PeekLock mode (which is the default).
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the subscriptionClient has already been closed.
            // If subscriptionClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}