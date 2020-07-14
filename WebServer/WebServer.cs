using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;

namespace YoutubeGameBarWidget.WebServer
{
    class WebServer : IDisposable
    {
        private const uint bufferSize = 8192;
        private StreamSocketListener listener;
        private StorageFolder videoUIDirectory = Windows.ApplicationModel.Package.Current.InstalledLocation;

        public void Dispose()
        {
            this.listener.Dispose();
        }

        public WebServer()
        {
            this.listener = new StreamSocketListener();
            this.listener.ConnectionReceived += (sender, eventArgs) => ProcessRequestAsync(eventArgs.Socket);
            this.listener.BindServiceNameAsync(Environment.GetEnvironmentVariable("YTGBWS_PORT"));
        }

        /// <summary>
        /// Responsible for processing (reading, validating and calling answerer) the request asynchronously.
        /// </summary>
        /// <param name="socket">The socket received from the Event Arguments.</param>
        public async void ProcessRequestAsync(StreamSocket socket)
        {
            StringBuilder request = new StringBuilder();

            using (IInputStream input = socket.InputStream)
            {
                byte[] data = new byte[bufferSize];
                IBuffer buffer = data.AsBuffer();
                uint dataRead = bufferSize;

                while (dataRead == bufferSize)
                {
                    await input.ReadAsync(buffer, bufferSize, InputStreamOptions.Partial);
                    request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                    dataRead = buffer.Length;
                }
            }

            using (IOutputStream output = socket.OutputStream)
            {
                string requestMethod = request.ToString().Split('\n').First();
                string[] requestParts = requestMethod.Split(' ');

                if (requestParts[0] == "GET") { await WriteResponseAsync(requestParts[1], output); }
                else { throw new InvalidDataException("Request Method not supported: " + requestParts[0]); }
            }
        }

        /// <summary>
        /// Task responsible for answering the HTTP request with the correct file (if it exists).
        /// </summary>
        /// <param name="path">The filepath provided by the Request.</param>
        /// <param name="output">The output stream object.</param>
        /// <returns></returns>
        private async Task WriteResponseAsync(string path, IOutputStream output)
        {
            using (Stream response = output.AsStreamForWrite())
            {
                string filePath = "VideoUI\\";
                string fileExtension = "";
                byte[] headerArray;

                try
                {
                    //Due to the VideoUI's implementation, the mediaUrl comes in a QueryString. 
                    //So the path needs to be checked to return the correct file.
                    if(Regex.IsMatch(path, "/[?](.+)"))
                    {
                         filePath += "index.html";
                    }
                    else
                    {
                        filePath += path.Replace('/', '\\');
                    }

                    fileExtension = filePath.Split('.').Last();

                    using (Stream fs = await videoUIDirectory.OpenStreamForReadAsync(filePath))
                    {
                        string headerString = String.Format("HTTP/1.1 200 OK\r\n" +
                                                      "Content-Length: {0}\r\n" +
                                                      "{1}\r\n" +
                                                      "Connection: close\r\n\r\n",
                                                      fs.Length,
                                                      determinateContentType(fileExtension));
                        headerArray = Encoding.UTF8.GetBytes(headerString);
                        await response.WriteAsync(headerArray, 0, headerArray.Length);
                        await fs.CopyToAsync(response);
                    }
                }
                catch (FileNotFoundException)
                {
                    headerArray = Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found\r\n" +
                                                         "Content-Length:0\r\n" +
                                                         "Connection: close\r\n\r\n");
                    await response.WriteAsync(headerArray, 0, headerArray.Length);
                }

                await response.FlushAsync();
            }
        }

        /// <summary>
        /// Determinates the Header's Content-Type entity content based on the given file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension to be sent as response.</param>
        /// <returns></returns>
        private string determinateContentType(string fileExtension)
        {
            Dictionary<string, string> contentType = new Dictionary<string, string>();
            contentType.Add("js", "Content-Type: text/javascript");
            contentType.Add("css", "Content-Type: text/css; charset=\"utf-8\"");
            contentType.Add("html", "Content-Type: text/html");
            contentType.Add("png", "Content-Type: image/png");

            switch (fileExtension)
            {
                case "js":
                    return contentType["js"];
                case "css":
                    return contentType["css"];
                case "png":
                    return contentType["png"];
                default:
                    return contentType["html"];
            }
        }
    }
}
