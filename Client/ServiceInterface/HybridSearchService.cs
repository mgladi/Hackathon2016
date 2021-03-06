﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ServiceInterface
{
    public class HybridSearchService : IService
    {
        private readonly string url;
        private readonly string customerName;
        private Guid agentId;

        public HybridSearchService(string url, string customerName)
        {
            this.url = url;
            this.customerName = customerName;
            this.agentId = Guid.NewGuid();
        }

        public SearchItem PollService()
        {
            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url +"/AgentPoll");
                client.DefaultRequestHeaders.Add("CustomerId", customerName);
                client.DefaultRequestHeaders.Add("AgentId", agentId.ToString());
                HttpResponseMessage result = client.GetAsync("").Result;
                resultContent = result.Content.ReadAsStringAsync().Result;
            }

            if (!string.IsNullOrEmpty(resultContent))
            {
                JObject obj = JObject.Parse(resultContent);
                var type = obj["type"].ToString();
                var query = obj["query"].ToString();
                var requestId = obj["requestId"].ToString();
                return new SearchItem()
                {
                    PollingResultType = type == "search" ? PollingResultType.SearchQuery : PollingResultType.FileToTransferPath,
                    ResultQuery = query,
                    RequestId = Guid.Parse(requestId)
                };
            }
            else
            {
                return new SearchItem()
                {
                    PollingResultType = PollingResultType.NoRequest
                };
            }
        }

        public List<ResultDataFromAgent> SearchFileInAllDevices(string query)
        {
            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("CustomerId", customerName);
                HttpResponseMessage result = client.GetAsync("/GetSearch?q=" + query).Result;
                resultContent = result.Content.ReadAsStringAsync().Result;
            }


            if (!string.IsNullOrEmpty(resultContent))
            {
                SearchResults searchResult = JsonConvert.DeserializeObject<SearchResults>(resultContent);

                List<ResultDataFromAgent> resultDataFromAgent = new List<ResultDataFromAgent>();

                foreach (var item in searchResult.results)
                {
                    ResultDataFromAgent tempResult = new ResultDataFromAgent()
                    {
                        AgentGuid = item.agentId,
                        FilesMetadata = item.result != null ? GetFilesMetadataFromByte(item.result) : new List<FileMetadata>(),
                        ResultType = ResultDataFromAgentType.FilesMetadataList, 
                        DeviceType = (DeviceType)Enum.Parse(typeof(DeviceType), item.deviceType),
                        DeviceName = item.deviceName
                    };
                    resultDataFromAgent.Add(tempResult);

                }
                return resultDataFromAgent;
            }
            else
            {
                throw new InvalidDataException("No data");
            }
        }

        public ResultDataFromAgent GetFileFromDevice(string path, Guid agentIdForFile)
        {
            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("CustomerId", customerName);
                client.DefaultRequestHeaders.Add("AgentId", agentIdForFile.ToString());

                HttpResponseMessage result = client.GetAsync("/GetFile?p=" + path).Result;
                resultContent = result.Content.ReadAsStringAsync().Result;
            }


            if (!string.IsNullOrEmpty(resultContent))
            {
                SearchResults searchResult = JsonConvert.DeserializeObject<SearchResults>(resultContent);

                List<ResultDataFromAgent> resultDataFromAgent = new List<ResultDataFromAgent>();

                AgentResult res = searchResult.results[0];
                    ResultDataFromAgent tempResult = new ResultDataFromAgent()
                    {
                        AgentGuid = res.agentId,
                        FileContent = GetFileContentFromByte(res.result),
                        ResultType = ResultDataFromAgentType.FileContent
                    };
                    resultDataFromAgent.Add(tempResult);

                return tempResult;
            }
            else
            {
                throw new InvalidDataException("No data");
            }
        }

        private string GetFileContentFromByte(byte[] result)
        {
            return Encoding.UTF8.GetString(result,0,result.Length);
        }

        private List<FileMetadata> GetFilesMetadataFromByte(byte[] result)
        {
            return JsonConvert.DeserializeObject<List<FileMetadata>>(Encoding.UTF8.GetString(result, 0, result.Length));
        }
        private byte[] CreateByteArrayFromFilesMetadata(List<FileMetadata> filesMetadata)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(filesMetadata));
        }

        private byte[] CreateByteArrayFromFileContent(string fileContent)
        {
            return Encoding.UTF8.GetBytes(fileContent);
        }

        public void SendResult(Guid requestId, ResultDataFromAgent agentResult)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("CustomerId", customerName);
                client.DefaultRequestHeaders.Add("AgentId", agentId.ToString());
                client.DefaultRequestHeaders.Add("RequestId", requestId.ToString());
                client.DefaultRequestHeaders.Add("DeviceType", agentResult.DeviceType.ToString());
                client.DefaultRequestHeaders.Add("DeviceName", agentResult.DeviceName.ToString());
                client.DefaultRequestHeaders.Add("SearchDuration", agentResult.SearchDuration);

                HttpContent content = null;
                if (agentResult.ResultType == ResultDataFromAgentType.FileContent)
                {
                    content = new ByteArrayContent(CreateByteArrayFromFileContent(agentResult.FileContent));
                }
                else if (agentResult.ResultType == ResultDataFromAgentType.FilesMetadataList)
                {
                    content = new ByteArrayContent(CreateByteArrayFromFilesMetadata(agentResult.FilesMetadata));
                }

                HttpResponseMessage result = client.PostAsync("/PostResults", content).Result;
            }
        }

        public void Register(string deviceName, string deviceType)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("CustomerId", customerName);
                client.DefaultRequestHeaders.Add("AgentId", agentId.ToString());
                client.DefaultRequestHeaders.Add("DeviceName", deviceName);
                client.DefaultRequestHeaders.Add("DeviceType", deviceType);

                HttpResponseMessage result = client.GetAsync("/Register").Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                this.agentId = JsonConvert.DeserializeObject<Guid>(resultContent);
            }
        }

        public List<AgentData> GetAgentsStatus()
        {
            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("CustomerId", customerName);
                HttpResponseMessage result = client.GetAsync("/GetStatus").Result;
                resultContent = result.Content.ReadAsStringAsync().Result;
            }


            if (!string.IsNullOrEmpty(resultContent))
            {
                List<AgentData> agentData = JsonConvert.DeserializeObject<List<AgentData>>(resultContent);
                return agentData;
            }
            else
            {
                throw new InvalidDataException("No data");
            }
        }
    }
}
