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

        public HybridSearchService(string url)
        {
            this.url = url;
        }

        public SearchItem PollService(Guid agentId, Guid customerId)
        {
            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("CustomerId", customerId.ToString());
                client.DefaultRequestHeaders.Add("AgentId", agentId.ToString());
                HttpResponseMessage result = client.GetAsync("/AgentPoll").Result;
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

        public List<ResultDataFromAgent> SearchFileInAllDevices(string query, Guid customerId)
        {


            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("CustomerId", customerId.ToString());
                HttpResponseMessage result = client.GetAsync("/GetSearch?q=" + query).Result;
                resultContent = result.Content.ReadAsStringAsync().Result;
            }


            if (!string.IsNullOrEmpty(resultContent))
            {
                SearchResults searchResult = JsonConvert.DeserializeObject<SearchResults>(content);

                List<ResultDataFromAgent> resultDataFromAgent = new List<ResultDataFromAgent>();

                foreach (var item in searchResult.results)
                {
                    ResultDataFromAgent tempResult = new ResultDataFromAgent()
                    {
                        AgentGuid = item.agentId,
                        FilesMetadata = GetFilesMetadataFromByte(item.result),
                        ResultType = ResultDataFromAgentType.FilesMetadataList
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

        public ResultDataFromAgent GetFileFromDevice(string path, Guid agentId, Guid customerId)
        {
            string resultContent;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("CustomerId", customerId.ToString());
                client.DefaultRequestHeaders.Add("AgentId", agentId.ToString());

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

        public void SendResult(Guid customerId, Guid agentId, Guid requestId, ResultDataFromAgent agentResult)
        {

            if (agentResult.ResultType == ResultDataFromAgentType.FileContent)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Add("CustomerId", customerId.ToString());
                    client.DefaultRequestHeaders.Add("AgentId", agentId.ToString());
                    HttpContent content = new ByteArrayContent(CreateByteArrayFromFileContent(agentResult.FileContent));
                    HttpResponseMessage result = client.PostAsync("/PostResults", content).Result;
                }
            }
            else if (agentResult.ResultType == ResultDataFromAgentType.FilesMetadataList)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Add("CustomerId", customerId.ToString());
                    client.DefaultRequestHeaders.Add("AgentId", agentId.ToString());
                    client.DefaultRequestHeaders.Add("RequestId", requestId.ToString());

                    HttpContent content = new ByteArrayContent(CreateByteArrayFromFilesMetadata(agentResult.FilesMetadata));
                    HttpResponseMessage result = client.PostAsync("/PostResults", content).Result;
                }
            }
        }

    }
}